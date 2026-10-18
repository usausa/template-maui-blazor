namespace Template.MobileApp.Behaviors;

using Microsoft.AspNetCore.Components.WebView;
using Microsoft.AspNetCore.Components.WebView.Maui;

using Smart.Maui.Interactivity;

public static partial class WebViewBind
{
    private static readonly TimeSpan StartTimeout = TimeSpan.FromSeconds(15);

    private static readonly TimeSpan DestroyDelay = TimeSpan.FromSeconds(1);

    //--------------------------------------------------------------------------------
    // Property
    //--------------------------------------------------------------------------------

    private static partial bool TryCheckWebView(out string? packageName, out string? packageVersion, out string? error);

    private static partial IDisposable? PrepareWebView(BlazorWebViewInitializedEventArgs args, bool clearCache, Action<string> renderProcessGone);

    private static partial void StopRenderer(BlazorWebView view);

    private static partial void DestroyWebView(object? platformView);

    public static readonly BindableProperty ControllerProperty = BindableProperty.CreateAttached(
        "Controller",
        typeof(WebViewState),
        typeof(WebViewBind),
        null,
        propertyChanged: BindChanged);

    public static WebViewState? GetController(BindableObject bindable) =>
        (WebViewState?)bindable.GetValue(ControllerProperty);

    public static void SetController(BindableObject bindable, WebViewState? value) =>
        bindable.SetValue(ControllerProperty, value);

    public static readonly BindableProperty TemplateProperty = BindableProperty.CreateAttached(
        "Template",
        typeof(DataTemplate),
        typeof(WebViewBind),
        null);

    public static DataTemplate? GetTemplate(BindableObject bindable) =>
        (DataTemplate?)bindable.GetValue(TemplateProperty);

    public static void SetTemplate(BindableObject bindable, DataTemplate? value) =>
        bindable.SetValue(TemplateProperty, value);

    private static void BindChanged(BindableObject bindable, object? oldValue, object? newValue)
    {
        if (bindable is not ContentView view)
        {
            return;
        }

        if (oldValue is not null)
        {
            var behavior = view.Behaviors.FirstOrDefault(static x => x is WebViewBindBehavior);
            if (behavior is not null)
            {
                view.Behaviors.Remove(behavior);
            }
        }

        if (newValue is not null)
        {
            view.Behaviors.Add(new WebViewBindBehavior());
        }
    }

    //--------------------------------------------------------------------------------
    // Behavior
    //--------------------------------------------------------------------------------

    private sealed class WebViewBindBehavior : BehaviorBase<ContentView>
    {
        private WebViewState? controller;

        private IDispatcherTimer? timer;

        private BlazorWebView? webView;

        private IDisposable? webViewClient;

        private bool clearCache;

        protected override void OnAttachedTo(ContentView bindable)
        {
            base.OnAttachedTo(bindable);

            controller = GetController(bindable);
            if (controller is not null)
            {
                controller.ReloadRequest += OnReloadRequest;
                controller.StopRendererRequest += OnStopRendererRequest;

                timer = bindable.Dispatcher.CreateTimer();
                timer.Interval = StartTimeout;
                timer.IsRepeating = false;
                timer.Tick += OnTimerTick;

                Create();
            }
        }

        protected override void OnDetachingFrom(ContentView bindable)
        {
            if (controller is not null)
            {
                controller.ReloadRequest -= OnReloadRequest;
                controller.StopRendererRequest -= OnStopRendererRequest;
            }

            Remove();

            if (timer is not null)
            {
                timer.Tick -= OnTimerTick;
            }

            controller = null;
            timer = null;

            base.OnDetachingFrom(bindable);
        }

        private void OnReloadRequest(object? sender, WebViewReloadEventArgs e)
        {
            clearCache = e.ClearCache;
            AssociatedObject?.Dispatcher.Dispatch(Create);
        }

        private void OnStopRendererRequest(object? sender, EventArgs e)
        {
            if (webView is not null)
            {
                StopRenderer(webView);
            }
        }

        private void OnTimerTick(object? sender, EventArgs e)
        {
            if ((controller is not null) && !controller.IsStarted && !controller.IsFailed)
            {
                controller.NotifyFailed(WebViewFailure.StartTimeout, null);
            }
        }

        private void OnBlazorWebViewInitialized(object? sender, BlazorWebViewInitializedEventArgs e)
        {
            webViewClient = PrepareWebView(e, clearCache, OnRenderProcessGone);
            clearCache = false;
        }

        private void OnRenderProcessGone(string detail)
        {
            timer?.Stop();
            controller?.NotifyFailed(WebViewFailure.RendererGone, detail);
            AssociatedObject?.Dispatcher.Dispatch(Remove);
        }

        private void Create()
        {
            if ((controller is null) || (AssociatedObject is null))
            {
                return;
            }

            Remove();

            if (!TryCheckWebView(out var packageName, out var packageVersion, out var error))
            {
                controller.NotifyFailed(WebViewFailure.Unavailable, error);
                return;
            }

            if (GetTemplate(AssociatedObject)?.CreateContent() is not BlazorWebView view)
            {
                return;
            }

            controller.NotifyCreating(packageName, packageVersion);

            webView = view;
            webView.BlazorWebViewInitialized += OnBlazorWebViewInitialized;
            AssociatedObject.Content = webView;

            timer?.Start();
        }

        private void Remove()
        {
            timer?.Stop();

            if (webView is null)
            {
                return;
            }

            var platformView = webView.Handler?.PlatformView;
            var client = webViewClient;
            webView.BlazorWebViewInitialized -= OnBlazorWebViewInitialized;
            AssociatedObject?.Content = null;

            webView.Handler?.DisconnectHandler();
            webViewClient = null;
            webView = null;

            Dispatcher.DispatchDelayed(DestroyDelay, () =>
            {
                DestroyWebView(platformView);
                client?.Dispose();
            });
        }
    }
}
