namespace Template.MobileApp.Interop;

using Template.MobileApp.Components;
using Template.MobileApp.Interop.Dialogs;

public sealed class PlatformInterop : IPlatformInterop
{
    private const string CameraDeniedMessage = "カメラを使うには、権限の許可が要ります。";

    private const string NotificationDeniedMessage = "通知を出すには、権限の許可が要ります。";

    private readonly IDialog dialog;

    private readonly IPopupNavigator popupNavigator;

    private readonly INotificationService notificationService;

    private readonly DeviceState deviceState;

    private readonly WebViewState webViewState;

    private int notificationId;

    public PlatformInterop(
        IDialog dialog,
        IPopupNavigator popupNavigator,
        INotificationService notificationService,
        DeviceState deviceState,
        WebViewState webViewState)
    {
        this.dialog = dialog;
        this.popupNavigator = popupNavigator;
        this.notificationService = notificationService;
        this.deviceState = deviceState;
        this.webViewState = webViewState;
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
    // WebView
    //--------------------------------------------------------------------------------

    public WebViewDiagnostics GetWebViewDiagnostics()
    {
        return new WebViewDiagnostics(
            webViewState.PackageName,
            webViewState.PackageVersion,
            webViewState.StartupTime,
            webViewState.CreateCount - 1,
            deviceState.NetworkAccess);
    }

    public void ReloadWebView(bool clearCache)
    {
        webViewState.RequestReload(clearCache);
    }

    public void StopWebViewRenderer()
    {
        webViewState.RequestStopRenderer();
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
