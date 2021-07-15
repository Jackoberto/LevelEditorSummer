using System.IO;
using System.IO.Compression;
using UnityEngine;

public class SaveSystem : ISaveSystem
{
    private const string RelativeDirectoryPath = "Maps";
    private static string FullDirectoryPath => Path.Combine(Application.persistentDataPath, RelativeDirectoryPath);
    private static readonly DirectoryInfo DirectorySelected = new DirectoryInfo(FullDirectoryPath);
    private static string Extension => ".json";
    private static bool NoMapDirectory => !Directory.Exists(FullDirectoryPath);

    private static void CreateMapFolder()
    {
        Directory.CreateDirectory(FullDirectoryPath);
    }

    public string[] GetAllMapNames()
    {
        if (NoMapDirectory)
        {
            CreateMapFolder();
            return new string[0];
        }
        var paths = Directory.GetFiles(FullDirectoryPath);
        for (var i = 0; i < paths.Length; i++)
        {
            paths[i] = paths[i].Remove(0,FullDirectoryPath.Length + 1);
            paths[i] = paths[i].Remove(paths[i].Length - ".json.gz".Length, ".json.gz".Length);
        }

        return paths;
    }
    
    public void Save(string mapName, string json)
    {
        if (NoMapDirectory)
            CreateMapFolder();
        var path = Path.Combine(FullDirectoryPath, mapName + Extension);
        File.WriteAllText(path, json);
        Compress(DirectorySelected);
        File.Delete(path);
    }

    public string Load(string mapName)
    {
        var fullPath = Path.Combine(FullDirectoryPath, mapName + Extension);
        if (!File.Exists(fullPath+".gz"))
        {
            Debug.LogError("User_map not found in " + fullPath);
            return null;
        }
        Decompress(new FileInfo(fullPath+".gz"));
        if (File.Exists(fullPath))
        {
            var data = File.ReadAllText(fullPath);
            File.Delete(fullPath);
            return data;
        }
        Debug.LogError("User_map not found in " + fullPath);
        return null;
    }

    private static void Compress(DirectoryInfo directorySelected)
    {
        foreach (var fileToCompress in directorySelected.GetFiles())
        {
            using var originalFileStream = fileToCompress.OpenRead();
            if (!((File.GetAttributes(fileToCompress.FullName) &
                   FileAttributes.Hidden) != FileAttributes.Hidden & fileToCompress.Extension != ".gz")) continue;
            using var compressedFileStream = File.Create(fileToCompress.FullName + ".gz");
            using var compressionStream = new GZipStream(compressedFileStream, CompressionMode.Compress);
            originalFileStream.CopyTo(compressionStream);
        }
    }

    private static void Decompress(FileInfo fileToDecompress)
    {
        using var originalFileStream = fileToDecompress.OpenRead();
        var currentFileName = fileToDecompress.FullName;
        var newFileName = currentFileName.Remove(currentFileName.Length - fileToDecompress.Extension.Length);
        using var decompressedFileStream = File.Create(newFileName);
        using var decompressionStream = new GZipStream(originalFileStream, CompressionMode.Decompress);
        decompressionStream.CopyTo(decompressedFileStream);
    }
}