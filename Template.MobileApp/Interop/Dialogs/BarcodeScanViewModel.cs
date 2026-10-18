namespace Template.MobileApp.Interop.Dialogs;

using BarcodeScanning;

public sealed class BarcodeScanViewModel : DialogViewModelBase
{
    // Field

    private readonly IVibration vibration;

    // Property

    public BarcodeController Controller { get; } = new();

    // Command

    public IObserveCommand DetectCommand { get; }

    public IObserveCommand CancelCommand { get; }

    //--------------------------------------------------------------------------------
    // Constructor
    //--------------------------------------------------------------------------------

    public BarcodeScanViewModel(IVibration vibration)
    {
        this.vibration = vibration;

        DetectCommand = MakeDelegateCommand<IReadOnlySet<BarcodeResult>>(CommandMode.Simple, x => _ = DetectAsync(x));
        CancelCommand = MakeAsyncCommand(async () => await CloseAsync());

        Controller.Enable = true;
    }

    //--------------------------------------------------------------------------------
    // Operation
    //--------------------------------------------------------------------------------

    private async Task DetectAsync(IReadOnlySet<BarcodeResult> barcodes)
    {
        if (barcodes.Count > 0)
        {
            vibration.Vibrate(200);
            Controller.Enable = false;

            var barcode = barcodes.First();
            await CloseAsync(new ScanResult(barcode.DisplayValue, barcode.BarcodeFormat.ToString()));
        }
    }
}
