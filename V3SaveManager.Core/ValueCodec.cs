using System.Buffers.Binary;
using System.Globalization;
using System.Text;

namespace V3SaveManager.Core;

/// <summary>Fixed-width, invariant-culture conversions. Integers and floating point use little endian.</summary>
public static class ValueCodec
{
    private static readonly CultureInfo Culture = CultureInfo.InvariantCulture;
    private static readonly Encoding StrictUtf8 = new UTF8Encoding(false, true);
    private static readonly Encoding StrictUtf16 = new UnicodeEncoding(false, false, true);

    public static IReadOnlyList<ValueFormat> GetSupportedFormats(int length)
    {
        var formats = new List<ValueFormat> { ValueFormat.Hex, ValueFormat.Utf8 };
        if (length % 2 == 0) formats.Add(ValueFormat.Utf16);
        if (length == 1) formats.Add(ValueFormat.Byte);
        if (length == 2) formats.AddRange(new[] { ValueFormat.Int16, ValueFormat.UInt16 });
        if (length == 4) formats.AddRange(new[] { ValueFormat.Int32, ValueFormat.UInt32, ValueFormat.Float32 });
        if (length == 8) formats.AddRange(new[] { ValueFormat.Int64, ValueFormat.UInt64, ValueFormat.Float64 });
        if (length is 1 or 2 or 4 or 8) formats.Add(ValueFormat.Boolean);
        return formats.AsReadOnly();
    }

    public static string Format(ReadOnlySpan<byte> bytes, ValueFormat format)
    {
        ValidateFormat(bytes.Length, format);
        return format switch
        {
            ValueFormat.Hex => Convert.ToHexString(bytes),
            ValueFormat.Byte => bytes[0].ToString(Culture),
            ValueFormat.Int16 => BinaryPrimitives.ReadInt16LittleEndian(bytes).ToString(Culture),
            ValueFormat.UInt16 => BinaryPrimitives.ReadUInt16LittleEndian(bytes).ToString(Culture),
            ValueFormat.Int32 => BinaryPrimitives.ReadInt32LittleEndian(bytes).ToString(Culture),
            ValueFormat.UInt32 => BinaryPrimitives.ReadUInt32LittleEndian(bytes).ToString(Culture),
            ValueFormat.Int64 => BinaryPrimitives.ReadInt64LittleEndian(bytes).ToString(Culture),
            ValueFormat.UInt64 => BinaryPrimitives.ReadUInt64LittleEndian(bytes).ToString(Culture),
            ValueFormat.Float32 => BitConverter.Int32BitsToSingle(BinaryPrimitives.ReadInt32LittleEndian(bytes)).ToString("R", Culture),
            ValueFormat.Float64 => BitConverter.Int64BitsToDouble(BinaryPrimitives.ReadInt64LittleEndian(bytes)).ToString("R", Culture),
            ValueFormat.Boolean => bytes.IndexOfAnyExceptZero() ? "true" : "false",
            ValueFormat.Utf8 => Encoding.UTF8.GetString(bytes).Split('\0')[0],
            ValueFormat.Utf16 => Encoding.Unicode.GetString(bytes).Split('\0')[0],
            _ => throw new ArgumentOutOfRangeException(nameof(format))
        };
    }

    public static byte[] Parse(string value, int length, ValueFormat format)
    {
        ArgumentNullException.ThrowIfNull(value);
        ValidateFormat(length, format);
        var bytes = new byte[length];
        switch (format)
        {
            case ValueFormat.Hex:
                var hex = string.Concat(value.Where(character => !char.IsWhiteSpace(character)));
                if (hex.Length != checked(length * 2))
                    throw new FormatException($"Expected exactly {length} bytes ({length * 2} hex digits). A field cannot change size.");
                return Convert.FromHexString(hex);
            case ValueFormat.Byte: bytes[0] = byte.Parse(value, NumberStyles.Integer, Culture); break;
            case ValueFormat.Int16: BinaryPrimitives.WriteInt16LittleEndian(bytes, short.Parse(value, NumberStyles.Integer, Culture)); break;
            case ValueFormat.UInt16: BinaryPrimitives.WriteUInt16LittleEndian(bytes, ushort.Parse(value, NumberStyles.Integer, Culture)); break;
            case ValueFormat.Int32: BinaryPrimitives.WriteInt32LittleEndian(bytes, int.Parse(value, NumberStyles.Integer, Culture)); break;
            case ValueFormat.UInt32: BinaryPrimitives.WriteUInt32LittleEndian(bytes, uint.Parse(value, NumberStyles.Integer, Culture)); break;
            case ValueFormat.Int64: BinaryPrimitives.WriteInt64LittleEndian(bytes, long.Parse(value, NumberStyles.Integer, Culture)); break;
            case ValueFormat.UInt64: BinaryPrimitives.WriteUInt64LittleEndian(bytes, ulong.Parse(value, NumberStyles.Integer, Culture)); break;
            case ValueFormat.Float32:
                var single = float.Parse(value, NumberStyles.Float, Culture);
                RejectImplicitInfinity(single, value);
                BinaryPrimitives.WriteInt32LittleEndian(bytes, BitConverter.SingleToInt32Bits(single));
                break;
            case ValueFormat.Float64:
                var number = double.Parse(value, NumberStyles.Float, Culture);
                RejectImplicitInfinity(number, value);
                BinaryPrimitives.WriteInt64LittleEndian(bytes, BitConverter.DoubleToInt64Bits(number));
                break;
            case ValueFormat.Boolean:
                var flag = value.Trim().ToLowerInvariant() switch
                {
                    "true" or "1" => true,
                    "false" or "0" => false,
                    _ => throw new FormatException("Expected true, false, 1, or 0.")
                };
                bytes[0] = flag ? (byte)1 : (byte)0;
                break;
            case ValueFormat.Utf8:
            case ValueFormat.Utf16:
                if (value.Contains('\0')) throw new FormatException("Use Hex to edit embedded NUL bytes.");
                var encoded = (format == ValueFormat.Utf8 ? StrictUtf8 : StrictUtf16).GetBytes(value);
                if (encoded.Length > length)
                    throw new FormatException($"The encoded text needs {encoded.Length} bytes; this field only holds {length}.");
                encoded.CopyTo(bytes, 0);
                break;
            default: throw new ArgumentOutOfRangeException(nameof(format));
        }
        return bytes;
    }

    private static void ValidateFormat(int length, ValueFormat format)
    {
        if (length <= 0) throw new ArgumentOutOfRangeException(nameof(length));
        if (!GetSupportedFormats(length).Contains(format))
            throw new ArgumentException($"{format} is not supported for a {length}-byte field.", nameof(format));
    }

    private static void RejectImplicitInfinity(double parsed, string value)
    {
        if (double.IsInfinity(parsed) && !new[] { "Infinity", "+Infinity", "-Infinity" }.Contains(value.Trim(), StringComparer.OrdinalIgnoreCase))
            throw new OverflowException("The number is outside the representable range.");
    }

    private static bool IndexOfAnyExceptZero(this ReadOnlySpan<byte> bytes)
    {
        foreach (byte value in bytes) if (value != 0) return true;
        return false;
    }
}
