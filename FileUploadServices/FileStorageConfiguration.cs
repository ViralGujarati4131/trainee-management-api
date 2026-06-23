namespace TraineeManagementApi.FileStorage.Configurations;

public class FileStorageConfiguration
{
    public string RootPath { get; set; } = string.Empty;
    public long MaxFileSizeInBytes { get; set; }
    public List<string> AllowedExtensions { get; set; } = new();
    public Dictionary<string, string> MagicNumbers { get; set; } = new();
}