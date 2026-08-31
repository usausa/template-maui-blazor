namespace Template.MobileApp.State;

#pragma warning disable CA1724
public sealed class Settings
{
    private readonly IPreferences preferences;

    public Settings(IPreferences preferences)
    {
        this.preferences = preferences;
    }

    // Id

    public string UniqueId
    {
        get => preferences.Get<string>(nameof(UniqueId), default!);
        set => preferences.Set(nameof(UniqueId), value);
    }

    // API

    public string ApiEndPoint
    {
        get => preferences.Get<string>(nameof(ApiEndPoint), default!);
        set => preferences.Set(nameof(ApiEndPoint), value);
    }
}
#pragma warning restore CA1724
