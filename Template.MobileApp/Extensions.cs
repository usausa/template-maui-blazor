namespace Template.MobileApp;

using System.Reflection;

public static class Extensions
{
    //--------------------------------------------------------------------------------
    // Resource
    //--------------------------------------------------------------------------------

    public static T FindResource<T>(this ResourceDictionary resource, string key) =>
        resource.TryGetValue(key, out var value) ? (T)value : default!;

    public static IEnumerable<(string Key, T Value)> EnumValues<T>(this ResourceDictionary resource)
    {
        if (resource is { } resources)
        {
            foreach (var key in resources.Keys)
            {
                if (resources[key] is T value)
                {
                    yield return (key, value);
                }
            }

            if (resources.MergedDictionaries is not null)
            {
                foreach (var dictionary in resources.MergedDictionaries)
                {
                    foreach (var key in dictionary.Keys)
                    {
                        if (resources[key] is T value)
                        {
                            yield return (key, value);
                        }
                    }
                }
            }
        }
    }

    public static IEnumerable<Type> UnderNamespaceTypes(this Assembly assembly, Type baseNamespaceType)
    {
        var ns = baseNamespaceType.Namespace!;
        return assembly.ExportedTypes.Where(x => x.Namespace?.StartsWith(ns, StringComparison.Ordinal) ?? false);
    }

    //--------------------------------------------------------------------------------
    // Reactive
    //--------------------------------------------------------------------------------

    public static IObservable<EventArgs> TickAsObservable(this IDispatcherTimer timer) =>
        Observable.FromEvent<EventHandler, EventArgs>(static h => (_, e) => h(e), h => timer.Tick += h, h => timer.Tick -= h);

    public static IObservable<ScreenStateEventArgs> StateChangedAsObservable(this IScreen screen) =>
        Observable.FromEvent<EventHandler<ScreenStateEventArgs>, ScreenStateEventArgs>(static h => (_, e) => h(e), h => screen.ScreenStateChanged += h, h => screen.ScreenStateChanged -= h);
}
