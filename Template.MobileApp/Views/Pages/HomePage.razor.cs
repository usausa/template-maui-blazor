namespace Template.MobileApp.Views.Pages;

using Template.MobileApp.Interop;

public sealed partial class HomePage : AppComponentBase
{
    // Field

    private string? barcode;

    // Property

    [Inject]
    public required IPlatformInterop PlatformInterop { get; set; }

    private Task OnClickScan()
    {
        return ExecuteAsync(async () =>
        {
            barcode = await PlatformInterop.ScanBarcodeAsync();
        });
    }
}
