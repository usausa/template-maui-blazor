#pragma warning disable IDE0130
// ReSharper disable once CheckNamespace
namespace Template.MobileApp;

using Android.Graphics;
using Android.Net.Http;
using Android.OS;
using Android.Views;
using Android.Webkit;

public sealed class DelegatingWebViewClient : WebViewClient
{
    private const string CrashedDetail = "Renderer crashed.";

    private const string KilledDetail = "Renderer was killed by the system.";

    private readonly WebViewClient inner;

    private readonly Action<string> renderProcessGone;

    public DelegatingWebViewClient(WebViewClient inner, Action<string> renderProcessGone)
    {
        this.inner = inner;
        this.renderProcessGone = renderProcessGone;
    }

    public override bool OnRenderProcessGone(Android.Webkit.WebView? view, RenderProcessGoneDetail? detail)
    {
        renderProcessGone((detail?.DidCrash() ?? false) ? CrashedDetail : KilledDetail);
        return true;
    }

    public override void DoUpdateVisitedHistory(Android.Webkit.WebView? view, string? url, bool isReload) =>
        inner.DoUpdateVisitedHistory(view, url, isReload);

    public override void OnFormResubmission(Android.Webkit.WebView? view, Message? dontResend, Message? resend) =>
        inner.OnFormResubmission(view, dontResend, resend);

    public override void OnLoadResource(Android.Webkit.WebView? view, string? url) =>
        inner.OnLoadResource(view, url);

    public override void OnPageCommitVisible(Android.Webkit.WebView? view, string? url) =>
        inner.OnPageCommitVisible(view, url);

    public override void OnPageFinished(Android.Webkit.WebView? view, string? url) =>
        inner.OnPageFinished(view, url);

    public override void OnPageStarted(Android.Webkit.WebView? view, string? url, Bitmap? favicon) =>
        inner.OnPageStarted(view, url, favicon);

    public override void OnReceivedClientCertRequest(Android.Webkit.WebView? view, ClientCertRequest? request) =>
        inner.OnReceivedClientCertRequest(view, request);

    public override void OnReceivedError(Android.Webkit.WebView? view, IWebResourceRequest? request, WebResourceError? error) =>
        inner.OnReceivedError(view, request, error);

    public override void OnReceivedHttpAuthRequest(Android.Webkit.WebView? view, HttpAuthHandler? handler, string? host, string? realm) =>
        inner.OnReceivedHttpAuthRequest(view, handler, host, realm);

    public override void OnReceivedHttpError(Android.Webkit.WebView? view, IWebResourceRequest? request, WebResourceResponse? errorResponse) =>
        inner.OnReceivedHttpError(view, request, errorResponse);

    public override void OnReceivedLoginRequest(Android.Webkit.WebView? view, string? realm, string? account, string? args) =>
        inner.OnReceivedLoginRequest(view, realm, account, args);

    public override void OnReceivedSslError(Android.Webkit.WebView? view, SslErrorHandler? handler, SslError? error) =>
        inner.OnReceivedSslError(view, handler, error);

    public override void OnSafeBrowsingHit(Android.Webkit.WebView? view, IWebResourceRequest? request, SafeBrowsingThreat threatType, SafeBrowsingResponse? callback) =>
        inner.OnSafeBrowsingHit(view, request, threatType, callback);

    public override void OnScaleChanged(Android.Webkit.WebView? view, float oldScale, float newScale) =>
        inner.OnScaleChanged(view, oldScale, newScale);

    public override void OnUnhandledInputEvent(Android.Webkit.WebView? view, InputEvent? e) =>
        inner.OnUnhandledInputEvent(view, e);

    public override void OnUnhandledKeyEvent(Android.Webkit.WebView? view, KeyEvent? e) =>
        inner.OnUnhandledKeyEvent(view, e);

    public override WebResourceResponse? ShouldInterceptRequest(Android.Webkit.WebView? view, IWebResourceRequest? request) =>
        inner.ShouldInterceptRequest(view, request);

    public override bool ShouldOverrideKeyEvent(Android.Webkit.WebView? view, KeyEvent? e) =>
        inner.ShouldOverrideKeyEvent(view, e);

    public override bool ShouldOverrideUrlLoading(Android.Webkit.WebView? view, IWebResourceRequest? request) =>
        inner.ShouldOverrideUrlLoading(view, request);
}
