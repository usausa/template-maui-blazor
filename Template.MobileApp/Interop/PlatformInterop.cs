namespace Template.MobileApp.Interop;

using Template.MobileApp.Interop.Dialogs;

public sealed class PlatformInterop : IPlatformInterop
{
    private readonly IPopupNavigator popupNavigator;

    public PlatformInterop(IPopupNavigator popupNavigator)
    {
        this.popupNavigator = popupNavigator;
    }

    public ValueTask<string?> ScanBarcodeAsync()
    {
        return popupNavigator.PopupAsync<string?>(DialogId.BarcodeScan);
    }

    public ValueTask DisplayBarcodeAsync(string barcode)
    {
        return popupNavigator.PopupAsync(DialogId.BarcodeDisplay, barcode);
    }
}
