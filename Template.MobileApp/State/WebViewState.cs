namespace Template.MobileApp.State;

using System.Diagnostics;

public enum WebViewFailure
{
    None,
    Unavailable,
    StartTimeout,
    RendererGone
}

public sealed class WebViewReloadEventArgs : EventArgs
{
    public bool ClearCache { get; init; }
}

public sealed partial class WebViewState : ObservableObject
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public event EventHandler<WebViewReloadEventArgs>? ReloadRequest;

    [EditorBrowsable(EditorBrowsableState.Never)]
    public event EventHandler? StopRendererRequest;

    private readonly ILogger<WebViewState> log;

    private long createdTimestamp;

    // Property

    [ObservableProperty]
    public partial WebViewFailure Failure { get; private set; }

    [ObservableProperty]
    public partial bool IsFailed { get; private set; }

    [ObservableProperty]
    public partial string? FailureDetail { get; private set; }

    [ObservableProperty]
    public partial int CreateCount { get; private set; }

    public bool IsStarted { get; private set; }

    public TimeSpan? StartupTime { get; private set; }

    public string? PackageName { get; private set; }

    public string? PackageVersion { get; private set; }

    public WebViewState(ILogger<WebViewState> log)
    {
        this.log = log;
    }

    //--------------------------------------------------------------------------------
    // Host
    //--------------------------------------------------------------------------------

    public void NotifyCreating(string? packageName, string? packageVersion)
    {
        PackageName = packageName;
        PackageVersion = packageVersion;
        IsStarted = false;
        StartupTime = null;
        createdTimestamp = Stopwatch.GetTimestamp();
        UpdateFailure(WebViewFailure.None, null);
        CreateCount++;

        log.InfoWebViewCreating(packageName, packageVersion, CreateCount);
    }

    public void NotifyStarted()
    {
        if (IsStarted)
        {
            return;
        }

        IsStarted = true;
        StartupTime = Stopwatch.GetElapsedTime(createdTimestamp);

        log.InfoWebViewStarted((long)StartupTime.Value.TotalMilliseconds);

        if (Failure == WebViewFailure.StartTimeout)
        {
            UpdateFailure(WebViewFailure.None, null);
        }
    }

    public void NotifyFailed(WebViewFailure failure, string? detail)
    {
        log.ErrorWebViewFailed(failure, detail);

        UpdateFailure(failure, detail);
    }

    //--------------------------------------------------------------------------------
    // Request
    //--------------------------------------------------------------------------------

    public void RequestReload(bool clearCache)
    {
        log.InfoWebViewReloadRequested(clearCache);

        ReloadRequest?.Invoke(this, new WebViewReloadEventArgs { ClearCache = clearCache });
    }

    public void RequestStopRenderer()
    {
        StopRendererRequest?.Invoke(this, EventArgs.Empty);
    }

    //--------------------------------------------------------------------------------
    // Helper
    //--------------------------------------------------------------------------------

    private void UpdateFailure(WebViewFailure failure, string? detail)
    {
        FailureDetail = detail;
        IsFailed = failure != WebViewFailure.None;
        Failure = failure;
    }
}
