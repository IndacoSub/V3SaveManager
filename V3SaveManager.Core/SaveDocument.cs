namespace V3SaveManager.Core;

/// <summary>A lossless byte-backed save. Editing one field never reserializes unrelated fields.</summary>
public sealed class SaveDocument
{
    private readonly byte[] data;

    private SaveDocument(byte[] bytes) => data = bytes;

    public int Length => data.Length;
    public int TrailingByteCount => data.Length - SaveSchema.Size;
    public IReadOnlyList<SaveField> Fields => SaveSchema.Fields;

    public static SaveDocument Load(string path) => FromBytes(File.ReadAllBytes(path));

    public static SaveDocument FromBytes(byte[] bytes)
    {
        ArgumentNullException.ThrowIfNull(bytes);
        if (bytes.Length < SaveSchema.Size)
            throw new InvalidDataException($"Save is truncated: expected at least {SaveSchema.Size} bytes, found {bytes.Length}.");
        return new SaveDocument((byte[])bytes.Clone());
    }

    public byte[] ToArray() => (byte[])data.Clone();
    public SaveDocument Clone() => new((byte[])data.Clone());

    public byte[] GetBytes(string name)
    {
        var field = SaveSchema.GetField(name);
        return data.AsSpan(field.Offset, field.Length).ToArray();
    }

    public string GetValue(string name, ValueFormat? format = null)
    {
        var field = SaveSchema.GetField(name);
        return ValueCodec.Format(data.AsSpan(field.Offset, field.Length), format ?? field.DefaultFormat);
    }

    public void SetValue(string name, string value, ValueFormat? format = null)
    {
        var field = SaveSchema.GetField(name);
        SetBytes(name, ValueCodec.Parse(value, field.Length, format ?? field.DefaultFormat));
    }

    public void SetBytes(string name, byte[] bytes)
    {
        ArgumentNullException.ThrowIfNull(bytes);
        var field = SaveSchema.GetField(name);
        if (bytes.Length != field.Length)
            throw new ArgumentException($"{name} requires exactly {field.Length} bytes; received {bytes.Length}.", nameof(bytes));
        bytes.CopyTo(data, field.Offset);
    }

    public void SetByte(string name, int index, byte value)
    {
        var field = SaveSchema.GetField(name);
        if (index < 0 || index >= field.Length) throw new ArgumentOutOfRangeException(nameof(index));
        data[field.Offset + index] = value;
    }

    /// <summary>Validate every change before updating the document; failure leaves it unchanged.</summary>
    public void ApplyChanges(IEnumerable<FieldChange> changes)
    {
        ArgumentNullException.ThrowIfNull(changes);
        var staged = Clone();
        foreach (var change in changes)
        {
            ArgumentNullException.ThrowIfNull(change);
            staged.SetValue(change.Name, change.Value, change.Format);
        }
        staged.data.CopyTo(data, 0);
    }

    /// <summary>Write a sibling temporary file, flush it, and replace the destination only after success.</summary>
    public void Save(string path, bool overwrite = false)
    {
        string destination = Path.GetFullPath(path);
        string directory = Path.GetDirectoryName(destination)!;
        string temporary = Path.Combine(directory, $".{Path.GetFileName(destination)}.{Guid.NewGuid():N}.tmp");
        try
        {
            using (var stream = new FileStream(temporary, FileMode.CreateNew, FileAccess.Write, FileShare.None))
            {
                stream.Write(data);
                stream.Flush(flushToDisk: true);
            }
            File.Move(temporary, destination, overwrite);
        }
        finally
        {
            try { if (File.Exists(temporary)) File.Delete(temporary); }
            catch (IOException) { /* Preserve the original write error. */ }
            catch (UnauthorizedAccessException) { /* Preserve the original write error. */ }
        }
    }
}
