namespace Template.MobileApp.State;

public sealed class BackButtonState
{
    private readonly List<Func<bool>> handlers = [];

    public IDisposable Subscribe(Func<bool> handler)
    {
        handlers.Add(handler);
        return Disposable.Create(() => handlers.Remove(handler));
    }

    public bool Publish() => (handlers.Count > 0) && handlers[^1]();

    public void Clear() => handlers.Clear();
}
