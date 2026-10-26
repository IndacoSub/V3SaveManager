using System.Buffers.Binary;
using System.Globalization;

namespace V3SaveManager.Core;

public enum CharacterState { Dead, Alive, Unknown }

public sealed record PlaytimeParts(long Hours, int Minutes, int Seconds, int Frames);

/// <summary>Validated normal-mode edits. Every operation returns a new document and leaves its input unchanged.</summary>
public static class GuidedEdits
{
    private static readonly CultureInfo Culture = CultureInfo.InvariantCulture;

    /// <summary>Apply only displayed values which changed, retaining original padding and noncanonical flags.</summary>
    public static SaveDocument Apply(SaveDocument source, IEnumerable<FieldChange> changes)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(changes);
        var result = source.Clone();
        foreach (var change in changes)
        {
            ArgumentNullException.ThrowIfNull(change);
            if (!string.Equals(result.GetValue(change.Name, change.Format), change.Value, StringComparison.Ordinal))
                result.SetValue(change.Name, change.Value, change.Format);
        }
        return result;
    }

    public static SaveDocument SetLevel(SaveDocument source, int level, int experience)
    {
        if (level < 1 || level > 99)
            throw new ArgumentOutOfRangeException(nameof(level), "Level must be between 1 and 99.");
        if (experience < 0)
            throw new ArgumentOutOfRangeException(nameof(experience), "Experience cannot be negative.");
        return Apply(source, new[]
        {
            new FieldChange("CurrentLevel", level.ToString(Culture)),
            new FieldChange("CurrentLevelShort", level.ToString(Culture)),
            new FieldChange("TotalEXP", experience.ToString(Culture))
        });
    }

    /// <summary>A null map ID retains both original IDs. Selected normal-mode maps must be known and unrestricted.</summary>
    public static SaveDocument SetMap(SaveDocument source, short? mapId, float x, float y)
    {
        if (!float.IsFinite(x)) throw new ArgumentOutOfRangeException(nameof(x), "X must be finite.");
        if (!float.IsFinite(y)) throw new ArgumentOutOfRangeException(nameof(y), "Y must be finite.");
        var changes = new List<FieldChange>
        {
            new("MapPositionX", x.ToString("R", Culture)),
            new("MapPositionY", y.ToString("R", Culture))
        };
        if (mapId.HasValue)
        {
            if (!GameCatalog.Maps.Any(map => map.Id == mapId.Value && !map.IsRestricted))
                throw new ArgumentOutOfRangeException(nameof(mapId), "Select a known unrestricted map, or use expert mode.");
            changes.Add(new FieldChange("CurrentMapID", mapId.Value.ToString(Culture)));
            changes.Add(new FieldChange("CurrentMapID_Again", mapId.Value.ToString(Culture)));
        }
        return Apply(source, changes);
    }

    public static IReadOnlyList<CharacterState> GetCharacterStatuses(SaveDocument source)
    {
        ArgumentNullException.ThrowIfNull(source);
        uint alive = BinaryPrimitives.ReadUInt32LittleEndian(source.GetBytes("IsStatusAlive"));
        uint unknown = BinaryPrimitives.ReadUInt32LittleEndian(source.GetBytes("IsStatusUnknown"));
        var states = new CharacterState[GameCatalog.Characters.Count];
        for (int index = 0; index < states.Length; index++)
        {
            uint mask = 1u << index;
            states[index] = (alive & mask) == 0 ? CharacterState.Dead
                : (unknown & mask) != 0 ? CharacterState.Unknown : CharacterState.Alive;
        }
        return Array.AsReadOnly(states);
    }

    /// <summary>Only changed characters alter their two bits; reserved and unchanged noncanonical bits survive.</summary>
    public static SaveDocument SetCharacterStatuses(SaveDocument source, IReadOnlyList<CharacterState> states)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(states);
        if (states.Count != GameCatalog.Characters.Count)
            throw new ArgumentException($"Expected exactly {GameCatalog.Characters.Count} character states.", nameof(states));
        var previous = GetCharacterStatuses(source);
        uint alive = BinaryPrimitives.ReadUInt32LittleEndian(source.GetBytes("IsStatusAlive"));
        uint unknown = BinaryPrimitives.ReadUInt32LittleEndian(source.GetBytes("IsStatusUnknown"));
        for (int index = 0; index < states.Count; index++)
        {
            if (!Enum.IsDefined(typeof(CharacterState), states[index]))
                throw new ArgumentException($"Invalid character state at index {index}.", nameof(states));
            if (states[index] == previous[index]) continue;
            uint mask = 1u << index;
            alive = states[index] == CharacterState.Dead ? alive & ~mask : alive | mask;
            unknown = states[index] == CharacterState.Unknown ? unknown | mask : unknown & ~mask;
        }
        return Apply(source, new[]
        {
            new FieldChange("IsStatusAlive", alive.ToString(Culture), ValueFormat.UInt32),
            new FieldChange("IsStatusUnknown", unknown.ToString(Culture), ValueFormat.UInt32)
        });
    }

    public static PlaytimeParts FramesToParts(long frames)
    {
        if (frames < 0) throw new ArgumentOutOfRangeException(nameof(frames), "Playtime cannot be negative.");
        return new PlaytimeParts(frames / 216000, (int)(frames / 3600 % 60), (int)(frames / 60 % 60), (int)(frames % 60));
    }

    public static long PartsToFrames(long hours, int minutes, int seconds, int frames)
    {
        if (hours < 0) throw new ArgumentOutOfRangeException(nameof(hours), "Hours cannot be negative.");
        if (minutes < 0 || minutes > 59) throw new ArgumentOutOfRangeException(nameof(minutes), "Minutes must be between 0 and 59.");
        if (seconds < 0 || seconds > 59) throw new ArgumentOutOfRangeException(nameof(seconds), "Seconds must be between 0 and 59.");
        if (frames < 0 || frames > 59) throw new ArgumentOutOfRangeException(nameof(frames), "Frames must be between 0 and 59.");
        return checked(hours * 216000 + minutes * 3600L + seconds * 60L + frames);
    }

    public static SaveDocument SetPlaytime(SaveDocument source, long frames)
    {
        if (frames < 0) throw new ArgumentOutOfRangeException(nameof(frames), "Playtime cannot be negative.");
        return Apply(source, new[] { new FieldChange("PlayTime", frames.ToString(Culture)) });
    }
}
