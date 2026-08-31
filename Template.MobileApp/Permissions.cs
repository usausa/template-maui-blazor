namespace Template.MobileApp;

#pragma warning disable CA1724
public static class Permissions
{
    public static async ValueTask<bool> RequestCameraAsync()
    {
        var status = await Microsoft.Maui.ApplicationModel.Permissions.RequestAsync<Microsoft.Maui.ApplicationModel.Permissions.Camera>();
        return status is PermissionStatus.Granted;
    }

    public static async ValueTask<bool> RequestLocationAsync()
    {
        var status = await Microsoft.Maui.ApplicationModel.Permissions.RequestAsync<Microsoft.Maui.ApplicationModel.Permissions.LocationAlways>();
        return status is PermissionStatus.Granted;
    }
}
#pragma warning restore CA1724
