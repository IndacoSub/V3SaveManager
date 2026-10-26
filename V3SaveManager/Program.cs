using System.Globalization;
using System.Text.Json;
using V3SaveManager.Core;

namespace V3SaveManager;

internal static class Program
{
    private const string Help = @"V3SaveManager - Danganronpa V3 legacy PC save editor

Usage:
  V3SaveManager fields [--filter TEXT]
  V3SaveManager inspect INPUT [--filter TEXT] [--format FORMAT]
  V3SaveManager get INPUT FIELD [--format FORMAT]
  V3SaveManager set INPUT FIELD=VALUE... --output OUTPUT [--format FORMAT] [--force]
  V3SaveManager export INPUT --output JSON [--force]
  V3SaveManager import INPUT JSON --output OUTPUT [--force]
  V3SaveManager --help

Formats (case insensitive): Hex, Int16, UInt16, Int32, UInt32, Int64, UInt64,
  Float32, Float64, Utf8, Utf16, Boolean, Byte.
Numeric formats are little endian and use invariant culture. Without --format,
known fields use their documented format; unknown fields use exact-width hex.
Hex edits must contain exactly the field's byte count. Quote values with spaces.

Examples:
  V3SaveManager get save.dat Monocoins
  V3SaveManager set save.dat Monocoins=999 CurrentLevel=50 --output edited.dat
  V3SaveManager set save.dat Monocoins=E703 --format Hex --output edited.dat
  V3SaveManager export save.dat --output fields.json
  V3SaveManager import save.dat fields.json --output edited.dat

Export writes {""version"":1,""fields"":{""FieldName"":""HEX..."",...}}.
Import accepts a partial fields object, with exact field names and hex values.
All edits are validated before writing. Unedited bytes, including trailing data,
are retained. Existing output files require --force. No command implicitly saves
back to its input. Real game save compatibility is not guaranteed; keep backups.
Exit codes: 0 success/help, 2 invalid arguments/data, 3 file/access error.";

    public static int Main(string[] args)
    {
        try
        {
            if (args.Length == 0 || (args.Length == 1 && args[0] is "--help" or "-h" or "help"))
            {
                Console.WriteLine(Help);
                return 0;
            }

            switch (args[0])
            {
                case "fields": ListFields(Parse(args, 0, "filter")); break;
                case "inspect": Inspect(Parse(args, 1, "filter", "format")); break;
                case "get": Get(Parse(args, 2, "format")); break;
                case "set": Set(Parse(args, -2, "output", "format", "force")); break;
                case "export": Export(Parse(args, 1, "output", "force")); break;
                case "import": Import(Parse(args, 2, "output", "force")); break;
                default: throw new ArgumentException($"Unknown command '{args[0]}'. Run --help for usage.");
            }
            return 0;
        }
        catch (Exception error) when (error is ArgumentException or FormatException or OverflowException
                                      or InvalidDataException or JsonException or KeyNotFoundException)
        {
            Console.Error.WriteLine($"Invalid input: {error.Message}");
            return 2;
        }
        catch (Exception error) when (error is IOException or UnauthorizedAccessException)
        {
            Console.Error.WriteLine($"File error: {error.Message}");
            return 3;
        }
    }

    private static Arguments Parse(string[] args, int positionalCount, params string[] allowedOptions)
    {
        var positionals = new List<string>();
        var options = new Dictionary<string, string>(StringComparer.Ordinal);
        bool endOfOptions = false;
        for (int index = 1; index < args.Length; index++)
        {
            string argument = args[index];
            if (!endOfOptions && argument == "--")
            {
                endOfOptions = true;
                continue;
            }
            if (!endOfOptions && argument.StartsWith("--", StringComparison.Ordinal))
            {
                string name = argument[2..];
                if (!allowedOptions.Contains(name, StringComparer.Ordinal))
                    throw new ArgumentException($"Unknown option '{argument}'.");
                if (options.ContainsKey(name))
                    throw new ArgumentException($"Option '{argument}' was specified more than once.");
                string value = "true";
                if (name != "force")
                {
                    if (++index >= args.Length || args[index].StartsWith("--", StringComparison.Ordinal))
                        throw new ArgumentException($"Option '{argument}' requires a value.");
                    value = args[index];
                }
                options.Add(name, value);
            }
            else
            {
                positionals.Add(argument);
            }
        }
        if (positionalCount >= 0 ? positionals.Count != positionalCount : positionals.Count < -positionalCount)
            throw new ArgumentException("Incorrect number of arguments. Run --help for usage.");
        return new Arguments(positionals, options);
    }

    private static IEnumerable<SaveField> SelectedFields(Arguments args)
    {
        string filter = args.Options.GetValueOrDefault("filter", "");
        return SaveSchema.Fields.Where(field => field.Name.Contains(filter, StringComparison.OrdinalIgnoreCase)
                                               || field.Category.Contains(filter, StringComparison.OrdinalIgnoreCase));
    }

    private static void ListFields(Arguments args)
    {
        Console.WriteLine("Name\tOffset\tLength\tFormat\tCategory");
        foreach (SaveField field in SelectedFields(args))
            Console.WriteLine($"{field.Name}\t0x{field.Offset:X}\t{field.Length}\t{field.DefaultFormat}\t{field.Category}");
    }

    private static void Inspect(Arguments args)
    {
        SaveDocument save = SaveDocument.Load(args.Positionals[0]);
        ValueFormat? format = GetFormat(args);
        Console.WriteLine("Name\tOffset\tLength\tFormat\tValue");
        foreach (SaveField field in SelectedFields(args))
            Console.WriteLine($"{field.Name}\t0x{field.Offset:X}\t{field.Length}\t{format ?? field.DefaultFormat}\t{Escape(save.GetValue(field.Name, format))}");
    }

    private static void Get(Arguments args)
    {
        SaveDocument save = SaveDocument.Load(args.Positionals[0]);
        Console.WriteLine(save.GetValue(args.Positionals[1], GetFormat(args)));
    }

    private static void Set(Arguments args)
    {
        string output = RequiredOutput(args);
        ValueFormat? format = GetFormat(args);
        var changes = new List<FieldChange>();
        var names = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        foreach (string assignment in args.Positionals.Skip(1))
        {
            int separator = assignment.IndexOf('=');
            if (separator <= 0)
                throw new ArgumentException($"Expected FIELD=VALUE, received '{assignment}'.");
            string name = assignment[..separator];
            if (!names.Add(name)) throw new ArgumentException($"Duplicate field assignment '{name}'.");
            changes.Add(new FieldChange(name, assignment[(separator + 1)..], format));
        }
        SaveDocument save = SaveDocument.Load(args.Positionals[0]);
        save.ApplyChanges(changes);
        save.Save(output, args.Options.ContainsKey("force"));
        Console.WriteLine($"Saved {changes.Count} field edit(s) to {Path.GetFullPath(output)}");
    }

    private static void Export(Arguments args)
    {
        string output = RequiredOutput(args);
        SaveDocument save = SaveDocument.Load(args.Positionals[0]);
        var fields = SaveSchema.Fields.ToDictionary(field => field.Name,
            field => Convert.ToHexString(save.GetBytes(field.Name)), StringComparer.Ordinal);
        var data = new { version = 1, fields };
        string json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
        WriteText(output, json, args.Options.ContainsKey("force"));
        Console.WriteLine($"Exported {fields.Count} fields to {Path.GetFullPath(output)}");
    }

    private static void Import(Arguments args)
    {
        string output = RequiredOutput(args);
        // Parse and validate the entire interchange document before changing the save.
        using JsonDocument json = JsonDocument.Parse(File.ReadAllText(args.Positionals[1]));
        JsonElement root = json.RootElement;
        if (root.ValueKind != JsonValueKind.Object) throw new ArgumentException("JSON root must be an object.");
        var rootNames = new HashSet<string>(StringComparer.Ordinal);
        foreach (JsonProperty property in root.EnumerateObject())
        {
            if (!rootNames.Add(property.Name)) throw new ArgumentException($"Duplicate JSON property '{property.Name}'.");
            if (property.Name is not ("version" or "fields"))
                throw new ArgumentException($"Unknown JSON property '{property.Name}'.");
        }
        if (!root.TryGetProperty("version", out JsonElement version) || version.ValueKind != JsonValueKind.Number
            || !version.TryGetInt32(out int number) || number != 1)
            throw new ArgumentException("JSON version must be the number 1.");
        if (!root.TryGetProperty("fields", out JsonElement fields) || fields.ValueKind != JsonValueKind.Object)
            throw new ArgumentException("JSON fields must be an object of exact field names and hex strings.");
        var changes = new List<FieldChange>();
        var names = new HashSet<string>(StringComparer.Ordinal);
        foreach (JsonProperty field in fields.EnumerateObject())
        {
            if (!names.Add(field.Name)) throw new ArgumentException($"Duplicate field '{field.Name}'.");
            if (SaveSchema.GetField(field.Name).Name != field.Name)
                throw new ArgumentException($"Field names in JSON are case sensitive: '{field.Name}'.");
            if (field.Value.ValueKind != JsonValueKind.String)
                throw new ArgumentException($"Field '{field.Name}' must contain a hex string.");
            changes.Add(new FieldChange(field.Name, field.Value.GetString()!, ValueFormat.Hex));
        }
        if (changes.Count == 0) throw new ArgumentException("JSON fields must contain at least one edit.");
        SaveDocument save = SaveDocument.Load(args.Positionals[0]);
        save.ApplyChanges(changes);
        save.Save(output, args.Options.ContainsKey("force"));
        Console.WriteLine($"Imported {changes.Count} fields to {Path.GetFullPath(output)}");
    }

    private static ValueFormat? GetFormat(Arguments args)
    {
        if (!args.Options.TryGetValue("format", out string? value)) return null;
        // Enum.TryParse also accepts undefined numeric values; those are not a CLI format.
        if (string.IsNullOrEmpty(value) || !Enum.GetNames<ValueFormat>().Contains(value, StringComparer.OrdinalIgnoreCase)
            || !Enum.TryParse(value, true, out ValueFormat format))
            throw new ArgumentException($"Unknown format '{value}'. Run --help for available formats.");
        return format;
    }

    private static string RequiredOutput(Arguments args)
    {
        if (!args.Options.TryGetValue("output", out string? path) || string.IsNullOrWhiteSpace(path))
            throw new ArgumentException("An explicit --output path is required.");
        return path;
    }

    private static string Escape(string value) => value.Replace("\\", "\\\\").Replace("\t", "\\t")
        .Replace("\r", "\\r").Replace("\n", "\\n").Replace("\0", "\\0");

    private static void WriteText(string output, string text, bool overwrite)
    {
        string path = Path.GetFullPath(output);
        string directory = Path.GetDirectoryName(path)!;
        string temporary = Path.Combine(directory, $".{Path.GetFileName(path)}.{Guid.NewGuid():N}.tmp");
        try
        {
            using (var file = new FileStream(temporary, FileMode.CreateNew, FileAccess.Write, FileShare.None))
            using (var writer = new StreamWriter(file, new System.Text.UTF8Encoding(false)))
                writer.Write(text);
            File.Move(temporary, path, overwrite);
        }
        finally
        {
            try { File.Delete(temporary); }
            catch (IOException) { /* Preserve the original write error. */ }
            catch (UnauthorizedAccessException) { /* Preserve the original write error. */ }
        }
    }

    private sealed record Arguments(List<string> Positionals, Dictionary<string, string> Options);
}
