namespace Template.MobileApp.Views.Pages;

using Template.MobileApp.Interop;

public sealed partial class HomePage : AppComponentBase
{
    private string? barcode;

    [Inject]
    public required IPlatformInterop PlatformInterop { get; set; }

    private async Task OnClickScan()
    {
        barcode = await PlatformInterop.ScanBarcodeAsync();
    }
}
