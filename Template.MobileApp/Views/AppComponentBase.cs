namespace Template.MobileApp.Views;

public abstract class AppComponentBase : ComponentBase, IDisposable
{
    private CompositeDisposable? disposables;

    protected ICollection<IDisposable> Disposables => disposables ??= [];

    ~AppComponentBase()
    {
        Dispose(false);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            disposables?.Dispose();
        }
    }

    // TODO Execute, BusyState
}
