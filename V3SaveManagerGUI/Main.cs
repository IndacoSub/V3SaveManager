using System.Globalization;
using V3SaveManager.Core;
using V3SaveManagerGUI.Editors;

namespace V3SaveManagerGUI;

/// <summary>Guided editors for everyday changes, with a separate full-schema expert workspace.</summary>
public sealed class Main : Form
{
    private readonly ToolStrip _toolbar = new() { GripStyle = ToolStripGripStyle.Hidden, Padding = new Padding(6) };
    private readonly ToolStripButton _save = new("Save");
    private readonly ToolStripButton _saveAs = new("Save as…");
    private readonly ToolStripButton _undo = new("Undo");
    private readonly ToolStripButton _redo = new("Redo");
    private readonly ToolStripButton _expert = new("Expert mode") { CheckOnClick = true };
    private readonly ToolStripTextBox _search = new() { AutoSize = false, Width = 180, ToolTipText = "Filter by field name or category" };
    private readonly ToolStripLabel _filterLabel = new("Filter:");
    private readonly Panel _guided = new() { Dock = DockStyle.Fill, BackColor = SystemColors.Window };
    private readonly Dictionary<GuidedEditorKind, Button> _editorCards = new();
    private readonly DataGridView _fields = NewGrid();
    private readonly DataGridView _bytes = NewGrid();
    private readonly SplitContainer _split = new() { Dock = DockStyle.Fill, Orientation = Orientation.Horizontal, Panel2Collapsed = true };
    private readonly Label _byteLabel = new() { Dock = DockStyle.Top, Height = 30, Padding = new Padding(6), Text = "Select a field to edit its bytes." };
    private readonly ToolStripStatusLabel _status = new() { Spring = true, TextAlign = ContentAlignment.MiddleLeft };
    private readonly Dictionary<string, ValueFormat> _formats = new();
    private readonly Stack<byte[]> _undoHistory = new();
    private readonly Stack<byte[]> _redoHistory = new();
    private List<SaveField> _visible = new();
    private SaveDocument? _document;
    private byte[]? _baseline;
    private SaveField? _selectedField;
    private byte[] _selectedBytes = Array.Empty<byte>();
    private string? _path;
    private bool _refreshing;
    private bool _byteGridActive;
    private bool _changingMode;
    private bool _expertOpened;
    private readonly HashSet<Control> _dropTargets = new();

    public Main()
    {
        Text = "V3 Save Manager";
        MinimumSize = new Size(950, 620);
        Size = new Size(1240, 860);
        StartPosition = FormStartPosition.CenterScreen;
        Font = new Font("Segoe UI", 10);
        var open = new ToolStripButton("Open…"); open.Click += (_, _) => Open();
        _save.Click += (_, _) => Save(false);
        _saveAs.Click += (_, _) => Save(true);
        _undo.Click += (_, _) => Undo();
        _redo.Click += (_, _) => Redo();
        _expert.CheckedChanged += (_, _) => ChangeMode();
        _search.TextChanged += (_, _) => { if (CommitEdit()) RefreshFields(); };
        _toolbar.Items.AddRange(new ToolStripItem[] { open, _save, _saveAs, new ToolStripSeparator(), _undo, _redo,
            new ToolStripSeparator(), _expert, new ToolStripSeparator(), _filterLabel, _search });
        var help = new ToolStripButton("Help"); help.Click += (_, _) => ShowHelp(); _toolbar.Items.Add(help);

        _fields.Columns.Add(new DataGridViewTextBoxColumn { Name = "Field", HeaderText = "Field", Width = 200, ReadOnly = true });
        _fields.Columns.Add(new DataGridViewTextBoxColumn { Name = "Offset", HeaderText = "Offset", Width = 95, ReadOnly = true });
        _fields.Columns.Add(new DataGridViewTextBoxColumn { Name = "Length", HeaderText = "Bytes", Width = 70, ReadOnly = true });
        _fields.Columns.Add(new DataGridViewComboBoxColumn { Name = "Format", HeaderText = "Format", Width = 110,
            FlatStyle = FlatStyle.Flat, DisplayStyle = DataGridViewComboBoxDisplayStyle.DropDownButton });
        _fields.Columns.Add(new DataGridViewTextBoxColumn { Name = "Value", HeaderText = "Value (double-click or F2 to edit)",
            AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, MinimumWidth = 240, MaxInputLength = int.MaxValue });
        _fields.CellValidating += ValidateField;
        _fields.CellEndEdit += (_, e) =>
        {
            if (e.RowIndex < 0 || _document is null || _refreshing) return;
            _fields.Rows[e.RowIndex].ErrorText = "";
            var field = _visible[e.RowIndex];
            _fields.Rows[e.RowIndex].Cells["Value"].Value = _document.GetValue(field.Name, _formats[field.Name]);
        };
        _fields.CurrentCellDirtyStateChanged += (_, _) =>
        {
            if (_fields.IsCurrentCellDirty && _fields.CurrentCell?.ColumnIndex == 3)
                _fields.CommitEdit(DataGridViewDataErrorContexts.Commit);
        };
        _fields.CellValueChanged += ChangeFormat;
        _fields.SelectionChanged += (_, _) => LoadBytes();
        _fields.DataError += (_, e) => { e.ThrowException = false; _status.Text = "Invalid cell value. Choose an available format."; };
        _fields.KeyDown += GridKeys;
        _fields.Enter += (_, _) => _byteGridActive = false;
        _fields.ContextMenuStrip = GridMenu();

        _bytes.VirtualMode = true;
        _bytes.Columns.Add(new DataGridViewTextBoxColumn { Name = "Offset", HeaderText = "Offset", Width = 105, ReadOnly = true });
        for (int c = 0; c < 16; c++) _bytes.Columns.Add(new DataGridViewTextBoxColumn { Name = "B" + c,
            HeaderText = c.ToString("X2"), Width = 52, MaxInputLength = 2, SortMode = DataGridViewColumnSortMode.NotSortable });
        foreach (DataGridViewColumn column in _fields.Columns) column.SortMode = DataGridViewColumnSortMode.NotSortable;
        foreach (DataGridViewColumn column in _bytes.Columns) column.SortMode = DataGridViewColumnSortMode.NotSortable;
        _bytes.DefaultCellStyle.Font = new Font("Consolas", 10);
        _bytes.CellValueNeeded += ByteValueNeeded;
        _bytes.CellValidating += ValidateByte;
        _bytes.CellEndEdit += (_, e) => { if (e.RowIndex >= 0) _bytes.Rows[e.RowIndex].ErrorText = ""; };
        _bytes.KeyDown += GridKeys;
        _bytes.Enter += (_, _) => _byteGridActive = true;
        _bytes.ContextMenuStrip = GridMenu();
        _bytes.CellBeginEdit += (_, e) => { if (e.ColumnIndex == 0 || e.RowIndex * 16 + e.ColumnIndex - 1 >= _selectedBytes.Length) e.Cancel = true; };
        _split.Panel1.Controls.Add(_fields);
        _split.Panel2.Controls.Add(_bytes);
        _split.Panel2.Controls.Add(_byteLabel);
        CreateGuidedView();
        var status = new StatusStrip(); status.Items.Add(_status);
        Controls.Add(_split); Controls.Add(_guided); Controls.Add(status); Controls.Add(_toolbar);
        RegisterDropTargets(this);
        Shown += (_, _) => UpdateState();
        FormClosing += (_, e) => { if (!ConfirmUnsaved()) e.Cancel = true; };
        UpdateState();
    }

    private static readonly (GuidedEditorKind Kind, string Title, string Description)[] Editors =
    {
        (GuidedEditorKind.Currency, "Coins", "Edit Monocoins and casino coins."),
        (GuidedEditorKind.Playtime, "Playtime", "Edit frames or convert hours, minutes and seconds."),
        (GuidedEditorKind.Level, "Level and EXP", "Calculate EXP and keep both level fields synchronized."),
        (GuidedEditorKind.Flags, "Flags", "Change the game-clear flag with a checkbox."),
        (GuidedEditorKind.Map, "Map and position", "Choose a named map and edit X / Y coordinates."),
        (GuidedEditorKind.Characters, "Character statuses", "Choose Alive, Dead or Unknown for each character."),
        (GuidedEditorKind.Files, "Files and script", "Edit archive names, script references and current line."),
        (GuidedEditorKind.Audio, "Audio", "Edit the music index, audio references and volumes.")
    };

    private void CreateGuidedView()
    {
        var introduction = new Label
        {
            Dock = DockStyle.Top, Height = 76, Padding = new Padding(18, 12, 18, 4),
            Text = "Save editors — open or drag a .dat file into this window.\nChoose an editor below. Use Expert mode for the complete field and byte grids.",
            AutoEllipsis = true
        };
        var cards = new FlowLayoutPanel { Dock = DockStyle.Fill, AutoScroll = true, Padding = new Padding(12) };
        foreach (var editor in Editors)
        {
            var button = new Button
            {
                Height = 118, Width = 450, Margin = new Padding(6), Padding = new Padding(14),
                TextAlign = ContentAlignment.MiddleLeft, FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(245, 248, 252), UseVisualStyleBackColor = false
            };
            button.Click += (_, _) => EditGuided(editor.Kind);
            _editorCards.Add(editor.Kind, button);
            cards.Controls.Add(button);
        }
        cards.ClientSizeChanged += (_, _) =>
        {
            int available = Math.Max(250, cards.ClientSize.Width - cards.Padding.Horizontal - SystemInformation.VerticalScrollBarWidth);
            int columns = available >= 640 ? 2 : 1;
            foreach (var button in _editorCards.Values) button.Width = Math.Max(240, available / columns - 12);
        };
        _guided.Controls.Add(cards);
        _guided.Controls.Add(introduction);
    }

    private void ChangeMode()
    {
        if (_changingMode) return;
        if (!CommitEdit())
        {
            _changingMode = true;
            _expert.Checked = !_expert.Checked;
            _changingMode = false;
            return;
        }
        RefreshFields();
        UpdateState();
    }

    private void EditGuided(GuidedEditorKind kind)
    {
        if (_document is null || !CommitEdit()) return;
        try
        {
            var edited = GuidedEditors.Edit(this, _document, kind);
            if (edited is not null) Accept(edited);
        }
        catch (Exception ex) when (IsUserError(ex)) { ShowError(ex); }
    }

    private string GuidedSummary(GuidedEditorKind kind)
    {
        if (_document is null) return "Open a save file to begin.";
        string Value(string name) => _document.GetValue(name).Replace('\r', ' ').Replace('\n', ' ');
        return kind switch
        {
            GuidedEditorKind.Currency => $"Monocoins: {Value("Monocoins")}  •  Casino: {Value("CasinoCoins")}",
            GuidedEditorKind.Playtime => $"Current playtime: {Value("PlayTime")} frames",
            GuidedEditorKind.Level => $"Level: {Value("CurrentLevel")}  •  EXP: {Value("TotalEXP")}",
            GuidedEditorKind.Flags => $"Game-clear flag: {Value("GameClearFlg")}",
            GuidedEditorKind.Map => $"Map: {Value("CurrentMapID")}  •  X: {Value("MapPositionX")}  •  Y: {Value("MapPositionY")}",
            GuidedEditorKind.Characters => "16 named characters; other status bits are preserved.",
            GuidedEditorKind.Files => $"Script: {Value("WRDFile")}  •  Line: {Value("CurrentLine")}",
            GuidedEditorKind.Audio => $"BGM index: {Value("BGMIndex")}",
            _ => ""
        };
    }

    private static DataGridView NewGrid() => new()
    {
        Dock = DockStyle.Fill, AllowUserToAddRows = false, AllowUserToDeleteRows = false,
        AllowUserToOrderColumns = false, RowHeadersVisible = false, MultiSelect = true,
        SelectionMode = DataGridViewSelectionMode.CellSelect, BackgroundColor = SystemColors.Window,
        BorderStyle = BorderStyle.None, AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None,
        ClipboardCopyMode = DataGridViewClipboardCopyMode.Disable, EditMode = DataGridViewEditMode.EditOnKeystrokeOrF2
    };

    private ContextMenuStrip GridMenu()
    {
        var menu = new ContextMenuStrip();
        menu.Items.Add("Copy (Ctrl+C)", null, (_, _) => CopySelection());
        menu.Items.Add("Paste (Ctrl+V)", null, (_, _) => PasteSelection());
        return menu;
    }

    private static bool IsCommon(SaveField field) => field.IsKnown;

    private void RefreshFields(string? selected = null)
    {
        if (_document is null) return;
        selected ??= _fields.CurrentCell?.RowIndex >= 0 && _fields.CurrentCell.RowIndex < _visible.Count
            ? _visible[_fields.CurrentCell.RowIndex].Name : null;
        _refreshing = true;
        try
        {
            _visible = SaveSchema.Fields.Where(f => f.Name.Contains(_search.Text, StringComparison.OrdinalIgnoreCase)
                || f.Category.Contains(_search.Text, StringComparison.OrdinalIgnoreCase)).ToList();
            _fields.Rows.Clear();
            foreach (var field in _visible)
            {
                if (!_formats.ContainsKey(field.Name)) _formats[field.Name] = field.DefaultFormat;
                // Leave the format cell empty until its per-field choices are installed below.
                int row = _fields.Rows.Add(field.Name, $"0x{field.Offset:X6}", field.Length, null!,
                    _document.GetValue(field.Name, _formats[field.Name]));
                var formatCell = (DataGridViewComboBoxCell)_fields.Rows[row].Cells["Format"];
                formatCell.DataSource = field.SupportedFormats.ToArray();
                formatCell.Value = _formats[field.Name];
                _fields.Rows[row].Cells["Field"].ToolTipText = field.Category;
                _fields.Rows[row].Cells["Value"].ToolTipText = "Ctrl+C copies Field / Format / Value rows. Ctrl+V also accepts a single Value column. For large arrays use the byte grid below.";
                if (!IsCommon(field)) _fields.Rows[row].DefaultCellStyle.BackColor = Color.FromArgb(255, 249, 232);
            }
            int selectedRow = selected is null ? 0 : _visible.FindIndex(f => f.Name == selected);
            if (_visible.Count > 0) _fields.CurrentCell = _fields.Rows[Math.Max(0, selectedRow)].Cells["Value"];
            _split.Panel2Collapsed = false;
        }
        finally { _refreshing = false; }
        LoadBytes(); UpdateState();
    }

    private void ValidateField(object? sender, DataGridViewCellValidatingEventArgs e)
    {
        if (_refreshing || _document is null || e.RowIndex < 0 || e.ColumnIndex != 4) return;
        try
        {
            var field = _visible[e.RowIndex];
            string value = e.FormattedValue?.ToString() ?? "";
            if (value == _document.GetValue(field.Name, _formats[field.Name])) return;
            var staged = _document.Clone();
            staged.SetValue(field.Name, value, _formats[field.Name]);
            Accept(staged, refresh: false);
            LoadBytes();
        }
        catch (Exception ex) when (IsUserError(ex))
        {
            e.Cancel = true; _fields.Rows[e.RowIndex].ErrorText = ex.Message;
            _status.Text = ex.Message + " Press Esc to cancel this edit.";
        }
    }

    private void ChangeFormat(object? sender, DataGridViewCellEventArgs e)
    {
        if (_refreshing || _document is null || e.RowIndex < 0 || e.ColumnIndex != 3) return;
        if (_fields.Rows[e.RowIndex].Cells[3].Value is not ValueFormat format) return;
        var field = _visible[e.RowIndex];
        _formats[field.Name] = format;
        _fields.Rows[e.RowIndex].Cells[4].Value = _document.GetValue(field.Name, format);
    }

    private void LoadBytes()
    {
        if (_refreshing) return;
        _selectedField = _document is null || _fields.CurrentCell is null ? null : _visible[_fields.CurrentCell.RowIndex];
        _selectedBytes = _selectedField is null ? Array.Empty<byte>() : _document!.GetBytes(_selectedField.Name);
        _bytes.RowCount = (_selectedBytes.Length + 15) / 16;
        _bytes.Invalidate();
        _byteLabel.Text = _selectedField is null ? "Select a field to edit its bytes." :
            $"{_selectedField.Name} • {_selectedField.Length:N0} bytes • hexadecimal • Ctrl+V pastes a rectangle; offsets are absolute";
    }

    private void ByteValueNeeded(object? sender, DataGridViewCellValueEventArgs e)
    {
        if (_selectedField is null) return;
        int index = e.RowIndex * 16 + e.ColumnIndex - 1;
        e.Value = e.ColumnIndex == 0 ? $"0x{_selectedField.Offset + e.RowIndex * 16:X6}" :
            index < _selectedBytes.Length ? _selectedBytes[index].ToString("X2") : "";
    }

    private void ValidateByte(object? sender, DataGridViewCellValidatingEventArgs e)
    {
        if (_refreshing || _document is null || _selectedField is null || e.ColumnIndex == 0) return;
        int index = e.RowIndex * 16 + e.ColumnIndex - 1;
        if (index >= _selectedBytes.Length) return;
        string value = e.FormattedValue?.ToString() ?? "";
        if (value.Length != 2 || !byte.TryParse(value, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out byte parsed))
        {
            e.Cancel = true; _bytes.Rows[e.RowIndex].ErrorText = "Enter exactly two hexadecimal digits (00–FF).";
            _status.Text = "Invalid byte. Enter 00–FF or press Esc to cancel."; return;
        }
        var staged = _document.Clone(); staged.SetByte(_selectedField.Name, index, parsed);
        Accept(staged, refresh: false); _selectedBytes[index] = parsed;
        int fieldRow = _visible.FindIndex(f => f.Name == _selectedField.Name);
        if (fieldRow >= 0) _fields.Rows[fieldRow].Cells["Value"].Value = _document.GetValue(_selectedField.Name, _formats[_selectedField.Name]);
    }

    private void Accept(SaveDocument staged, bool refresh = true)
    {
        if (_document is null || _document.ToArray().SequenceEqual(staged.ToArray())) return;
        if (_undoHistory.Count >= 100)
        {
            var recent = _undoHistory.Take(99).Reverse().ToArray(); _undoHistory.Clear();
            foreach (var bytes in recent) _undoHistory.Push(bytes);
        }
        _undoHistory.Push(_document.ToArray()); _redoHistory.Clear(); _document = staged;
        if (refresh) RefreshFields(); else UpdateState();
    }

    private void CopySelection()
    {
        if (_document is null || !CommitEdit()) return;
        try
        {
            if (_byteGridActive)
            {
                var cells = _bytes.SelectedCells.Cast<DataGridViewCell>().Where(c => c.ColumnIndex > 0).ToArray();
                if (cells.Length == 0) return;
                int minRow = cells.Min(c => c.RowIndex), maxRow = cells.Max(c => c.RowIndex);
                int minCol = cells.Min(c => c.ColumnIndex), maxCol = cells.Max(c => c.ColumnIndex);
                if (cells.Length != (maxRow - minRow + 1) * (maxCol - minCol + 1))
                    throw new FormatException("Select a rectangular range of bytes to copy.");
                var rows = new List<string[]>();
                for (int r = minRow; r <= maxRow; r++)
                {
                    var values = new List<string>();
                    for (int c = minCol; c <= maxCol; c++)
                    {
                        int index = r * 16 + c - 1;
                        if (index >= _selectedBytes.Length) throw new FormatException("The selection includes cells beyond this field.");
                        values.Add(_selectedBytes[index].ToString("X2"));
                    }
                    rows.Add(values.ToArray());
                }
                Clipboard.SetText(ClipboardEdits.WriteRows(rows));
            }
            else
            {
                var rows = _fields.SelectedCells.Cast<DataGridViewCell>().Select(c => c.RowIndex).Distinct().OrderBy(i => i);
                var values = new List<string[]> { new[] { "Field", "Format", "Value" } };
                foreach (int row in rows)
                {
                    var field = _visible[row];
                    values.Add(new[] { field.Name, _formats[field.Name].ToString(), _document.GetValue(field.Name, _formats[field.Name]) });
                }
                Clipboard.SetText(ClipboardEdits.WriteRows(values));
            }
            _status.Text = "Copied to clipboard.";
        }
        catch (Exception ex) when (IsUserError(ex)) { ShowError(ex); }
    }

    private void PasteSelection()
    {
        if (_document is null || !CommitEdit()) return;
        try
        {
            if (!Clipboard.ContainsText()) throw new FormatException("The clipboard does not contain text.");
            var staged = _document.Clone();
            if (_byteGridActive)
            {
                if (_selectedField is null || _bytes.CurrentCell is null || _bytes.CurrentCell.ColumnIndex == 0)
                    throw new FormatException("Select the first destination byte.");
                int start = _bytes.CurrentCell.RowIndex * 16 + _bytes.CurrentCell.ColumnIndex - 1;
                staged.SetBytes(_selectedField.Name, ClipboardEdits.PasteBytes(_selectedBytes, start, Clipboard.GetText()));
                Accept(staged);
            }
            else
            {
                var changes = ClipboardEdits.ParseFields(Clipboard.GetText(), _visible, _fields.CurrentCell?.RowIndex ?? 0, _formats);
                // Displaying text, booleans, and NaNs can be lossy. Re-pasting an unchanged
                // displayed value must preserve its exact backing bytes and padding.
                staged.ApplyChanges(changes.Where(change => change.Value != _document.GetValue(change.Name, change.Format)));
                foreach (var change in changes) if (change.Format is ValueFormat format) _formats[change.Name] = format;
                Accept(staged); RefreshFields();
            }
            _status.Text = "Paste applied. Undo reverses the whole paste.";
        }
        catch (Exception ex) when (IsUserError(ex)) { ShowError(ex); }
    }

    private bool CommitEdit()
    {
        try { return _fields.EndEdit() && _bytes.EndEdit(); }
        catch (Exception ex) when (IsUserError(ex)) { ShowError(ex); return false; }
    }

    private void GridKeys(object? sender, KeyEventArgs e)
    {
        if (!e.Control) return;
        if (e.KeyCode == Keys.C) CopySelection();
        else if (e.KeyCode == Keys.V) PasteSelection();
        else return;
        e.Handled = true; e.SuppressKeyPress = true;
    }

    protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
    {
        switch (keyData)
        {
            case Keys.Control | Keys.O: Open(); return true;
            case Keys.Control | Keys.S: Save(false); return true;
            case Keys.Control | Keys.Shift | Keys.S: Save(true); return true;
            case Keys.Control | Keys.Z: Undo(); return true;
            case Keys.Control | Keys.Y: Redo(); return true;
            default: return base.ProcessCmdKey(ref msg, keyData);
        }
    }

    private void Undo()
    {
        if (_document is null || !CommitEdit() || _undoHistory.Count == 0) return;
        _redoHistory.Push(_document.ToArray()); _document = SaveDocument.FromBytes(_undoHistory.Pop()); RefreshFields();
    }

    private void Redo()
    {
        if (_document is null || !CommitEdit() || _redoHistory.Count == 0) return;
        _undoHistory.Push(_document.ToArray()); _document = SaveDocument.FromBytes(_redoHistory.Pop()); RefreshFields();
    }

    private void Open()
    {
        using var dialog = new OpenFileDialog { Filter = "V3 save files (*.dat)|*.dat|All files (*.*)|*.*", CheckFileExists = true };
        if (dialog.ShowDialog(this) != DialogResult.OK) return;
        OpenPath(dialog.FileName);
    }

    // Picker and drag/drop share one replacement path, including pending-cell validation,
    // the unsaved-changes decision, and retaining the current document when loading fails.
    private bool OpenPath(string path)
    {
        if (!ConfirmUnsaved()) return false;
        try
        {
            var loaded = SaveDocument.Load(path);
            _document = loaded; _path = path; _baseline = loaded.ToArray();
            _formats.Clear(); _undoHistory.Clear(); _redoHistory.Clear(); RefreshFields();
            return true;
        }
        catch (Exception ex) when (IsUserError(ex)) { ShowError(ex); return false; }
    }

    private void RegisterDropTargets(Control control)
    {
        if (control.IsDisposed || !_dropTargets.Add(control)) return;
        control.AllowDrop = true;
        control.DragEnter += PreviewFileDrop;
        control.DragOver += PreviewFileDrop;
        control.DragDrop += DropSaveFile;
        // DataGridView adds/reuses editing controls dynamically. Register their descendants
        // too, so dropping over a text box, grid, toolbar, or dashboard uses the same action.
        control.ControlAdded += (_, e) => { if (e.Control is { } added) RegisterDropTargets(added); };
        control.Disposed += (_, _) => _dropTargets.Remove(control);
        foreach (Control child in control.Controls) RegisterDropTargets(child);
    }

    private static bool TryGetDropPath(DragEventArgs e, out string path)
    {
        path = string.Empty;
        if ((e.AllowedEffect & DragDropEffects.Copy) == 0) return false;
        try
        {
            return e.Data?.GetDataPresent(DataFormats.FileDrop, autoConvert: false) == true
                && e.Data.GetData(DataFormats.FileDrop, autoConvert: false) is string[] files
                && SaveFileDrop.TryGetPath(files, out path);
        }
        catch (Exception ex) when (IsUserError(ex) || ex is NotSupportedException or System.Security.SecurityException) { return false; }
    }

    private static void PreviewFileDrop(object? sender, DragEventArgs e) =>
        e.Effect = TryGetDropPath(e, out _) ? DragDropEffects.Copy : DragDropEffects.None;

    private void DropSaveFile(object? sender, DragEventArgs e)
    {
        e.Effect = DragDropEffects.None;
        if (TryGetDropPath(e, out string path) && OpenPath(path)) e.Effect = DragDropEffects.Copy;
    }

    private bool Save(bool saveAs)
    {
        if (_document is null || !CommitEdit()) return false;
        string? destination = _path;
        bool overwrite = false;
        if (saveAs || destination is null)
        {
            using var dialog = new SaveFileDialog { Filter = "V3 save files (*.dat)|*.dat|All files (*.*)|*.*",
                FileName = _path is null ? "edited-save.dat" : Path.GetFileNameWithoutExtension(_path) + "-edited.dat",
                AddExtension = true, DefaultExt = "dat", OverwritePrompt = true };
            if (dialog.ShowDialog(this) != DialogResult.OK) return false;
            destination = dialog.FileName; overwrite = File.Exists(destination);
        }
        else if (File.Exists(destination))
        {
            if (MessageBox.Show(this, $"Replace this file?\n\n{destination}\n\nUse Save as to retain the original.", "Replace save file",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return false;
            overwrite = true;
        }
        try
        {
            _document.Save(destination, overwrite); _path = destination; _baseline = _document.ToArray();
            UpdateState(); _status.Text = $"Saved {Path.GetFileName(destination)}."; return true;
        }
        catch (Exception ex) when (IsUserError(ex)) { ShowError(ex); return false; }
    }

    private bool IsDirty => _document is not null && (_baseline is null || !_document.ToArray().SequenceEqual(_baseline));

    private bool ConfirmUnsaved()
    {
        if (!CommitEdit()) return false;
        if (!IsDirty) return true;
        var result = MessageBox.Show(this, "Save your changes before continuing?", "Unsaved changes",
            MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
        return result == DialogResult.No || (result == DialogResult.Yes && Save(false));
    }

    private void UpdateState()
    {
        _save.Enabled = _saveAs.Enabled = _document is not null;
        _undo.Enabled = _undoHistory.Count > 0; _redo.Enabled = _redoHistory.Count > 0;
        _fields.Enabled = _bytes.Enabled = _document is not null;
        _split.Visible = _expert.Checked;
        _guided.Visible = !_expert.Checked;
        _search.Visible = _filterLabel.Visible = _expert.Checked;
        if (_expert.Checked && !_expertOpened && _split.Height >= 400)
        {
            _split.SplitterDistance = _split.Height / 2;
            _expertOpened = true;
        }
        foreach (var editor in Editors)
        {
            var card = _editorCards[editor.Kind];
            card.Enabled = _document is not null;
            card.Text = $"{editor.Title}\n{editor.Description}\n\n{GuidedSummary(editor.Kind)}";
        }
        Text = $"{(IsDirty ? "* " : "")}{(_path is null ? "V3 Save Manager" : Path.GetFileName(_path) + " — V3 Save Manager")}";
        _status.Text = _document is null ? "Open or drag one .dat save into this window to begin. Ctrl+O opens a file." :
            (_expert.Checked
                ? $"{_visible.Count} / {SaveSchema.Fields.Count} fields • {_document.Length:N0} bytes • Expert: select a field for byte editing."
                : $"{_document.Length:N0} bytes • Guided editors • Changes also appear in Expert mode; use Undo to reverse an edit.");
    }

    private static bool IsUserError(Exception ex) => ex is IOException or InvalidDataException or UnauthorizedAccessException or ArgumentException
        or FormatException or OverflowException or InvalidOperationException or System.Runtime.InteropServices.ExternalException;

    private void ShowError(Exception ex)
    {
        _status.Text = ex.Message;
        MessageBox.Show(this, ex.Message, "Unable to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }

    private void ShowHelp() => MessageBox.Show(this,
        "Open a legacy Steam PC save, or drag one .dat file into this window, choose an editor, then Save as to write a separate file. " +
        "Dropping works over the dashboard, grids, and their controls; a drop asks about unsaved changes just like Open.\n\n" +
        "Normal mode has dedicated editors for coins, playtime, level / EXP, flags, map / position, characters, files, and audio. " +
        "Apply accepts an editor's changes as one undoable action. Cancel leaves the save untouched. " +
        "The level editor synchronizes both level fields; choosing a map synchronizes both map IDs.\n\n" +
        "Expert mode is a separate workspace displaying EVERY declared member, including unknown and reserved arrays. " +
        "Select a field to edit its bytes in the lower grid. Unknown data is shaded. Field sizes and offsets remain fixed.\n\n" +
        "Values: double-click or F2 to edit; use Format to select an interpretation. Numbers are little-endian, using a decimal point. " +
        "Hex values must contain the exact field length. Text must fit its byte capacity. Esc cancels an invalid cell.\n\n" +
        "Clipboard: Ctrl+C copies selected field rows as Field / Format / Value, including a header. " +
        "Ctrl+V accepts this format, or one Value column starting at the current field. Quoted TSV text is supported. " +
        "In the byte grid, select a rectangle to copy or choose the first destination byte and paste two-digit hex cells. " +
        "An invalid paste changes nothing. Ctrl+Z / Ctrl+Y undo or redo a whole edit or paste.\n\n" +
        "Ctrl+O: Open • Ctrl+S: Save • Ctrl+Shift+S: Save as.\n\n" +
        "The format remains a work in progress. Preserve backups of real game saves.",
        "Editing guide", MessageBoxButtons.OK, MessageBoxIcon.Information);
}
