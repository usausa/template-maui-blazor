namespace Template.MobileApp.Interop;

public interface IPlatformInterop
{
    ValueTask<string?> ScanBarcodeAsync();

    ValueTask DisplayBarcodeAsync(string barcode);
}
