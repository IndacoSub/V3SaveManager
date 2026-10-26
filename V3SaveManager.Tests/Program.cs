using System.Diagnostics;
using System.Globalization;
using System.Runtime.Versioning;
using System.Text;
using System.Text.Json;
using V3SaveManager.Core;
using V3SaveManagerGUI;

// A dependency-free executable suite: each case can fail independently and the process exits nonzero.
internal static partial class Program
{
    private static int passed, failed;
    private static readonly byte[] Original = Fixture();
    private static readonly string Temp = Path.Combine(Path.GetTempPath(), "v3-regression-" + Guid.NewGuid().ToString("N"));
    private static string Cli = "";

    private static int Main()
    {
        Directory.CreateDirectory(Temp);
        try
        {
            SchemaTests();
            DocumentTests();
            CodecTests();
            ClipboardTests();
            RunGuidedTests();
            RunDropTests();
            FileTests();
            CliTests();
        }
        catch (Exception ex) { failed++; Console.Error.WriteLine($"Suite setup failed: {ex}"); }
        finally { Directory.Delete(Temp, true); }
        Console.WriteLine($"{passed} passed, {failed} failed");
        return failed == 0 ? 0 : 1;
    }

    private static byte[] Fixture()
    {
        var result = new byte[279528 + 37];
        new Random(1849).NextBytes(result);
        return result;
    }

    private static void SchemaTests()
    {
        var legacy = File.ReadLines(Path.Combine(AppContext.BaseDirectory, "LegacyLayout.tsv"))
            .Where(l => !l.StartsWith('#')).Select(l => l.Split('\t')).ToArray();
        Test("schema contains every original member in serialization order", () =>
        {
            Equal(144, legacy.Length); Equal(144, SaveSchema.Fields.Count); Equal(279528, SaveSchema.Size);
            for (int i = 0; i < legacy.Length; i++)
            {
                var field = SaveSchema.Fields[i];
                Equal(legacy[i][0], field.Name);
                Equal(int.Parse(legacy[i][1]), field.Offset); Equal(int.Parse(legacy[i][2]), field.Length);
            }
        });
        foreach (var row in legacy)
        {
            string name = row[0]; int offset = int.Parse(row[1]), length = int.Parse(row[2]);
            Test($"{name}: field edit modifies exactly its original byte range", () =>
            {
                var doc = SaveDocument.FromBytes(Original);
                var bytes = doc.GetBytes(name);
                Bytes(Original.AsSpan(offset, length).ToArray(), bytes);
                for (int i = 0; i < bytes.Length; i++) bytes[i] ^= 0xFF;
                doc.SetBytes(name, bytes);
                var expected = (byte[])Original.Clone(); bytes.CopyTo(expected, offset);
                Bytes(expected, doc.ToArray());
                doc.SetValue(name, Convert.ToHexString(Original.AsSpan(offset, length)), ValueFormat.Hex);
                Bytes(Original, doc.ToArray());
            });
        }
    }

    private static void DocumentTests()
    {
        Test("arbitrary binary and trailing bytes survive roundtrip", () => Bytes(Original, SaveDocument.FromBytes(Original).ToArray()));
        Test("truncated saves are rejected", () => Rejected(() => SaveDocument.FromBytes(new byte[279527])));
        Test("document byte arrays are defensive copies", () =>
        {
            var source = (byte[])Original.Clone(); var doc = SaveDocument.FromBytes(source);
            source[0] ^= 0xFF; doc.ToArray()[0] ^= 0xFF; doc.GetBytes("GameName")[0] ^= 0xFF;
            Bytes(Original, doc.ToArray());
            var clone = doc.Clone(); clone.SetByte("Unk1", 0, 0x42);
            Bytes(Original, doc.ToArray());
        });
        Test("raw byte bounds and fixed widths are enforced", () =>
        {
            var doc = SaveDocument.FromBytes(Original);
            Rejected(() => doc.SetByte("Unk1", -1, 1));
            Rejected(() => doc.SetByte("Unk1", 4, 1));
            Rejected(() => doc.SetBytes("Unk1", new byte[3]));
            Rejected(() => doc.GetBytes("DoesNotExist"));
            Bytes(Original, doc.ToArray());
        });
        Test("batch field changes commit together and roll back on validation failure", () =>
        {
            var doc = SaveDocument.FromBytes(Original);
            Rejected(() => doc.ApplyChanges(new[] { new FieldChange("Monocoins", "999"), new FieldChange("CurrentLevel", "bad") }));
            Bytes(Original, doc.ToArray());
            doc.ApplyChanges(new[] { new FieldChange("Monocoins", "999"), new FieldChange("CurrentLevel", "99") });
            Equal("999", doc.GetValue("Monocoins")); Equal("99", doc.GetValue("CurrentLevel"));
            Bytes(Original.AsSpan(SaveSchema.Size).ToArray(), doc.ToArray().AsSpan(SaveSchema.Size).ToArray());
        });
        Test("case-insensitive field lookup retains canonical identity", () => Equal("Monocoins", SaveSchema.GetField("monocoins").Name));
    }

    private static void CodecTests()
    {
        foreach (var (name, format, values, width) in new[]
        {
            ("Unk5", ValueFormat.Int16, new[] { "-32768", "32767" }, 2),
            ("Unk5", ValueFormat.UInt16, new[] { "0", "65535" }, 2),
            ("Unk1", ValueFormat.Int32, new[] { "-2147483648", "2147483647" }, 4),
            ("Unk1", ValueFormat.UInt32, new[] { "0", "4294967295" }, 4),
            ("Unk9", ValueFormat.Int64, new[] { "-9223372036854775808", "9223372036854775807" }, 8),
            ("Unk9", ValueFormat.UInt64, new[] { "0", "18446744073709551615" }, 8)
        })
        Test($"{format} boundaries and little-endian serialization", () =>
        {
            var doc = SaveDocument.FromBytes(Original);
            foreach (var value in values)
            {
                doc.SetValue(name, value, format); Equal(value, doc.GetValue(name, format));
                var expected = new byte[width];
                if (format is ValueFormat.Int16 or ValueFormat.Int32 or ValueFormat.Int64)
                {
                    long number = long.Parse(value, CultureInfo.InvariantCulture);
                    for (int i = 0; i < width; i++) expected[i] = (byte)(number >> (8 * i));
                }
                else
                {
                    ulong number = ulong.Parse(value, CultureInfo.InvariantCulture);
                    for (int i = 0; i < width; i++) expected[i] = (byte)(number >> (8 * i));
                }
                Bytes(expected, doc.GetBytes(name));
            }
        });
        Test("floating-point values use invariant culture and little endian", () =>
        {
            var previous = CultureInfo.CurrentCulture;
            try
            {
                CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("it-IT");
                var doc = SaveDocument.FromBytes(Original);
                doc.SetValue("MapPositionX", "-12.5", ValueFormat.Float32);
                Equal("-12.5", doc.GetValue("MapPositionX", ValueFormat.Float32));
                Bytes(new byte[] { 0, 0, 0x48, 0xC1 }, doc.GetBytes("MapPositionX"));
                doc.SetValue("Unk9", "1.5", ValueFormat.Float64);
                Bytes(new byte[] { 0, 0, 0, 0, 0, 0, 0xF8, 0x3F }, doc.GetBytes("Unk9"));
            }
            finally { CultureInfo.CurrentCulture = previous; }
        });
        Test("UTF8 and UTF16 text encode Unicode and pad within the field", () =>
        {
            foreach (var format in new[] { ValueFormat.Utf8, ValueFormat.Utf16 })
            {
                var doc = SaveDocument.FromBytes(Original);
                const string text = "Caffè 東京 😀";
                doc.SetValue("GameName", text, format); Equal(text, doc.GetValue("GameName", format));
                var encoding = format == ValueFormat.Utf8 ? Encoding.UTF8 : Encoding.Unicode;
                var expected = new byte[128]; encoding.GetBytes(text).CopyTo(expected, 0);
                Bytes(expected, doc.GetBytes("GameName"));
            }
        });
        Test("byte and boolean codecs validate ranges and normalize flags", () =>
        {
            Bytes(new byte[] { 255 }, ValueCodec.Parse("255", 1, ValueFormat.Byte));
            Equal("255", ValueCodec.Format(new byte[] { 255 }, ValueFormat.Byte));
            Rejected(() => ValueCodec.Parse("256", 1, ValueFormat.Byte));
            var doc = SaveDocument.FromBytes(Original);
            doc.SetValue("Unk5", "true", ValueFormat.Boolean); Bytes(new byte[] { 1, 0 }, doc.GetBytes("Unk5"));
            doc.SetValue("Unk5", "0", ValueFormat.Boolean); Equal("false", doc.GetValue("Unk5", ValueFormat.Boolean));
            doc.SetBytes("Unk5", new byte[] { 0, 0x80 }); Equal("true", doc.GetValue("Unk5", ValueFormat.Boolean));
            Rejected(() => doc.SetValue("Unk5", "yes", ValueFormat.Boolean));
        });
        Test("text boundaries and invalid Unicode cannot silently truncate", () =>
        {
            var doc = SaveDocument.FromBytes(Original);
            doc.SetValue("GameName", new string('a', 128), ValueFormat.Utf8); Equal(new string('a', 128), doc.GetValue("GameName", ValueFormat.Utf8));
            doc.SetValue("GameName", new string('a', 64), ValueFormat.Utf16); Equal(new string('a', 64), doc.GetValue("GameName", ValueFormat.Utf16));
            var before = doc.ToArray();
            Rejected(() => doc.SetValue("GameName", "a\0b", ValueFormat.Utf8));
            Rejected(() => doc.SetValue("GameName", "\ud800", ValueFormat.Utf8));
            Rejected(() => doc.SetValue("GameName", "\ud800", ValueFormat.Utf16));
            Bytes(before, doc.ToArray());
        });
        Test("floating overflow is rejected instead of silently becoming infinity", () =>
        {
            var doc = SaveDocument.FromBytes(Original);
            Rejected(() => doc.SetValue("MapPositionX", "1e100", ValueFormat.Float32));
            Rejected(() => doc.SetValue("Unk9", "1e10000", ValueFormat.Float64));
            Bytes(Original, doc.ToArray());
        });
        Test("invalid numeric values, widths, hex and oversized text leave data untouched", () =>
        {
            var doc = SaveDocument.FromBytes(Original);
            foreach (var change in new[]
            {
                new FieldChange("Monocoins", "32768", ValueFormat.Int16),
                new FieldChange("Monocoins", "-32769", ValueFormat.Int16),
                new FieldChange("Monocoins", "-1", ValueFormat.UInt16),
                new FieldChange("Unk1", "4294967296", ValueFormat.UInt32),
                new FieldChange("Unk9", "18446744073709551616", ValueFormat.UInt64),
                new FieldChange("Unk1", "1", ValueFormat.Int64),
                new FieldChange("Unk1", "001122", ValueFormat.Hex),
                new FieldChange("Unk1", "001122GG", ValueFormat.Hex),
                new FieldChange("Unk1", "0011223344", ValueFormat.Hex),
                new FieldChange("GameName", new string('é', 129), ValueFormat.Utf8),
                new FieldChange("GameName", new string('a', 65), ValueFormat.Utf16)
            })
            {
                Rejected(() => doc.SetValue(change.Name, change.Value, change.Format)); Bytes(Original, doc.ToArray());
            }
        });
    }

    private static void ClipboardTests()
    {
        Test("spreadsheet quoted TSV roundtrips tabs, newlines, quotes and empty cells", () =>
        {
            string[][] original = { new[] { "Field", "Format", "Value" }, new[] { "ChapterName", "Utf8", "a\tb\r\nc\"d" }, new[] { "", "", "" } };
            var parsed = ClipboardEdits.ReadRows(ClipboardEdits.WriteRows(original));
            Equal(original.Length, parsed.Count);
            for (int i = 0; i < original.Length; i++) Equal(string.Join("|", original[i]), string.Join("|", parsed[i]));
        });
        Test("malformed quoted TSV is rejected", () =>
        {
            foreach (string text in new[] { "", "\"unclosed", "\"closed\"suffix", "prefix\"bad\"" }) Rejected(() => ClipboardEdits.ReadRows(text));
        });
        Test("named field paste validates, applies atomically and preserves Unicode", () =>
        {
            var formats = SaveSchema.Fields.ToDictionary(f => f.Name, f => f.DefaultFormat);
            var doc = SaveDocument.FromBytes(Original);
            string text = ClipboardEdits.WriteRows(new[] { new[] { "Field", "Format", "Value" }, new[] { "Monocoins", "Int16", "998" }, new[] { "ChapterName", "Utf8", "a\tb\nc" } });
            doc.ApplyChanges(ClipboardEdits.ParseFields(text, SaveSchema.Fields, 0, formats));
            Equal("998", doc.GetValue("Monocoins")); Equal("a\tb\nc", doc.GetValue("ChapterName"));
            var before = doc.ToArray();
            Rejected(() => doc.ApplyChanges(ClipboardEdits.ParseFields("Monocoins\tInt16\t1\nCurrentLevel\tInt32\tbad", SaveSchema.Fields, 0, formats)));
            Bytes(before, doc.ToArray());
        });
        Test("field paste rejects hidden, duplicate, unknown, and beyond-grid fields", () =>
        {
            var formats = SaveSchema.Fields.ToDictionary(f => f.Name, f => f.DefaultFormat);
            var visible = new[] { SaveSchema.GetField("Monocoins") };
            foreach (string text in new[] { "CurrentLevel\tInt32\t1", "Monocoins\tInt16\t1\nMonocoins\tInt16\t2", "Missing\tInt32\t1", "Monocoins\t999\t1", "1\n2" })
                Rejected(() => ClipboardEdits.ParseFields(text, visible, 0, formats));
        });
        Test("byte grid pastes rectangular cells with 16-byte row stride", () =>
        {
            var bytes = new byte[35];
            var edited = ClipboardEdits.PasteBytes(bytes, 1, "A0\tB1\nC2\tD3");
            var expected = new byte[35]; expected[1] = 0xA0; expected[2] = 0xB1; expected[17] = 0xC2; expected[18] = 0xD3;
            Bytes(expected, edited); Bytes(new byte[35], bytes);
        });
        Test("invalid byte pastes never change source bytes", () =>
        {
            var bytes = Enumerable.Range(0, 35).Select(i => (byte)i).ToArray(); var before = (byte[])bytes.Clone();
            foreach (var (start, text) in new[] { (1, "AA\tZZ"), (1, "AA\tBB\nCC"), (15, "AA\tBB"), (34, "AA\tBB"), (34, "AA\nBB"), (-1, "AA"), (35, "AA"), (0, "A") })
            {
                Rejected(() => ClipboardEdits.PasteBytes(bytes, start, text)); Bytes(before, bytes);
            }
        });
    }

    private static void FileTests()
    {
        Test("safe save refuses accidental overwrite and replaces only explicitly", () =>
        {
            string path = Path.Combine(Temp, "safe.dat"); var doc = SaveDocument.FromBytes(Original);
            doc.Save(path); Bytes(Original, File.ReadAllBytes(path));
            doc.SetValue("Monocoins", "777");
            Throws<IOException>(() => doc.Save(path)); Bytes(Original, File.ReadAllBytes(path));
            doc.Save(path, overwrite: true); Equal("777", SaveDocument.Load(path).GetValue("Monocoins"));
            Equal(1, Directory.GetFiles(Temp, "safe*").Length);
        });
        Test("failed save leaves existing files intact and removes temporary files", () =>
        {
            string folder = Path.Combine(Temp, "blocked"); Directory.CreateDirectory(folder);
            var doc = SaveDocument.FromBytes(Original);
            RejectedIO(() => doc.Save(folder, overwrite: true)); Equal(0, Directory.GetFiles(folder).Length);
            Equal(0, Directory.GetFiles(Temp, "*.tmp").Length);
        });
    }

    private static void CliTests()
    {
        var location = new DirectoryInfo(AppContext.BaseDirectory);
        string configuration = location.Parent!.Name;
        while (location != null && !File.Exists(Path.Combine(location.FullName, "V3SaveManager.sln"))) location = location.Parent;
        if (location == null) throw new IOException("Cannot locate solution for CLI integration tests.");
        var framework = new FrameworkName(AppContext.TargetFrameworkName ?? throw new InvalidOperationException("The test target framework is unavailable."));
        string targetFramework = $"net{framework.Version.Major}.{framework.Version.Minor}";
        Cli = Path.Combine(location.FullName, "V3SaveManager", "bin", configuration, targetFramework, "V3SaveManager.dll");
        if (!File.Exists(Cli)) throw new IOException($"CLI was not built: {Cli}");
        string input = Path.Combine(Temp, "cli-input.dat"), output = Path.Combine(Temp, "cli-output.dat"), json = Path.Combine(Temp, "cli.json");
        File.WriteAllBytes(input, Original);
        Test("CLI help and schema discovery", () =>
        {
            Exit(0, Run("--help"));
            var fields = Run("fields"); Exit(0, fields);
            foreach (var field in SaveSchema.Fields) Check(fields.Out.Contains(field.Name), $"Missing CLI field: {field.Name}");
            var filtered = Run("fields", "--filter", "Monocoins"); Exit(0, filtered); Check(filtered.Out.Contains("Monocoins"), "Filter omitted field");
        });
        Test("CLI edits multiple fields without changing source and reads values", () =>
        {
            Exit(0, Run("set", input, "Monocoins=999", "CurrentLevel=99", "--output", output));
            var doc = SaveDocument.Load(output); Equal("999", doc.GetValue("Monocoins")); Equal("99", doc.GetValue("CurrentLevel"));
            Bytes(Original, File.ReadAllBytes(input));
            var get = Run("get", output, "Monocoins"); Exit(0, get); Check(get.Out.Contains("999"), "CLI get did not print changed value");
            Exit(0, Run("inspect", output, "--filter", "Monocoins"));
        });
        Test("CLI validation and IO failures have distinct exit codes", () =>
        {
            string rejected = Path.Combine(Temp, "rejected.dat");
            Exit(2, Run("set", input, "Monocoins=1", "CurrentLevel=bad", "--output", rejected)); Check(!File.Exists(rejected), "Failed batch created output");
            Exit(2, Run("get", input, "Missing")); Exit(2, Run("set", input, "Monocoins=1"));
            Exit(2, Run("not-a-command")); Exit(3, Run("get", Path.Combine(Temp, "missing.dat"), "Monocoins"));
            File.WriteAllBytes(rejected, new byte[10]); Exit(2, Run("get", rejected, "Monocoins"));
            Exit(3, Run("set", input, "Monocoins=1", "--output", output));
            Bytes(Original, File.ReadAllBytes(input));
        });
        Test("CLI overwrite requires force, including source path", () =>
        {
            Exit(3, Run("set", input, "Monocoins=1", "--output", input)); Bytes(Original, File.ReadAllBytes(input));
            Exit(0, Run("set", input, "Monocoins=23", "--output", output, "--force")); Equal("23", SaveDocument.Load(output).GetValue("Monocoins"));
        });
        Test("CLI JSON export/import losslessly roundtrips every field and trailing bytes", () =>
        {
            Exit(0, Run("export", input, "--output", json));
            using (var parsed = JsonDocument.Parse(File.ReadAllText(json)))
            {
                Equal(1, parsed.RootElement.GetProperty("version").GetInt32());
                Equal(144, parsed.RootElement.GetProperty("fields").EnumerateObject().Count());
            }
            string restored = Path.Combine(Temp, "restored.dat");
            Exit(0, Run("import", input, json, "--output", restored)); Bytes(Original, File.ReadAllBytes(restored));
            Exit(3, Run("export", input, "--output", json));
        });
        Test("CLI partial JSON import is atomic and rejects duplicate or invalid fields", () =>
        {
            string partial = Path.Combine(Temp, "partial.json"), destination = Path.Combine(Temp, "imported.dat");
            File.WriteAllText(partial, "{\"version\":1,\"fields\":{\"Monocoins\":\"2A00\"}}");
            Exit(0, Run("import", input, partial, "--output", destination)); Equal("42", SaveDocument.Load(destination).GetValue("Monocoins"));
            var expected = (byte[])Original.Clone(); expected[0x590] = 42; expected[0x591] = 0; Bytes(expected, File.ReadAllBytes(destination));
            foreach (string invalid in new[]
            {
                "{\"version\":1,\"fields\":{\"Monocoins\":\"2A00\",\"CurrentLevel\":\"GG\"}}",
                "{\"version\":1,\"fields\":{\"Missing\":\"00\"}}",
                "{\"version\":1,\"fields\":{\"Monocoins\":\"2A00\",\"Monocoins\":\"2B00\"}}",
                "{\"version\":2,\"fields\":{}}", "not json"
            })
            {
                File.WriteAllText(partial, invalid);
                Exit(2, Run("import", input, partial, "--output", destination, "--force")); Bytes(expected, File.ReadAllBytes(destination));
            }
        });
    }

    private static (int Code, string Out, string Error) Run(params string[] args)
    {
        var info = new ProcessStartInfo(Environment.ProcessPath ?? "dotnet") { RedirectStandardOutput = true, RedirectStandardError = true, UseShellExecute = false };
        // A framework-dependent apphost can be Environment.ProcessPath; explicitly use the same runtime host.
        string? dotnetRoot = Environment.GetEnvironmentVariable("DOTNET_ROOT");
        info.FileName = string.IsNullOrEmpty(dotnetRoot) ? "dotnet" : Path.Combine(dotnetRoot, OperatingSystem.IsWindows() ? "dotnet.exe" : "dotnet");
        info.ArgumentList.Add(Cli);
        foreach (var arg in args) info.ArgumentList.Add(arg);
        using var process = Process.Start(info) ?? throw new IOException("Cannot start CLI.");
        var stdout = process.StandardOutput.ReadToEndAsync(); var stderr = process.StandardError.ReadToEndAsync();
        if (!process.WaitForExit(30000)) { process.Kill(true); throw new TimeoutException("CLI did not finish within 30 seconds."); }
        return (process.ExitCode, stdout.GetAwaiter().GetResult(), stderr.GetAwaiter().GetResult());
    }

    private static void Test(string name, Action action)
    {
        try { action(); passed++; Console.WriteLine("PASS " + name); }
        catch (Exception ex) { failed++; Console.Error.WriteLine($"FAIL {name}: {ex}"); }
    }
    private static void Check(bool condition, string message) { if (!condition) throw new Exception(message); }
    private static void Equal<T>(T expected, T actual) { if (!EqualityComparer<T>.Default.Equals(expected, actual)) throw new Exception($"Expected '{expected}', got '{actual}'."); }
    private static void Bytes(byte[] expected, byte[] actual) { Check(expected.AsSpan().SequenceEqual(actual), $"Byte arrays differ (expected {expected.Length} bytes, got {actual.Length})."); }
    private static void Exit(int expected, (int Code, string Out, string Error) result) { Check(expected == result.Code, $"Expected CLI exit {expected}, got {result.Code}: {result.Error} {result.Out}"); }
    private static void Throws<T>(Action action) where T : Exception
    {
        try { action(); } catch (T) { return; }
        throw new Exception($"Expected {typeof(T).Name}.");
    }
    private static void Rejected(Action action)
    {
        try { action(); }
        catch (Exception ex) when (ex is ArgumentException or FormatException or OverflowException or InvalidDataException) { return; }
        throw new Exception("Expected a validation exception.");
    }
    private static void RejectedIO(Action action)
    {
        try { action(); } catch (Exception ex) when (ex is IOException or UnauthorizedAccessException) { return; }
        throw new Exception("Expected an IO/access exception.");
    }
}
