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
                    NavigationManager.NavigateTo("/home");
                    break;
                case SelectPage.Search:
                    NavigationManager.NavigateTo("/search");
                    break;
                case SelectPage.Notifications:
                    NavigationManager.NavigateTo("/notification");
                    break;
                case SelectPage.Account:
                    NavigationManager.NavigateTo("/account");
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
