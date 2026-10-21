namespace Template.MobileApp;

using Template.MobileApp.Components;
using Template.MobileApp.Interop;
using Template.MobileApp.Markup;
using Template.MobileApp.Views;

[ObservableGeneratorOption(Reactive = true, ViewModel = true)]
public sealed partial class MainPageViewModel : ExtendViewModelBase, IAppLifecycle
{
    // Field

    private const string WebViewStoreUrl = "https://play.google.com/store/apps/details?id=";

    private const string DefaultWebViewPackage = "com.google.android.webview";

    private readonly IDialog dialog;

    private readonly INotificationService notification;

    // Property

    public WebViewState WebView { get; }

    [ObservableProperty]
    public partial SelectPage Selected { get; set; }

    [ObservableProperty]
    public partial int NotificationCount { get; set; }

    [ObservableProperty]
    public partial bool HasAccountAlert { get; set; }

    [ObservableProperty]
    public partial string WebErrorTitle { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string WebErrorMessage { get; set; } = string.Empty;

    [ObservableProperty]
    public partial bool CanUpdateWebView { get; set; }

    // Command

    public IObserveCommand PayCommand { get; }

    public IObserveCommand PageCommand { get; }

    public IObserveCommand RetryCommand { get; }

    public IObserveCommand UpdateWebViewCommand { get; }

    //--------------------------------------------------------------------------------
    // Constructor
    //--------------------------------------------------------------------------------

    public MainPageViewModel(
        IReactiveMessenger messenger,
        IDialog dialog,
        INotificationService notification,
        IPlatformInterop platformInterop,
        WebViewState webView,
        BackButtonState backButton)
    {
        this.dialog = dialog;
        this.notification = notification;
        WebView = webView;

        Selected = SelectPage.Home;
        AppIcons.SetNavigationSelection(Selected);
        PayCommand = MakeAsyncCommand(async () =>
        {
            await platformInterop.DisplayBarcodeAsync();
        });
        PageCommand = MakeDelegateCommand<SelectPage>(page =>
        {
            Selected = page;
            AppIcons.SetNavigationSelection(page);
            messenger.Send(page);
        });
        RetryCommand = MakeDelegateCommand(() => webView.RequestReload(false));
        UpdateWebViewCommand = MakeAsyncCommand(async () =>
        {
            await Launcher.Default.OpenAsync(new Uri(WebViewStoreUrl + (webView.PackageName ?? DefaultWebViewPackage)));
        });

        Disposables.Add(webView.AsObservable(nameof(WebViewState.Failure)).Subscribe(x => UpdateWebError(x.Failure)));
        Disposables.Add(webView.AsObservable(nameof(WebViewState.IsFailed)).Where(static x => x.IsFailed).Subscribe(_ => backButton.Clear()));
        Disposables.Add(webView.AsObservable(nameof(WebViewState.CreateCount)).Subscribe(_ =>
        {
            Selected = SelectPage.Home;
            AppIcons.SetNavigationSelection(Selected);
            backButton.Clear();
        }));

        NotificationCount = 99;
        HasAccountAlert = true;
    }

    //--------------------------------------------------------------------------------
    // Lifecycle
    //--------------------------------------------------------------------------------

    // ReSharper disable once AsyncVoidMethod
    public async void OnCreated()
    {
        // ReSharper disable AsyncVoidLambda
        Disposables.Add(notification.TappedAsObservable().ObserveOnCurrentContext().Subscribe(async x => await dialog.Toast(FormatNotificationTap(x), true)));
        // ReSharper restore AsyncVoidLambda
        if (notification.TakePendingTap() is { } pending)
        {
            await dialog.Toast(FormatNotificationTap(pending), true);
        }
    }

    private static string FormatNotificationTap(NotificationTappedEventArgs args) =>
        $"通知: {args.Payload}";

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

    //--------------------------------------------------------------------------------
    // Helper
    //--------------------------------------------------------------------------------

    private void UpdateWebError(WebViewFailure failure)
    {
        (WebErrorTitle, WebErrorMessage) = failure switch
        {
            WebViewFailure.Unavailable => ("この端末の WebView を使えません", "WebView を有効にするか、更新してください。"),
            WebViewFailure.StartTimeout => ("画面を開けませんでした", "画面の読み込みが終わりませんでした。"),
            WebViewFailure.RendererGone => ("画面が止まりました", "WebView の描画のプロセスが終わりました。"),
            _ => (string.Empty, string.Empty)
        };
        CanUpdateWebView = failure == WebViewFailure.Unavailable;
    }
}
