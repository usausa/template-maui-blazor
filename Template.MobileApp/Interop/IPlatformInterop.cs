namespace Template.MobileApp.Interop;

public interface IPlatformInterop
{
    ValueTask<ScanResult?> ScanBarcodeAsync();

    ValueTask DisplayBarcodeAsync();

    ValueTask<byte[]?> CapturePhotoAsync(int maxSize);

    ValueTask<bool> ShowNotificationAsync(string title, string message);

    ValueTask<bool> ScheduleNotificationAsync(string title, string message, TimeSpan delay);

    ValueTask ShowToastAsync(string message);

    WebViewDiagnostics GetWebViewDiagnostics();

    void ReloadWebView(bool clearCache);

    void StopWebViewRenderer();
}
