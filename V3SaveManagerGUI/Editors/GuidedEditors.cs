using System.Globalization;
using V3SaveManager.Core;

namespace V3SaveManagerGUI.Editors;

public enum GuidedEditorKind { Currency, Playtime, Level, Flags, Map, Characters, Files, Audio }

/// <summary>Dedicated, transactional editors. The supplied document is never mutated.</summary>
public static class GuidedEditors
{
    public static SaveDocument? Edit(IWin32Window owner, SaveDocument document, GuidedEditorKind kind)
    {
        using EditorDialog dialog = kind switch
        {
            GuidedEditorKind.Currency => new CurrencyEditor(document),
            GuidedEditorKind.Playtime => new PlaytimeEditor(document),
            GuidedEditorKind.Level => new LevelEditor(document),
            GuidedEditorKind.Flags => new FlagEditor(document),
            GuidedEditorKind.Map => new MapEditor(document),
            GuidedEditorKind.Characters => new CharacterEditor(document),
            GuidedEditorKind.Files => new FilesEditor(document),
            GuidedEditorKind.Audio => new AudioEditor(document),
            _ => throw new ArgumentOutOfRangeException(nameof(kind))
        };
        return dialog.ShowDialog(owner) == DialogResult.OK ? dialog.Result : null;
    }
}

internal abstract class EditorDialog : Form
{
    protected readonly SaveDocument Source;
    protected readonly TableLayoutPanel Content;
    protected static readonly CultureInfo Invariant = CultureInfo.InvariantCulture;
    private readonly Label _error = new() { AutoSize = true, ForeColor = Color.Firebrick, MaximumSize = new Size(660, 0) };
    private readonly ToolTip _tips = new();

    public SaveDocument? Result { get; private set; }

    protected EditorDialog(SaveDocument source, string title, int height = 470)
    {
        Source = source;
        Text = title;
        Size = new Size(760, height);
        MinimumSize = new Size(660, 350);
        StartPosition = FormStartPosition.CenterParent;
        ShowInTaskbar = false;
        MaximizeBox = false;
        MinimizeBox = false;
        Font = new Font("Segoe UI", 10);
        var scroll = new Panel { Dock = DockStyle.Fill, AutoScroll = true, Padding = new Padding(16) };
        Content = new TableLayoutPanel { AutoSize = true, AutoSizeMode = AutoSizeMode.GrowAndShrink,
            Dock = DockStyle.Top, ColumnCount = 2, Padding = new Padding(0, 0, 8, 10) };
        Content.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 190));
        Content.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        scroll.Controls.Add(Content);
        var footer = new FlowLayoutPanel { Dock = DockStyle.Bottom, Height = 62,
            FlowDirection = FlowDirection.RightToLeft, Padding = new Padding(12) };
        var cancel = new Button { Text = "Cancel", AutoSize = true, DialogResult = DialogResult.Cancel };
        var apply = new Button { Text = "Apply", AutoSize = true };
        apply.Click += (_, _) => Guard(() =>
        {
            Result = BuildDocument(); DialogResult = DialogResult.OK; Close();
        });
        footer.Controls.Add(cancel); footer.Controls.Add(apply);
        var errors = new Panel { Dock = DockStyle.Bottom, Height = 54, Padding = new Padding(16, 4, 16, 0) };
        errors.Controls.Add(_error);
        Controls.Add(scroll); Controls.Add(errors); Controls.Add(footer);
        AcceptButton = apply; CancelButton = cancel;
    }

    protected abstract SaveDocument BuildDocument();

    protected void AddRow(string label, Control control, string? hint = null)
    {
        int row = Content.RowCount++;
        Content.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        Content.Controls.Add(new Label { Text = label, AutoSize = true, Anchor = AnchorStyles.Left,
            Margin = new Padding(0, 8, 12, 8), MaximumSize = new Size(180, 0) }, 0, row);
        control.Anchor = AnchorStyles.Left | AnchorStyles.Right;
        control.Margin = new Padding(0, 5, 0, 5);
        Content.Controls.Add(control, 1, row);
        if (hint is not null) _tips.SetToolTip(control, hint);
    }

    protected TextBox AddText(string label, string initial, string? hint = null)
    {
        var control = new TextBox { Text = initial, MaxLength = int.MaxValue };
        AddRow(label, control, hint); return control;
    }

    protected void AddNote(string text)
    {
        int row = Content.RowCount++;
        Content.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        var label = new Label { Text = text, AutoSize = true, MaximumSize = new Size(660, 0),
            ForeColor = SystemColors.GrayText, Margin = new Padding(0, 8, 0, 12) };
        Content.Controls.Add(label, 0, row); Content.SetColumnSpan(label, 2);
    }

    protected Button AddAction(string label, Action action)
    {
        var button = new Button { Text = label, AutoSize = true };
        button.Click += (_, _) => Guard(action);
        AddRow("", button); return button;
    }

    protected void Guard(Action action)
    {
        try { _error.Text = ""; action(); }
        catch (Exception ex) when (ex is ArgumentException or FormatException or OverflowException or InvalidOperationException)
        { _error.Text = ex.Message; }
    }

    protected static int Int(string text) => int.Parse(text, NumberStyles.Integer, Invariant);
    protected static long Long(string text) => long.Parse(text, NumberStyles.Integer, Invariant);
    protected static float Float(string text) => float.Parse(text, NumberStyles.Float, Invariant);

    protected override void Dispose(bool disposing)
    {
        if (disposing) _tips.Dispose();
        base.Dispose(disposing);
    }
}

internal abstract class FieldsEditor : EditorDialog
{
    private readonly List<(string Name, TextBox Input, string Original, ValueFormat? Format, Action<string>? Validate)> _fields = new();

    protected FieldsEditor(SaveDocument source, string title, int height = 470) : base(source, title, height) { }

    protected TextBox AddField(string name, string label, ValueFormat? format = null, Action<string>? validate = null)
    {
        var field = SaveSchema.GetField(name);
        var initial = Source.GetValue(name, format);
        var input = AddText(label, initial, $"{name} • {field.Length} bytes • {format ?? field.DefaultFormat}");
        _fields.Add((name, input, initial, format, validate));
        return input;
    }

    protected override SaveDocument BuildDocument()
    {
        var changes = new List<FieldChange>();
        foreach (var field in _fields)
        {
            if (field.Input.Text == field.Original) continue;
            field.Validate?.Invoke(field.Input.Text);
            changes.Add(new FieldChange(field.Name, field.Input.Text, field.Format));
        }
        return GuidedEdits.Apply(Source, changes);
    }

    protected static void NonNegative(string text)
    {
        if (Long(text) < 0) throw new FormatException("Enter a non-negative number.");
    }
}

internal sealed class CurrencyEditor : FieldsEditor
{
    public CurrencyEditor(SaveDocument source) : base(source, "Currency", 350)
    {
        AddNote("Change your currency balances. All other save data stays unchanged.");
        AddField("Monocoins", "Monocoins", validate: NonNegative);
        AddField("CasinoCoins", "Casino coins", validate: NonNegative);
        AddNote("Monocoins: 0–32,767. Casino coins: 0–2,147,483,647. Only Apply commits these edits.");
    }
}

internal sealed class FilesEditor : FieldsEditor
{
    public FilesEditor(SaveDocument source) : base(source, "Script and file references", 700)
    {
        AddNote("Edit archive and script names directly. Text uses UTF-8 and must fit each fixed byte capacity. Unchanged text retains its original padding.");
        AddField("WRDArchive", "WRD archive");
        AddField("SubroutineArchive", "Subroutine archive");
        AddField("WRDFile", "WRD file");
        AddField("SubroutineWRD", "Subroutine WRD");
        AddField("TextArchive", "Text archive");
        AddField("SubroutineTextArchive", "Subroutine text archive");
        AddField("TextSTX", "Text STX file");
        AddField("SubroutineSTX", "Subroutine STX file");
        AddField("WRDMap", "WRD map");
        AddField("CurrentLine", "Current line", validate: NonNegative);
    }
}

internal sealed class AudioEditor : FieldsEditor
{
    public AudioEditor(SaveDocument source) : base(source, "Audio", 570)
    {
        AddNote("Edit track references, track index, and volume values. Track names must fit their fixed UTF-8 fields.");
        AddField("BGMIndex", "BGM index", validate: NonNegative);
        AddField("CurrentBGM", "Current BGM");
        AddField("CurrentSFX", "Current SFX");
        AddField("CurrentSFX_Again", "Current SFX (second)");
        AddField("BGMVolume", "BGM volume", validate: NonNegative);
        AddField("SFXVolume", "SFX volume", validate: NonNegative);
        AddField("VoicesVolume", "Voice volume", validate: NonNegative);
    }
}
