namespace Template.MobileApp.Messaging;

using System.Diagnostics;

public enum BlazorWebViewStatus
{
    None,
    Starting,
    Started,
    Unavailable,
    RendererGone
}

public sealed class BlazorWebViewFailedEventArgs : EventArgs
{
    public BlazorWebViewStatus Status { get; init; }

    public string? Detail { get; init; }
}

public sealed partial class BlazorWebViewController : ObservableObject
{
    public event EventHandler<BlazorWebViewFailedEventArgs>? Failed;

    [EditorBrowsable(EditorBrowsableState.Never)]
    public event EventHandler? ReloadRequest;

    private long createdTimestamp;

    // Property

    [ObservableProperty]
    public partial BlazorWebViewStatus Status { get; private set; }

    public TimeSpan? StartupTime { get; private set; }

    public string? PackageName { get; private set; }

    public string? PackageVersion { get; private set; }

    public int CreateCount { get; private set; }

    //--------------------------------------------------------------------------------
    // Host
    //--------------------------------------------------------------------------------

    public void NotifyCreating(string? packageName, string? packageVersion)
    {
        PackageName = packageName;
        PackageVersion = packageVersion;
        StartupTime = null;
        createdTimestamp = Stopwatch.GetTimestamp();
        CreateCount++;

        Status = BlazorWebViewStatus.Starting;
    }

    public void NotifyStarted()
    {
        if (Status != BlazorWebViewStatus.Starting)
        {
            return;
        }

        StartupTime = Stopwatch.GetElapsedTime(createdTimestamp);

        Status = BlazorWebViewStatus.Started;
    }

    public void NotifyUnavailable(string? error)
    {
        Status = BlazorWebViewStatus.Unavailable;

        Failed?.Invoke(this, new BlazorWebViewFailedEventArgs { Status = BlazorWebViewStatus.Unavailable, Detail = error });
    }

    public void NotifyRendererGone(string detail)
    {
        Status = BlazorWebViewStatus.RendererGone;

        Failed?.Invoke(this, new BlazorWebViewFailedEventArgs { Status = BlazorWebViewStatus.RendererGone, Detail = detail });
    }

    //--------------------------------------------------------------------------------
    // Request
    //--------------------------------------------------------------------------------

    public void RequestReload()
    {
        ReloadRequest?.Invoke(this, EventArgs.Empty);
    }
}
