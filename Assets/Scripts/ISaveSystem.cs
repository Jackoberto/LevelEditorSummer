public interface ISaveSystem
{
    string DirectoryPath { get; }
    string[] GetAllMapNames();
    void Save(string mapName, string json);
    string Load(string mapName);
}