namespace Template.MobileApp.Extender;

public sealed class PopupActivationPlugin : IPopupPlugin
{
    private readonly IBusyState busyState;

    private IDisposable? busy;

    private int depth;

    public PopupActivationPlugin(IBusyState busyState)
    {
        this.busyState = busyState;
    }

    public void Extend(ContentView view)
    {
    }

    public void OnOpening(ContentView view)
    {
        depth++;
        if (depth == 1)
        {
            busy = busyState.Begin();
        }
    }

    public void OnClosed(ContentView view)
    {
        depth--;
        if (depth == 0)
        {
            busy?.Dispose();
            busy = null;
        }
    }
}
