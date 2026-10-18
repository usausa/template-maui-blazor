namespace Template.MobileApp.Views.Layout;

public sealed class PageNavigator : ComponentBase, IDisposable
{
    private IDisposable? subscription;

    [Inject]
    public required IReactiveMessenger Messenger { get; set; }

    [Inject]
    public required NavigationManager NavigationManager { get; set; }

    protected override void OnInitialized()
    {
        subscription = Messenger.Observe<SelectPage>().Subscribe(x =>
        {
            switch (x)
            {
                case SelectPage.Home:
                    NavigationManager.NavigateTo("/", replace: true);
                    break;
                case SelectPage.Search:
                    NavigationManager.NavigateTo("/search", replace: true);
                    break;
                case SelectPage.Notifications:
                    NavigationManager.NavigateTo("/notification", replace: true);
                    break;
                case SelectPage.Account:
                    NavigationManager.NavigateTo("/account", replace: true);
                    break;
            }
        });
    }

    public void Dispose()
    {
        subscription?.Dispose();
        subscription = null;
    }
}
