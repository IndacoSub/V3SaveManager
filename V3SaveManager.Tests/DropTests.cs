using V3SaveManagerGUI;

internal static partial class Program
{
    private static void RunDropTests()
    {
        string folder = Path.Combine(Temp, "file-drop");
        Directory.CreateDirectory(folder);
        string lower = Path.Combine(folder, "sample.dat"), upper = Path.Combine(folder, "UPPER.DAT");
        string text = Path.Combine(folder, "sample.txt"), directory = Path.Combine(folder, "directory.dat");
        File.WriteAllBytes(lower, Original);
        File.WriteAllBytes(upper, Original);
        File.WriteAllText(text, "This is not a save file.");
        Directory.CreateDirectory(directory);

        Test("file drop accepts exactly one existing DAT file regardless of extension case", () =>
        {
            foreach (string candidate in new[] { lower, upper })
            {
                Check(SaveFileDrop.TryGetPath(new[] { candidate }, out string actual), "A single existing DAT file should be accepted.");
                Equal(Path.GetFullPath(candidate), Path.GetFullPath(actual));
            }
        });
        Test("file drop rejects missing, multiple, directory, non-DAT, empty and malformed paths", () =>
        {
            IReadOnlyList<string>?[] invalid =
            {
                null, Array.Empty<string>(), new[] { lower, upper }, new[] { lower, text },
                new[] { text }, new[] { directory }, new[] { Path.Combine(folder, "missing.dat") },
                new[] { "" }, new[] { " " }, new string[] { null! }, new[] { "invalid\0.dat" }
            };
            foreach (var files in invalid)
            {
                Check(!SaveFileDrop.TryGetPath(files, out string rejectedPath), "An invalid file drop was accepted.");
                Equal(string.Empty, rejectedPath);
            }
        });
        Test("drag hover validation reads path metadata without modifying filesystem contents", () =>
        {
            var entries = Directory.GetFileSystemEntries(folder).OrderBy(path => path).ToArray();
            var contents = Directory.GetFiles(folder).ToDictionary(path => path, File.ReadAllBytes);
            var modified = Directory.GetFiles(folder).ToDictionary(path => path, File.GetLastWriteTimeUtc);
            for (int i = 0; i < 10; i++)
            {
                SaveFileDrop.TryGetPath(new[] { lower }, out _);
                SaveFileDrop.TryGetPath(new[] { text }, out _);
                SaveFileDrop.TryGetPath(new[] { Path.Combine(folder, "missing.dat") }, out _);
                SaveFileDrop.TryGetPath(new[] { lower, upper }, out _);
            }
            Equal(string.Join("\n", entries), string.Join("\n", Directory.GetFileSystemEntries(folder).OrderBy(path => path)));
            foreach (var entry in contents)
            {
                Bytes(entry.Value, File.ReadAllBytes(entry.Key));
                Equal(modified[entry.Key], File.GetLastWriteTimeUtc(entry.Key));
            }
        });
    }
}
