namespace Template.MobileApp.Interop;

using Template.MobileApp.Components;
using Template.MobileApp.Interop.Dialogs;

public sealed class PlatformInterop : IPlatformInterop
{
    private const string CameraDeniedMessage = "カメラを使用するための権限が必要です";

    private const string NotificationDeniedMessage = "通知を使用するための権限が必要です";

    private readonly IDialog dialog;

    private readonly IPopupNavigator popupNavigator;

    private readonly INotificationService notificationService;

    private int notificationId;

    public PlatformInterop(
        IDialog dialog,
        IPopupNavigator popupNavigator,
        INotificationService notificationService)
    {
        this.dialog = dialog;
        this.popupNavigator = popupNavigator;
        this.notificationService = notificationService;
    }

    //--------------------------------------------------------------------------------
    // Barcode
    //--------------------------------------------------------------------------------

    public async ValueTask<ScanResult?> ScanBarcodeAsync()
    {
        if (!await RequestPermissionAsync(Permissions.RequestCameraAsync, CameraDeniedMessage))
        {
            return null;
        }

        return await popupNavigator.PopupAsync<ScanResult?>(DialogId.BarcodeScan);
    }

    public ValueTask DisplayBarcodeAsync()
    {
        return popupNavigator.PopupAsync(DialogId.BarcodeDisplay);
    }

    //--------------------------------------------------------------------------------
    // Camera
    //--------------------------------------------------------------------------------

    public async ValueTask<byte[]?> CapturePhotoAsync(int maxSize)
    {
        if (!await RequestPermissionAsync(Permissions.RequestCameraAsync, CameraDeniedMessage))
        {
            return null;
        }

        return await popupNavigator.PopupAsync<int, byte[]?>(DialogId.CameraCapture, maxSize);
    }

    //--------------------------------------------------------------------------------
    // Notification
    //--------------------------------------------------------------------------------

    public async ValueTask<bool> ShowNotificationAsync(string title, string message)
    {
        if (!await RequestPermissionAsync(Permissions.RequestNotificationsAsync, NotificationDeniedMessage))
        {
            return false;
        }

        notificationService.Show(Interlocked.Increment(ref notificationId), title, message, title);
        return true;
    }

    public async ValueTask<bool> ScheduleNotificationAsync(string title, string message, TimeSpan delay)
    {
        if (!await RequestPermissionAsync(Permissions.RequestNotificationsAsync, NotificationDeniedMessage))
        {
            return false;
        }

        notificationService.Schedule(Interlocked.Increment(ref notificationId), title, message, delay, title);
        return true;
    }

    //--------------------------------------------------------------------------------
    // Toast
    //--------------------------------------------------------------------------------

    public ValueTask ShowToastAsync(string message)
    {
        return dialog.Toast(message);
    }

    //--------------------------------------------------------------------------------
    // Helper
    //--------------------------------------------------------------------------------

    private async ValueTask<bool> RequestPermissionAsync(Func<ValueTask<bool>> request, string deniedMessage)
    {
        if (await request())
        {
            return true;
        }

        await dialog.InformationAsync(deniedMessage);
        return false;
    }
}
