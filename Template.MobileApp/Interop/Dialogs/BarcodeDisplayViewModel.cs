namespace Template.MobileApp.Interop.Dialogs;

public sealed partial class BarcodeDisplayViewModel : DialogViewModelBase, IPopupInitialize<string>
{
    [ObservableProperty]
    public partial string? Barcode { get; set; }

    public void Initialize(string parameter)
    {
        Barcode = parameter;
    }
}
