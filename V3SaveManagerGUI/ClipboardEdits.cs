using System.Globalization;
using System.Text;
using V3SaveManager.Core;

namespace V3SaveManagerGUI;

/// <summary>Platform-independent, strict TSV parsing shared by both GUI grids.</summary>
public static class ClipboardEdits
{
    public static string WriteRows(IEnumerable<IEnumerable<string>> rows) => string.Join("\r\n",
        rows.Select(row => string.Join("\t", row.Select(value => value.IndexOfAny(new[] { '\t', '\r', '\n', '"' }) >= 0
            ? "\"" + value.Replace("\"", "\"\"") + "\"" : value))));

    public static IReadOnlyList<string[]> ReadRows(string text)
    {
        if (string.IsNullOrEmpty(text)) throw new FormatException("The clipboard is empty.");
        var rows = new List<string[]>();
        var cells = new List<string>();
        var cell = new StringBuilder();
        bool quoted = false, closed = false;
        for (int i = 0; i < text.Length; i++)
        {
            char c = text[i];
            if (quoted)
            {
                if (c != '"') cell.Append(c);
                else if (i + 1 < text.Length && text[i + 1] == '"') { cell.Append('"'); i++; }
                else { quoted = false; closed = true; }
            }
            else if (c == '\t' || c == '\r' || c == '\n')
            {
                cells.Add(cell.ToString()); cell.Clear(); closed = false;
                if (c != '\t')
                {
                    rows.Add(cells.ToArray()); cells.Clear();
                    if (c == '\r' && i + 1 < text.Length && text[i + 1] == '\n') i++;
                }
            }
            else if (closed) throw new FormatException("Unexpected text after a quoted cell.");
            else if (c == '"')
            {
                if (cell.Length != 0) throw new FormatException("Quotes must surround the entire cell.");
                quoted = true;
            }
            else cell.Append(c);
        }
        if (quoted) throw new FormatException("Unterminated quoted cell.");
        if (cell.Length > 0 || cells.Count > 0 || closed || (text[^1] != '\r' && text[^1] != '\n'))
        {
            cells.Add(cell.ToString()); rows.Add(cells.ToArray());
        }
        if (rows.Count == 0) throw new FormatException("The clipboard is empty.");
        return rows;
    }

    public static IReadOnlyList<FieldChange> ParseFields(string text, IReadOnlyList<SaveField> visibleFields,
        int startRow, IReadOnlyDictionary<string, ValueFormat> formats)
    {
        var rows = ReadRows(text);
        var changes = new List<FieldChange>();
        bool named = rows[0].Length == 3;
        int first = named && rows[0][0].Equals("Field", StringComparison.OrdinalIgnoreCase)
            && rows[0][1].Equals("Format", StringComparison.OrdinalIgnoreCase) ? 1 : 0;
        for (int i = first; i < rows.Count; i++)
        {
            var row = rows[i];
            if (named)
            {
                if (row.Length != 3) throw new FormatException($"Clipboard row {i + 1} needs Field, Format and Value columns.");
                var field = SaveSchema.GetField(row[0]);
                if (!visibleFields.Any(f => f.Name == field.Name))
                    throw new FormatException($"{field.Name} is hidden. Clear the filter or enable Expert mode before pasting.");
                if (!Enum.TryParse<ValueFormat>(row[1], true, out var format) || !Enum.IsDefined(typeof(ValueFormat), format))
                    throw new FormatException($"Unknown format '{row[1]}'.");
                changes.Add(new FieldChange(field.Name, row[2], format));
            }
            else
            {
                if (row.Length != 1) throw new FormatException("Paste one Value column, or Field / Format / Value columns.");
                int target = startRow + i;
                if (target < 0 || target >= visibleFields.Count) throw new FormatException("The paste extends beyond the visible fields.");
                var field = visibleFields[target];
                changes.Add(new FieldChange(field.Name, row[0], formats[field.Name]));
            }
        }
        if (changes.Count == 0) throw new FormatException("There are no values to paste.");
        if (changes.Select(c => c.Name).Distinct(StringComparer.OrdinalIgnoreCase).Count() != changes.Count)
            throw new FormatException("A field can appear only once per paste.");
        return changes;
    }

    public static byte[] PasteBytes(byte[] original, int startIndex, string text)
    {
        if (startIndex < 0 || startIndex >= original.Length) throw new FormatException("Select a byte before pasting.");
        var rows = ReadRows(text);
        int width = rows[0].Length;
        int column = startIndex % 16;
        if (column + width > 16) throw new FormatException("The paste extends beyond the 16 byte columns.");
        var result = (byte[])original.Clone();
        for (int r = 0; r < rows.Count; r++)
        {
            if (rows[r].Length != width) throw new FormatException("The clipboard must be a rectangular grid.");
            for (int c = 0; c < width; c++)
            {
                int target = checked(startIndex + r * 16 + c);
                string value = rows[r][c].Trim();
                if (target >= original.Length) throw new FormatException("The paste extends beyond this field.");
                if (value.Length != 2 || !byte.TryParse(value, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out byte parsed))
                    throw new FormatException($"'{value}' is not a two-digit hexadecimal byte.");
                result[target] = parsed;
            }
        }
        return result;
    }
}
