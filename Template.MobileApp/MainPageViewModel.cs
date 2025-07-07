namespace Template.MobileApp;

using Template.MobileApp.Shell;
using Template.MobileApp.Views;

[ObservableGeneratorOption(Reactive = true, ViewModel = true)]
public sealed partial class MainPageViewModel : ExtendViewModelBase, IAppLifecycle
{
    private readonly IScreen screen;

    public IBusyView BusyView { get; }

    [ObservableProperty]
    public partial SelectedPage Selected { get; set; }

    public ICommand PageCommand { get; }

    //--------------------------------------------------------------------------------
    // Constructor
    //--------------------------------------------------------------------------------

    public MainPageViewModel(
        ILogger<MainPageViewModel> log,
        IBusyView progressView,
        IScreen screen,
        IDialog dialog)
    {
        BusyView = progressView;
        this.screen = screen;

        Selected = SelectedPage.Home;

        // TODO
        PageCommand = MakeDelegateCommand<SelectedPage>(page => Selected = page);

        // Screen lock detection
        // ReSharper disable AsyncVoidLambda
        Disposables.Add(screen.StateChangedAsObservable().ObserveOnCurrentContext().Subscribe(async x =>
        {
            log.DebugScreenStateChanged(x.ScreenOn);
            if (x.ScreenOn)
            {
                await dialog.Toast("Screen on", true);
            }
        }));
        // ReSharper restore AsyncVoidLambda
    }

    //--------------------------------------------------------------------------------
    // Lifecycle
    //--------------------------------------------------------------------------------

    public void OnCreated()
    {
        screen.EnableDetectScreenState(true);
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
