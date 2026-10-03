namespace Template.MobileApp.Interop.Dialogs;

using BarcodeScanning;

public sealed class BarcodeScanViewModel : DialogViewModelBase
{
    private readonly IPopupNavigator popupNavigator;

    private readonly IVibration vibration;

    public BarcodeController Controller { get; } = new();

    public IObserveCommand DetectCommand { get; }

    //--------------------------------------------------------------------------------
    // Constructor
    //--------------------------------------------------------------------------------

    public BarcodeScanViewModel(
        IPopupNavigator popupNavigator,
        IVibration vibration)
    {
        this.popupNavigator = popupNavigator;
        this.vibration = vibration;

        DetectCommand = MakeDelegateCommand<IReadOnlySet<BarcodeResult>>(CommandMode.Simple, x => _ = DetectAsync(x));

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

            await popupNavigator.CloseAsync(barcodes.First().DisplayValue);
        }
    }
}
