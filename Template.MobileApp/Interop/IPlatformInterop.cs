namespace Template.MobileApp.Interop;

public interface IPlatformInterop
{
    //--------------------------------------------------------------------------------
    // Barcode
    //--------------------------------------------------------------------------------

    ValueTask<ScanResult?> ScanBarcodeAsync();

    ValueTask DisplayBarcodeAsync();

    //--------------------------------------------------------------------------------
    // Camera
    //--------------------------------------------------------------------------------

    ValueTask<byte[]?> CapturePhotoAsync(int maxSize);

    //--------------------------------------------------------------------------------
    // Notification
    //--------------------------------------------------------------------------------

    ValueTask<bool> ShowNotificationAsync(string title, string message);

    ValueTask<bool> ScheduleNotificationAsync(string title, string message, TimeSpan delay);

    //--------------------------------------------------------------------------------
    // Toast
    //--------------------------------------------------------------------------------

    ValueTask ShowToastAsync(string message);

    //--------------------------------------------------------------------------------
    // WebView
    //--------------------------------------------------------------------------------

    WebViewDiagnostics GetWebViewDiagnostics();

    void ReloadWebView(bool clearCache);

    void StopWebViewRenderer();
}
