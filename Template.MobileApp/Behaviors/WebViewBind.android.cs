namespace Template.MobileApp.Behaviors;

using Android.Webkit;

using Microsoft.AspNetCore.Components.WebView;
using Microsoft.AspNetCore.Components.WebView.Maui;

public static partial class WebViewBind
{
    private const string StopRendererUrl = "chrome://crash";

    private static partial bool TryCheckWebView(out string? packageName, out string? packageVersion, out string? error)
    {
        try
        {
            var package = Android.Webkit.WebView.CurrentWebViewPackage;
            packageName = package?.PackageName;
            packageVersion = package?.VersionName;

            WebSettings.GetDefaultUserAgent(Android.App.Application.Context);

            error = null;
            return true;
        }
        catch (Java.Lang.Exception ex)
        {
            packageName = null;
            packageVersion = null;
            error = ex.Message;
            return false;
        }
    }

    private static partial IDisposable? PrepareWebView(BlazorWebViewInitializedEventArgs args, bool clearCache, Action<string> renderProcessGone)
    {
        var view = args.WebView;
        var client = view.WebViewClient is { } inner ? new DelegatingWebViewClient(inner, renderProcessGone) : null;
        if (client is not null)
        {
            view.SetWebViewClient(client);
        }

        if (clearCache)
        {
            view.ClearCache(true);
            WebStorage.Instance?.DeleteAllData();
            CookieManager.Instance?.RemoveAllCookies(null);
        }

        return client;
    }

    private static partial void StopRenderer(BlazorWebView view)
    {
        (view.Handler?.PlatformView as Android.Webkit.WebView)?.LoadUrl(StopRendererUrl);
    }

    private static partial void DestroyWebView(object? platformView)
    {
        (platformView as Android.Webkit.WebView)?.Destroy();
    }
}
