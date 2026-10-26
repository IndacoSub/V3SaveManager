namespace V3SaveManagerGUI;

/// <summary>Validates file-drop paths without opening or modifying the save.</summary>
public static class SaveFileDrop
{
    public static bool TryGetPath(IReadOnlyList<string>? files, out string path)
    {
        path = string.Empty;
        if (files is null || files.Count != 1 || string.IsNullOrWhiteSpace(files[0])) return false;
        try
        {
            string candidate = Path.GetFullPath(files[0]);
            if (!Path.GetExtension(candidate).Equals(".dat", StringComparison.OrdinalIgnoreCase) || !File.Exists(candidate))
                return false;
            path = candidate;
            return true;
        }
        catch (Exception ex) when (ex is ArgumentException or IOException or NotSupportedException
            or UnauthorizedAccessException or System.Security.SecurityException)
        {
            return false;
        }
    }
}
