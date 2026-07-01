namespace TraineeManagement.Api.Data.FileStoreValidation;

public class CustomFileStoreValidation
{
    public long MaxFileSizeInBytes { get; set; }
    public List<string> AllowedExtensions { get; set; } = new();
    public Dictionary<string, string> MagicNumbers { get; set; } = new();
    public string BasePath { get; set; } = string.Empty;
}