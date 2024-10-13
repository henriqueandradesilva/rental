namespace CrossCutting.Settings;

public class FirebaseStorageSettings
{
    public string Url { get; set; }

    public FirebaseStorageSettings()
    {

    }

    public FirebaseStorageSettings(
        string url)
    {
        Url = url;
    }
}
