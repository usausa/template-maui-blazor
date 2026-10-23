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

    private readonly ILogger<MainPageViewModel> log;

    private readonly IDialog dialog;

    private readonly INotificationService notification;

    // Property

    [ObservableProperty]
    public partial SelectPage Selected { get; set; }

    [ObservableProperty]
    public partial int NotificationCount { get; set; }

    [ObservableProperty]
    public partial bool HasAccountAlert { get; set; }

    // WebView

    public BlazorWebViewController WebView { get; } = new();

    [ObservableProperty]
    public partial bool IsWebError { get; set; }

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
        ILogger<MainPageViewModel> log,
        IReactiveMessenger messenger,
        IDialog dialog,
        INotificationService notification,
        IPlatformInterop platformInterop,
        BackButtonState backButton)
    {
        this.log = log;
        this.dialog = dialog;
        this.notification = notification;

        // Navigation
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
        RetryCommand = MakeDelegateCommand(WebView.RequestReload);
        UpdateWebViewCommand = MakeAsyncCommand(async () =>
        {
            await Launcher.Default.OpenAsync(new Uri(WebViewStoreUrl + (WebView.PackageName ?? DefaultWebViewPackage)));
        });

        // WebView
        WebView.Failed += OnWebViewFailed;
        Disposables.Add(WebView.AsObservable(nameof(BlazorWebViewController.Status)).Subscribe(x =>
        {
            if (x.Status != BlazorWebViewStatus.Started)
            {
                backButton.Clear();
            }

            if (x.Status == BlazorWebViewStatus.Starting)
            {
                log.InfoWebViewCreating(x.PackageName, x.PackageVersion, x.CreateCount);
                Selected = SelectPage.Home;
                AppIcons.SetNavigationSelection(Selected);
            }
            else if (x.Status == BlazorWebViewStatus.Started)
            {
                log.InfoWebViewStarted((long)(x.StartupTime?.TotalMilliseconds ?? 0));
            }

            UpdateWebError(x.Status);
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
        args.Action is null ? $"Notification: {args.Payload}" : $"Notification [{args.Action}]: {args.Payload}";

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
    // WebView
    //--------------------------------------------------------------------------------

    private void OnWebViewFailed(object? sender, BlazorWebViewFailedEventArgs e)
    {
        log.ErrorWebViewFailed(e.Status, e.Detail);
    }

    private void UpdateWebError(BlazorWebViewStatus status)
    {
        (WebErrorTitle, WebErrorMessage) = status switch
        {
            BlazorWebViewStatus.Unavailable => ("この端末の WebView を使えません", "WebView を有効にするか、更新してください。"),
            BlazorWebViewStatus.RendererGone => ("画面が止まりました", "WebView の描画のプロセスが終わりました。"),
            _ => (string.Empty, string.Empty)
        };
        IsWebError = status is BlazorWebViewStatus.Unavailable or BlazorWebViewStatus.RendererGone;
        CanUpdateWebView = status == BlazorWebViewStatus.Unavailable;
    }
}
