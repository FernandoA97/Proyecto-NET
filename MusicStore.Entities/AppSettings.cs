namespace MusicStore.Entities;

public class AppSettings
{
    public Storageconfiguration StorageConfiguration { get; set; } = default!;
    public Jwt Jwt { get; set; } = default!;

}

public class Storageconfiguration
{
    public string Path { get; set; } = default!;
    public string PublicUrl { get; set; } = default!;
}

public class Jwt
{
    public string SecretKey { get; set; } = default!;
    public string Audience { get; set; } = default!;
    public string Issuer { get; set; } = default!;
}