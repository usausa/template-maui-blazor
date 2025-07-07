namespace Template.MobileApp;

using Template.MobileApp.Shell;
using Template.MobileApp.Views;

[ObservableGeneratorOption(Reactive = true, ViewModel = true)]
public sealed partial class MainPageViewModel : ExtendViewModelBase, IAppLifecycle
{
    public IBusyView BusyView { get; }

    [ObservableProperty]
    public partial SelectPage Selected { get; set; }

    public ICommand PageCommand { get; }

    //--------------------------------------------------------------------------------
    // Constructor
    //--------------------------------------------------------------------------------

    public MainPageViewModel(
        IBusyView progressView,
        IReactiveMessenger messenger)
    {
        BusyView = progressView;

        Selected = SelectPage.Home;
        PageCommand = MakeDelegateCommand<SelectPage>(page =>
        {
            Selected = page;
            messenger.Send(page);
        });
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
