namespace MusicStore.Entities;

public class AppSettings
{
    public Storageconfiguration StorageConfiguration { get; set; } = default!;

    public class Storageconfiguration
    {
        public string Path { get; set; } = default!;
        public string PublicUrl { get; set; } = default!;
    }

}