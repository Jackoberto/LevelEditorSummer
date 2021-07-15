public interface ISaveSystem
{
    string[] GetAllMapNames();
    void Save(string mapName, string json);
    string Load(string mapName);
}