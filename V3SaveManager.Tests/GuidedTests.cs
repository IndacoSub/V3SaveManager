using System.Buffers.Binary;
using System.Globalization;
using System.Text;
using V3SaveManager.Core;

internal static partial class Program
{
    private static void RunGuidedTests()
    {
        var catalog = File.ReadLines(Path.Combine(AppContext.BaseDirectory, "LegacyCatalog.tsv"))
            .Where(line => !line.StartsWith('#')).Select(line => line.Split('\t')).ToArray();
        var experience = catalog.Where(row => row[0] == "EXP").Select(row => int.Parse(row[2])).ToArray();
        var maps = catalog.Where(row => row[0] == "MAP").ToArray();
        var blocked = catalog.Where(row => row[0] == "BLOCKED").Select(row => short.Parse(row[1])).ToHashSet();
        var characters = catalog.Where(row => row[0] == "CHARACTER").Select(row => row[2]).ToArray();

        Test("guided EXP lookup matches every original level including curve boundaries and level 99", () =>
        {
            Equal(100, experience.Length);
            for (int level = 1; level <= 99; level++) Equal(experience[level], GameCatalog.ExperienceForLevel(level));
            Equal(0, GameCatalog.ExperienceForLevel(1)); Equal(500, GameCatalog.ExperienceForLevel(2));
            Equal(168500, GameCatalog.ExperienceForLevel(99));
            foreach (int level in new[] { int.MinValue, -1, 0, 100, int.MaxValue }) Rejected(() => GameCatalog.ExperienceForLevel(level));
        });
        Test("guided map catalog retains every original named ID and all nine restricted maps", () =>
        {
            Equal(164, maps.Length); Equal(164, GameCatalog.Maps.Count); Equal(9, blocked.Count);
            Equal(9, GameCatalog.Maps.Count(map => map.IsRestricted));
            for (int i = 0; i < maps.Length; i++)
            {
                var actual = GameCatalog.Maps[i]; short id = short.Parse(maps[i][1]);
                Equal(id, actual.Id); Equal(maps[i][2], actual.Name); Equal(blocked.Contains(id), actual.IsRestricted);
                Equal($"ID{id:D3}_{maps[i][2]}", actual.ToString());
            }
            Equal(GameCatalog.Maps.Count, GameCatalog.Maps.Select(map => map.Id).Distinct().Count());
        });
        Test("guided character names retain exact legacy bit ordering", () =>
        {
            Equal(16, characters.Length); Equal(16, GameCatalog.Characters.Count);
            for (int i = 0; i < characters.Length; i++) Equal(characters[i], GameCatalog.Characters[i]);
        });

        Test("guided level edits synchronize both level members without changing unrelated bytes", () =>
        {
            var source = SaveDocument.FromBytes(Original);
            source.SetValue("CurrentLevel", "25"); source.SetValue("CurrentLevelShort", "17");
            var before = source.ToArray();
            foreach (int level in new[] { 1, 6, 7, 49, 50, 51, 74, 75, 76, 98, 99 })
            {
                int exp = GameCatalog.ExperienceForLevel(level);
                var edited = GuidedEdits.SetLevel(source, level, exp);
                Equal(level.ToString(CultureInfo.InvariantCulture), edited.GetValue("CurrentLevel"));
                Equal(level.ToString(CultureInfo.InvariantCulture), edited.GetValue("CurrentLevelShort"));
                Equal(exp.ToString(CultureInfo.InvariantCulture), edited.GetValue("TotalEXP"));
                var expected = (byte[])before.Clone();
                SetExpected(expected, "CurrentLevel", Int32Bytes(level));
                var shortBytes = new byte[2]; BinaryPrimitives.WriteInt16LittleEndian(shortBytes, (short)level);
                SetExpected(expected, "CurrentLevelShort", shortBytes); SetExpected(expected, "TotalEXP", Int32Bytes(exp));
                Bytes(expected, edited.ToArray()); Bytes(before, source.ToArray());
            }
        });
        Test("invalid guided levels and EXP reject atomically", () =>
        {
            var source = SaveDocument.FromBytes(Original);
            foreach (int level in new[] { -1, 0, 100, int.MaxValue }) Rejected(() => GuidedEdits.SetLevel(source, level, 0));
            Rejected(() => GuidedEdits.SetLevel(source, 1, -1)); Bytes(Original, source.ToArray());
            Equal(int.MaxValue.ToString(CultureInfo.InvariantCulture), GuidedEdits.SetLevel(source, 99, int.MaxValue).GetValue("TotalEXP"));
        });
        Test("guided map movement preserves mismatched map IDs until explicit selection", () =>
        {
            var source = SaveDocument.FromBytes(Original);
            source.SetValue("CurrentMapID", "5"); source.SetValue("CurrentMapID_Again", "42");
            var before = source.ToArray();
            var moved = GuidedEdits.SetMap(source, null, -12.5f, 25.25f);
            Equal("5", moved.GetValue("CurrentMapID")); Equal("42", moved.GetValue("CurrentMapID_Again"));
            var expected = (byte[])before.Clone(); SetExpected(expected, "MapPositionX", Int32Bytes(BitConverter.SingleToInt32Bits(-12.5f)));
            SetExpected(expected, "MapPositionY", Int32Bytes(BitConverter.SingleToInt32Bits(25.25f)));
            Bytes(expected, moved.ToArray()); Bytes(before, source.ToArray());
            var selected = GuidedEdits.SetMap(source, 1, -12.5f, 25.25f);
            SetExpected(expected, "CurrentMapID", new byte[] { 1, 0 }); SetExpected(expected, "CurrentMapID_Again", new byte[] { 1, 0 });
            Bytes(expected, selected.ToArray()); Bytes(before, source.ToArray());
        });
        Test("guided map selection accepts named unrestricted IDs and rejects blocked or unknown IDs", () =>
        {
            var source = SaveDocument.FromBytes(Original);
            foreach (var map in maps)
            {
                short id = short.Parse(map[1]);
                if (blocked.Contains(id)) Rejected(() => GuidedEdits.SetMap(source, id, 0, 0));
                else
                {
                    var edited = GuidedEdits.SetMap(source, id, 0, 0);
                    Equal(id.ToString(CultureInfo.InvariantCulture), edited.GetValue("CurrentMapID"));
                    Equal(id.ToString(CultureInfo.InvariantCulture), edited.GetValue("CurrentMapID_Again"));
                }
            }
            foreach (short id in new short[] { -1, 9, short.MaxValue }) Rejected(() => GuidedEdits.SetMap(source, id, 0, 0));
            foreach (float invalid in new[] { float.NaN, float.PositiveInfinity, float.NegativeInfinity })
            {
                Rejected(() => GuidedEdits.SetMap(source, 1, invalid, 0));
                Rejected(() => GuidedEdits.SetMap(source, null, 0, invalid));
            }
            Bytes(Original, source.ToArray());
        });

        for (int index = 0; index < 16; index++)
        {
            int character = index;
            Test($"guided status bit {character} ({characters[character]}): all raw states, transitions and reserved bits", () =>
            {
                uint mask = 1u << character;
                foreach (bool aliveBit in new[] { false, true })
                foreach (bool unknownBit in new[] { false, true })
                {
                    uint alive = (0xA53C5A96u & ~mask) | (aliveBit ? mask : 0);
                    uint unknown = (0xC17E96A5u & ~mask) | (unknownBit ? mask : 0);
                    var source = SaveDocument.FromBytes(Original);
                    source.SetBytes("IsStatusAlive", UInt32Bytes(alive)); source.SetBytes("IsStatusUnknown", UInt32Bytes(unknown));
                    var before = source.ToArray();
                    var states = GuidedEdits.GetCharacterStatuses(source).ToArray();
                    var initial = !aliveBit ? CharacterState.Dead : unknownBit ? CharacterState.Unknown : CharacterState.Alive;
                    Equal(initial, states[character]);
                    Bytes(before, GuidedEdits.SetCharacterStatuses(source, states).ToArray());
                    foreach (CharacterState desired in Enum.GetValues<CharacterState>())
                    {
                        var next = (CharacterState[])states.Clone(); next[character] = desired;
                        var changed = GuidedEdits.SetCharacterStatuses(source, next);
                        uint expectedAlive = alive, expectedUnknown = unknown;
                        if (desired != initial)
                        {
                            expectedAlive = desired == CharacterState.Dead ? alive & ~mask : alive | mask;
                            expectedUnknown = desired == CharacterState.Unknown ? unknown | mask : unknown & ~mask;
                        }
                        var expected = (byte[])before.Clone();
                        SetExpected(expected, "IsStatusAlive", UInt32Bytes(expectedAlive));
                        SetExpected(expected, "IsStatusUnknown", UInt32Bytes(expectedUnknown));
                        Bytes(expected, changed.ToArray()); Bytes(before, source.ToArray());
                        Equal(desired, GuidedEdits.GetCharacterStatuses(changed)[character]);
                        Equal(alive & 0xFFFF0000, BinaryPrimitives.ReadUInt32LittleEndian(changed.GetBytes("IsStatusAlive")) & 0xFFFF0000);
                        Equal(unknown & 0xFFFF0000, BinaryPrimitives.ReadUInt32LittleEndian(changed.GetBytes("IsStatusUnknown")) & 0xFFFF0000);
                    }
                }
            });
        }
        Test("invalid guided character lists reject without partial mutation", () =>
        {
            var source = SaveDocument.FromBytes(Original);
            Rejected(() => GuidedEdits.SetCharacterStatuses(source, new CharacterState[15]));
            Rejected(() => GuidedEdits.SetCharacterStatuses(source, new CharacterState[17]));
            var states = GuidedEdits.GetCharacterStatuses(source).ToArray();
            states[0] = states[0] == CharacterState.Alive ? CharacterState.Dead : CharacterState.Alive;
            states[15] = (CharacterState)999;
            Rejected(() => GuidedEdits.SetCharacterStatuses(source, states)); Bytes(Original, source.ToArray());
        });
        Test("playtime parts preserve 60fps boundaries and very large frame counts exactly", () =>
        {
            foreach (long frames in new long[] { 0, 1, 59, 60, 3599, 3600, 215999, 216000, 216001, 9876543210, long.MaxValue })
            {
                var parts = GuidedEdits.FramesToParts(frames);
                Equal(frames, GuidedEdits.PartsToFrames(parts.Hours, parts.Minutes, parts.Seconds, parts.Frames));
                Check(parts.Minutes is >= 0 and <= 59 && parts.Seconds is >= 0 and <= 59 && parts.Frames is >= 0 and <= 59, "Playtime parts were not normalized.");
            }
            Equal(new PlaytimeParts(1, 2, 3, 4), GuidedEdits.FramesToParts(223384));
            const long hours = 1000000000000;
            var watch = System.Diagnostics.Stopwatch.StartNew();
            long large = GuidedEdits.PartsToFrames(hours, 59, 58, 57);
            Equal(new PlaytimeParts(hours, 59, 58, 57), GuidedEdits.FramesToParts(large));
            Check(watch.Elapsed < TimeSpan.FromSeconds(5), "Playtime conversion should not iterate over elapsed frames.");
        });
        Test("playtime validates part ranges, negative frames and checked overflow", () =>
        {
            Rejected(() => GuidedEdits.FramesToParts(-1));
            foreach (var parts in new[]
            {
                (-1L, 0, 0, 0), (0L, -1, 0, 0), (0L, 60, 0, 0), (0L, 0, -1, 0),
                (0L, 0, 60, 0), (0L, 0, 0, -1), (0L, 0, 0, 60), (long.MaxValue, 0, 0, 0)
            }) Rejected(() => GuidedEdits.PartsToFrames(parts.Item1, parts.Item2, parts.Item3, parts.Item4));
            var maximum = GuidedEdits.FramesToParts(long.MaxValue);
            Rejected(() => GuidedEdits.PartsToFrames(maximum.Hours, maximum.Minutes, maximum.Seconds, maximum.Frames + 1));
            var source = SaveDocument.FromBytes(Original);
            Rejected(() => GuidedEdits.SetPlaytime(source, -1)); Bytes(Original, source.ToArray());
            var edited = GuidedEdits.SetPlaytime(source, long.MaxValue);
            var expected = (byte[])Original.Clone(); var encoded = new byte[8]; BinaryPrimitives.WriteInt64LittleEndian(encoded, long.MaxValue);
            SetExpected(expected, "PlayTime", encoded); Bytes(expected, edited.ToArray()); Bytes(Original, source.ToArray());
        });
        Test("guided displayed-value no-ops preserve text padding, NaN payloads, and noncanonical flags", () =>
        {
            var source = SaveDocument.FromBytes(Original);
            var padded = Enumerable.Repeat((byte)0xA5, 128).ToArray(); Encoding.UTF8.GetBytes("name\0").CopyTo(padded, 0);
            source.SetBytes("GameName", padded);
            source.SetBytes("GameClearFlg", new byte[] { 2, 0 });
            source.SetBytes("MapPositionX", UInt32Bytes(0x7FC01234));
            var changes = SaveSchema.Fields.Select(field => new FieldChange(field.Name, source.GetValue(field.Name))).ToList();
            changes.Add(new FieldChange("GameClearFlg", source.GetValue("GameClearFlg", ValueFormat.Boolean), ValueFormat.Boolean));
            var before = source.ToArray();
            Bytes(before, GuidedEdits.Apply(source, changes).ToArray()); Bytes(before, source.ToArray());
            Rejected(() => GuidedEdits.Apply(source, new[] { new FieldChange("Monocoins", "999"), new FieldChange("CurrentLevel", "bad") }));
            Bytes(before, source.ToArray());
        });
    }

    private static void SetExpected(byte[] document, string field, byte[] value)
    {
        var row = File.ReadLines(Path.Combine(AppContext.BaseDirectory, "LegacyLayout.tsv"))
            .Where(line => !line.StartsWith('#')).Select(line => line.Split('\t')).Single(parts => parts[0] == field);
        Equal(int.Parse(row[2]), value.Length);
        value.CopyTo(document, int.Parse(row[1]));
    }

    private static byte[] Int32Bytes(int value)
    {
        var result = new byte[4]; BinaryPrimitives.WriteInt32LittleEndian(result, value); return result;
    }

    private static byte[] UInt32Bytes(uint value)
    {
        var result = new byte[4]; BinaryPrimitives.WriteUInt32LittleEndian(result, value); return result;
    }
}
