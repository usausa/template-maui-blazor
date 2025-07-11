namespace Template.MobileApp.Interop.Dialogs;

using BarcodeScanning;

public sealed class BarcodeScanViewModel : DialogViewModelBase
{
    public BarcodeController Controller { get; } = new();

    public IObserveCommand DetectCommand { get; }

    public BarcodeScanViewModel(
        IPopupNavigator popupNavigator,
        IVibration vibration)
    {
        DetectCommand = new AsyncCommand<IReadOnlySet<BarcodeResult>>(async x =>
        {
            if (x.Count > 0)
            {
                vibration.Vibrate(200);
                Controller.Enable = false;

                await popupNavigator.CloseAsync(x.First().DisplayValue);
            }
        });

        Controller.Enable = true;
    }
}
