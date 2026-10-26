using V3SaveManager.Core;

namespace V3SaveManagerGUI.Editors;

internal sealed class LevelEditor : EditorDialog
{
    private readonly TextBox _level;
    private readonly TextBox _experience;
    private readonly TextBox _desired;
    private readonly TextBox _required;
    private readonly string _originalLevel;
    private readonly string _originalExperience;

    public LevelEditor(SaveDocument source) : base(source, "Level and experience", 550)
    {
        _originalLevel = source.GetValue("CurrentLevel");
        _originalExperience = source.GetValue("TotalEXP");
        AddNote("Set the current level and total EXP, or calculate the EXP required for a desired level. Applying a change synchronizes both stored level fields.");
        AddNote($"Stored levels: {_originalLevel} (32-bit), {source.GetValue("CurrentLevelShort")} (16-bit). Leaving everything unchanged preserves both values.");
        _level = AddText("Current level (1–99)", _originalLevel);
        _experience = AddText("Total EXP", _originalExperience);
        _desired = AddText("Desired level (1–99)", _originalLevel);
        _required = AddText("Required total EXP", ""); _required.ReadOnly = true;
        AddAction("Calculate required EXP", () => _required.Text = GameCatalog.ExperienceForLevel(Int(_desired.Text)).ToString(Invariant));
        AddAction("Use desired level and EXP", () =>
        {
            int desired = Int(_desired.Text);
            int exp = GameCatalog.ExperienceForLevel(desired);
            _required.Text = exp.ToString(Invariant);
            _level.Text = desired.ToString(Invariant); _experience.Text = exp.ToString(Invariant);
        });
    }

    protected override SaveDocument BuildDocument()
    {
        if (_level.Text == _originalLevel && _experience.Text == _originalExperience) return Source.Clone();
        return GuidedEdits.SetLevel(Source, Int(_level.Text), Int(_experience.Text));
    }
}

internal sealed class FlagEditor : EditorDialog
{
    private readonly CheckBox _gameClear;
    private readonly bool _original;

    public FlagEditor(SaveDocument source) : base(source, "Game flags", 350)
    {
        AddNote("Toggle the game-clear flag. Other flags remain unchanged and are available individually in Expert mode.");
        _original = bool.Parse(source.GetValue("GameClearFlg", ValueFormat.Boolean));
        _gameClear = new CheckBox { Text = "Game cleared", Checked = _original, AutoSize = true };
        AddRow("Clear status", _gameClear);
        AddNote($"Stored GameClearFlg: {source.GetValue("GameClearFlg")}. An unchanged checkbox preserves its exact original bytes.");
    }

    protected override SaveDocument BuildDocument() => _gameClear.Checked == _original ? Source.Clone() :
        GuidedEdits.Apply(Source, new[] { new FieldChange("GameClearFlg", _gameClear.Checked ? "true" : "false", ValueFormat.Boolean) });
}

internal sealed class PlaytimeEditor : EditorDialog
{
    private readonly RadioButton _framesMode;
    private readonly RadioButton _partsMode;
    private readonly TextBox _frames;
    private readonly TextBox _hours;
    private readonly TextBox _minutes;
    private readonly TextBox _seconds;
    private readonly TextBox _remainder;
    private readonly string _originalFrames;

    public PlaytimeEditor(SaveDocument source) : base(source, "Playtime", 650)
    {
        AddNote("Playtime is stored as a 64-bit frame count at 60 frames per second. Choose total frames or hours/minutes/seconds/frames. No conversion uses repeated loops.");
        _originalFrames = source.GetValue("PlayTime");
        long original = Long(_originalFrames);
        var parts = original >= 0 ? GuidedEdits.FramesToParts(original) : new PlaytimeParts(0, 0, 0, 0);
        _framesMode = new RadioButton { Text = "Edit total frames", Checked = true, AutoSize = true };
        _partsMode = new RadioButton { Text = "Edit time components", AutoSize = true };
        AddRow("Input mode", _framesMode);
        _frames = AddText("Total frames", _originalFrames);
        AddRow("", _partsMode);
        _hours = AddText("Hours", parts.Hours.ToString(Invariant));
        _minutes = AddText("Minutes (0–59)", parts.Minutes.ToString(Invariant));
        _seconds = AddText("Seconds (0–59)", parts.Seconds.ToString(Invariant));
        _remainder = AddText("Frames (0–59)", parts.Frames.ToString(Invariant));
        void UpdateMode()
        {
            _frames.Enabled = _framesMode.Checked;
            _hours.Enabled = _minutes.Enabled = _seconds.Enabled = _remainder.Enabled = _partsMode.Checked;
        }
        _framesMode.CheckedChanged += (_, _) => UpdateMode();
        _partsMode.CheckedChanged += (_, _) => UpdateMode();
        AddAction("Convert frames to components", () =>
        {
            var parsed = GuidedEdits.FramesToParts(Long(_frames.Text));
            _hours.Text = parsed.Hours.ToString(Invariant); _minutes.Text = parsed.Minutes.ToString(Invariant);
            _seconds.Text = parsed.Seconds.ToString(Invariant); _remainder.Text = parsed.Frames.ToString(Invariant);
            _partsMode.Checked = true;
        });
        AddAction("Convert components to frames", () =>
        {
            _frames.Text = ComponentFrames().ToString(Invariant); _framesMode.Checked = true;
        });
        if (original < 0) AddNote("The stored frame count is negative. Keeping total frames unchanged preserves it; new values must be non-negative.");
        UpdateMode();
    }

    private long ComponentFrames() => GuidedEdits.PartsToFrames(Long(_hours.Text), Int(_minutes.Text), Int(_seconds.Text), Int(_remainder.Text));

    protected override SaveDocument BuildDocument()
    {
        if (_framesMode.Checked && _frames.Text == _originalFrames) return Source.Clone();
        return GuidedEdits.SetPlaytime(Source, _framesMode.Checked ? Long(_frames.Text) : ComponentFrames());
    }
}
