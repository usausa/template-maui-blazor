namespace Template.MobileApp.Behaviors;

using Android.Graphics;
using Android.Net.Http;
using Android.OS;
using Android.Views;
using Android.Webkit;

using Microsoft.AspNetCore.Components.WebView;

public static partial class BlazorWebViewBind
{
    //--------------------------------------------------------------------------------
    // Check
    //--------------------------------------------------------------------------------

    private static partial bool TryCheckWebView(out string? packageName, out string? packageVersion, out string? error)
    {
        try
        {
            var package = WebView.CurrentWebViewPackage;
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

    //--------------------------------------------------------------------------------
    // Delegation
    //--------------------------------------------------------------------------------

    private static partial IDisposable? PrepareWebView(BlazorWebViewInitializedEventArgs args, Action pageFinished, Action<string> renderProcessGone)
    {
        var view = args.WebView;
        var client = view.WebViewClient is { } inner ? new DelegatingWebViewClient(inner, pageFinished, renderProcessGone) : null;
        if (client is not null)
        {
            view.SetWebViewClient(client);
        }

        return client;
    }

    private static partial void DestroyWebView(object? platformView)
    {
        (platformView as WebView)?.Destroy();
    }

    //--------------------------------------------------------------------------------
    // DelegatingWebViewClient
    //--------------------------------------------------------------------------------

    private sealed class DelegatingWebViewClient : WebViewClient
    {
        private const string CrashedDetail = "Renderer crashed.";

        private const string KilledDetail = "Renderer was killed by the system.";

        private readonly WebViewClient inner;

        private readonly Action pageFinished;

        private readonly Action<string> renderProcessGone;

        public DelegatingWebViewClient(WebViewClient inner, Action pageFinished, Action<string> renderProcessGone)
        {
            this.inner = inner;
            this.pageFinished = pageFinished;
            this.renderProcessGone = renderProcessGone;
        }

        public override void OnPageFinished(WebView? view, string? url)
        {
            inner.OnPageFinished(view, url);
            pageFinished();
        }

        public override bool OnRenderProcessGone(WebView? view, RenderProcessGoneDetail? detail)
        {
            renderProcessGone((detail?.DidCrash() ?? false) ? CrashedDetail : KilledDetail);
            return true;
        }

        public override void DoUpdateVisitedHistory(WebView? view, string? url, bool isReload) => inner.DoUpdateVisitedHistory(view, url, isReload);

        public override void OnFormResubmission(WebView? view, Message? dontResend, Message? resend) => inner.OnFormResubmission(view, dontResend, resend);

        public override void OnLoadResource(WebView? view, string? url) => inner.OnLoadResource(view, url);

        public override void OnPageCommitVisible(WebView? view, string? url) => inner.OnPageCommitVisible(view, url);

        public override void OnPageStarted(WebView? view, string? url, Bitmap? favicon) => inner.OnPageStarted(view, url, favicon);

        public override void OnReceivedClientCertRequest(WebView? view, ClientCertRequest? request) => inner.OnReceivedClientCertRequest(view, request);

        public override void OnReceivedError(WebView? view, IWebResourceRequest? request, WebResourceError? error) => inner.OnReceivedError(view, request, error);

        public override void OnReceivedHttpAuthRequest(WebView? view, HttpAuthHandler? handler, string? host, string? realm) => inner.OnReceivedHttpAuthRequest(view, handler, host, realm);

        public override void OnReceivedHttpError(WebView? view, IWebResourceRequest? request, WebResourceResponse? errorResponse) => inner.OnReceivedHttpError(view, request, errorResponse);

        public override void OnReceivedLoginRequest(WebView? view, string? realm, string? account, string? args) =>
            inner.OnReceivedLoginRequest(view, realm, account, args);

        public override void OnReceivedSslError(WebView? view, SslErrorHandler? handler, SslError? error) => inner.OnReceivedSslError(view, handler, error);

        public override void OnSafeBrowsingHit(WebView? view, IWebResourceRequest? request, SafeBrowsingThreat threatType, SafeBrowsingResponse? callback) => inner.OnSafeBrowsingHit(view, request, threatType, callback);

        public override void OnScaleChanged(WebView? view, float oldScale, float newScale) => inner.OnScaleChanged(view, oldScale, newScale);

        public override void OnUnhandledInputEvent(WebView? view, InputEvent? e) => inner.OnUnhandledInputEvent(view, e);

        public override void OnUnhandledKeyEvent(WebView? view, KeyEvent? e) => inner.OnUnhandledKeyEvent(view, e);

        public override WebResourceResponse? ShouldInterceptRequest(WebView? view, IWebResourceRequest? request) => inner.ShouldInterceptRequest(view, request);

        public override bool ShouldOverrideKeyEvent(WebView? view, KeyEvent? e) => inner.ShouldOverrideKeyEvent(view, e);

        public override bool ShouldOverrideUrlLoading(WebView? view, IWebResourceRequest? request) => inner.ShouldOverrideUrlLoading(view, request);
    }
}
