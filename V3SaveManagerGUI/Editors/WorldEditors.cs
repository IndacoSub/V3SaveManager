using V3SaveManager.Core;

namespace V3SaveManagerGUI.Editors;

internal sealed class MapEditor : EditorDialog
{
    private readonly ComboBox _maps;
    private readonly TextBox _x;
    private readonly TextBox _y;
    private readonly string _originalX;
    private readonly string _originalY;

    public MapEditor(SaveDocument source) : base(source, "Map and position", 480)
    {
        AddNote("Choose a named map or keep both existing IDs untouched. Choosing a map synchronizes both stored IDs. Maps previously associated with crashes or infinite loading are excluded.");
        string id = source.GetValue("CurrentMapID"), mirror = source.GetValue("CurrentMapID_Again");
        var named = GameCatalog.Maps.FirstOrDefault(map => map.Id.ToString(Invariant) == id);
        AddNote($"Current map: {id}{(named is null ? " (not in map catalog)" : " — " + named)}. Second stored ID: {mirror}.");
        _maps = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, MaxDropDownItems = 18, IntegralHeight = true };
        _maps.Items.Add("Keep current map IDs (unchanged)");
        foreach (var map in GameCatalog.Maps.Where(map => !map.IsRestricted).OrderBy(map => map.Id)) _maps.Items.Add(map);
        _maps.SelectedIndex = 0;
        AddRow("Destination map", _maps);
        _originalX = source.GetValue("MapPositionX"); _originalY = source.GetValue("MapPositionY");
        _x = AddText("Position X", _originalX, "Use a decimal point. New coordinates must be finite.");
        _y = AddText("Position Y", _originalY, "Use a decimal point. New coordinates must be finite.");
        AddNote("Map names follow the game's internal identifiers. Changing map data can affect game progression; preserve a backup before trying it in the game.");
    }

    protected override SaveDocument BuildDocument()
    {
        if (_maps.SelectedIndex == 0 && _x.Text == _originalX && _y.Text == _originalY) return Source.Clone();
        short? mapId = _maps.SelectedItem is GameMap map ? map.Id : null;
        return GuidedEdits.SetMap(Source, mapId, Float(_x.Text), Float(_y.Text));
    }
}

internal sealed class CharacterEditor : EditorDialog
{
    private readonly List<ComboBox> _states = new();
    private readonly IReadOnlyList<CharacterState> _original;

    public CharacterEditor(SaveDocument source) : base(source, "Character statuses", 740)
    {
        AddNote("Set each named character to Dead, Alive, or Unknown. Only changed character bits are updated; reserved bits and unchanged status encodings are preserved.");
        _original = GuidedEdits.GetCharacterStatuses(source);
        for (int i = 0; i < GameCatalog.Characters.Count; i++)
        {
            var combo = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList };
            AddRow(GameCatalog.Characters[i], combo);
            // Three fixed items need no data binding or BindingContext lifecycle.
            combo.Items.AddRange(Enum.GetValues<CharacterState>().Cast<object>().ToArray());
            combo.SelectedItem = _original[i];
            _states.Add(combo);
        }
        AddAction("Reset to loaded statuses", () =>
        {
            for (int i = 0; i < _states.Count; i++) _states[i].SelectedItem = _original[i];
        });
    }

    protected override SaveDocument BuildDocument()
    {
        var statuses = _states.Select(combo => combo.SelectedItem is CharacterState state ? state :
            throw new FormatException("Choose a status for every character.")).ToArray();
        return GuidedEdits.SetCharacterStatuses(Source, statuses);
    }
}
