namespace Template.MobileApp.Behaviors;

using Microsoft.AspNetCore.Components.WebView;
using Microsoft.AspNetCore.Components.WebView.Maui;

using Smart.Maui.Interactivity;

public static partial class BlazorWebViewBind
{
    private static readonly TimeSpan DestroyDelay = TimeSpan.FromSeconds(1);

    //--------------------------------------------------------------------------------
    // Property
    //--------------------------------------------------------------------------------

    private static partial bool TryCheckWebView(out string? packageName, out string? packageVersion, out string? error);

    private static partial IDisposable? PrepareWebView(BlazorWebViewInitializedEventArgs args, Action pageFinished, Action<string> renderProcessGone);

    private static partial void DestroyWebView(object? platformView);

    public static readonly BindableProperty ControllerProperty = BindableProperty.CreateAttached(
        "Controller",
        typeof(BlazorWebViewController),
        typeof(BlazorWebViewBind),
        null,
        propertyChanged: BindChanged);

    public static BlazorWebViewController? GetController(BindableObject bindable) =>
        (BlazorWebViewController?)bindable.GetValue(ControllerProperty);

    public static void SetController(BindableObject bindable, BlazorWebViewController? value) =>
        bindable.SetValue(ControllerProperty, value);

    public static readonly BindableProperty TemplateProperty = BindableProperty.CreateAttached(
        "Template",
        typeof(DataTemplate),
        typeof(BlazorWebViewBind),
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
            var behavior = view.Behaviors.FirstOrDefault(static x => x is BlazorWebViewBindBehavior);
            if (behavior is not null)
            {
                view.Behaviors.Remove(behavior);
            }
        }

        if (newValue is not null)
        {
            view.Behaviors.Add(new BlazorWebViewBindBehavior());
        }
    }

    //--------------------------------------------------------------------------------
    // Behavior
    //--------------------------------------------------------------------------------

    private sealed class BlazorWebViewBindBehavior : BehaviorBase<ContentView>
    {
        private BlazorWebViewController? controller;

        private BlazorWebView? webView;

        private IDisposable? webViewClient;

        protected override void OnAttachedTo(ContentView bindable)
        {
            base.OnAttachedTo(bindable);

            controller = GetController(bindable);
            if (controller is not null)
            {
                controller.ReloadRequest += OnReloadRequest;

                Create();
            }
        }

        protected override void OnDetachingFrom(ContentView bindable)
        {
            if (controller is not null)
            {
                controller.ReloadRequest -= OnReloadRequest;
            }

            Remove();

            controller = null;

            base.OnDetachingFrom(bindable);
        }

        private void OnReloadRequest(object? sender, EventArgs e)
        {
            AssociatedObject?.Dispatcher.Dispatch(Create);
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
                controller.NotifyUnavailable(error);
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
        }

        private void Remove()
        {
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

        //--------------------------------------------------------------------------------
        // WebView
        //--------------------------------------------------------------------------------

        private void OnBlazorWebViewInitialized(object? sender, BlazorWebViewInitializedEventArgs e)
        {
            webViewClient = PrepareWebView(e, OnPageFinished, OnRenderProcessGone);
        }

        private void OnPageFinished()
        {
            controller?.NotifyStarted();
        }

        private void OnRenderProcessGone(string detail)
        {
            controller?.NotifyRendererGone(detail);
            AssociatedObject?.Dispatcher.Dispatch(Remove);
        }
    }
}
