namespace Template.MobileApp.Interop.Dialogs;

public static class DialogSize
{
    public static double ScreenHeight { get; } = DeviceDisplay.Current.MainDisplayInfo.Height / DeviceDisplay.Current.MainDisplayInfo.Density;

    public static double ScreenWidth { get; } = DeviceDisplay.Current.MainDisplayInfo.Width / DeviceDisplay.Current.MainDisplayInfo.Density;

    // Dialog

    public static double LargeWidth { get; } = ScreenWidth * 0.8;
}
