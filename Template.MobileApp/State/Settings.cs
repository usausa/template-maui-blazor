namespace Template.MobileApp.State;

public sealed class Settings
{
    private readonly IPreferences preferences;

    public Settings(IPreferences preferences)
    {
        this.preferences = preferences;
    }

    // Id

    public string UniqId
    {
        get => preferences.Get<string>(nameof(UniqId), default!);
        set => preferences.Set(nameof(UniqId), value);
    }

    // API

    public string ApiEndPoint
    {
        get => preferences.Get<string>(nameof(ApiEndPoint), default!);
        set => preferences.Set(nameof(ApiEndPoint), value);
    }
}
