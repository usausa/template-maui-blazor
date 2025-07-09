namespace Template.MobileApp.Interop.Dialogs;

using BarcodeScanning;

public sealed class BarcodeScanViewModel : DialogViewModelBase
{
    public BarcodeController Controller { get; } = new();

    public IObserveCommand DetectCommand { get; }

    public IObserveCommand CloseCommand { get; }

    public BarcodeScanViewModel(IPopupNavigator popupNavigator)
    {
        // TODO
        //CloseCommand = MakeAsyncCommand(async () => await popupNavigator.CloseAsync());
        CloseCommand = new AsyncCommand(async () =>
        {
            System.Diagnostics.Debug.WriteLine("****");
            await popupNavigator.CloseAsync();
        });
        DetectCommand = MakeAsyncCommand<IReadOnlySet<BarcodeResult>>(async x =>
        {
            if (x.Count > 0)
            {
                await popupNavigator.CloseAsync(x.First().DisplayValue);
            }
        });

        Controller.Enable = true;
    }
}
