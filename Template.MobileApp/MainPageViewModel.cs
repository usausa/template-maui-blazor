namespace Template.MobileApp;

using Template.MobileApp.Interop;
using Template.MobileApp.Views;

[ObservableGeneratorOption(Reactive = true, ViewModel = true)]
public sealed partial class MainPageViewModel : ExtendViewModelBase, IAppLifecycle
{
    [ObservableProperty]
    public partial SelectPage Selected { get; set; }

    [ObservableProperty]
    public partial int NotificationCount { get; set; }

    [ObservableProperty]
    public partial bool HasAccountAlert { get; set; }

    public IObserveCommand PayCommand { get; }

    public IObserveCommand PageCommand { get; }

    //--------------------------------------------------------------------------------
    // Constructor
    //--------------------------------------------------------------------------------

    public MainPageViewModel(
        IReactiveMessenger messenger,
        Settings settings,
        IPlatformInterop platformInterop)
    {
        Selected = SelectPage.Home;
        PayCommand = MakeAsyncCommand(async () =>
        {
            await platformInterop.DisplayBarcodeAsync(settings.UniqId);
        });
        PageCommand = MakeDelegateCommand<SelectPage>(page =>
        {
            Selected = page;
            messenger.Send(page);
        });

        NotificationCount = 99;
        HasAccountAlert = true;
    }

    //--------------------------------------------------------------------------------
    // Lifecycle
    //--------------------------------------------------------------------------------

    public void OnCreated()
    {
    }

    public void OnActivated()
    {
    }

    public void OnDeactivated()
    {
    }

    public void OnStopped()
    {
    }

    public void OnResumed()
    {
    }

    public void OnDestroying()
    {
    }
}
