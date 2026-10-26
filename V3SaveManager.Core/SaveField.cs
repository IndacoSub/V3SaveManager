namespace V3SaveManager.Core;

public enum ValueFormat
{
    Hex, Int16, UInt16, Int32, UInt32, Int64, UInt64, Float32, Float64, Utf16, Boolean, Byte, Utf8
}

public sealed record SaveField(string Name, int Offset, int Length, ValueFormat DefaultFormat, string Category)
{
    public bool IsKnown => DefaultFormat != ValueFormat.Hex;

    public IReadOnlyList<ValueFormat> SupportedFormats => ValueCodec.GetSupportedFormats(Length);
}

public sealed record FieldChange(string Name, string Value, ValueFormat? Format = null);
