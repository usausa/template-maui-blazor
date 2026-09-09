namespace Template.MobileApp.Markup;

using Fonts;

using Microsoft.Extensions.DependencyInjection;

public static class AppIcons
{
    private const double SelectSize = 36d;

    //--------------------------------------------------------------------------------
    // Center (Material / 36 / White)
    //--------------------------------------------------------------------------------

    public static readonly FontImageSource Pay = Create(MaterialIcons.Qr_code, SelectSize, Colors.White);

    //--------------------------------------------------------------------------------
    // Warmup
    //--------------------------------------------------------------------------------

    public static void WarmTypefaces(IServiceProvider provider)
    {
        var fontManager = provider.GetRequiredService<IFontManager>();
        fontManager.GetTypeface(Microsoft.Maui.Font.OfSize(MaterialIcons.FontFamily, SelectSize));
    }

    public static ValueTask WarmStartupAsync(IServiceProvider provider) => WarmAsync(provider, EnumerateStartup());

    private static IEnumerable<FontImageSource> EnumerateStartup()
    {
        yield return Pay;

        var selectedColor = ResourceColor("PinkAccent3");
        var unselectedColor = ResourceColor("GrayDefault");
        string[] glyphs = [MaterialIcons.Home, MaterialIcons.Search, MaterialIcons.Notifications_none, MaterialIcons.Account_circle];
        foreach (var glyph in glyphs)
        {
            yield return Create(glyph, SelectSize, selectedColor);
            yield return Create(glyph, SelectSize, unselectedColor);
        }
    }

    private static async ValueTask WarmAsync(IServiceProvider provider, IEnumerable<FontImageSource> sources)
    {
#if ANDROID
        var imageSourceServiceProvider = provider.GetService<IImageSourceServiceProvider>();
        if (imageSourceServiceProvider is null)
        {
            return;
        }

        var context = Android.App.Application.Context;
        var pending = new List<Task>();
        foreach (var source in sources)
        {
            var service = imageSourceServiceProvider.GetRequiredImageSourceService(source);
            pending.Add(service.GetDrawableAsync(source, context));
        }

        await Task.WhenAll(pending).ConfigureAwait(true);
#endif
    }

    //--------------------------------------------------------------------------------
    // Helper
    //--------------------------------------------------------------------------------

    private static FontImageSource Create(string glyph, double size, Color color) => new()
    {
        FontFamily = MaterialIcons.FontFamily,
        Glyph = glyph,
        Size = size,
        Color = color
    };

    private static Color ResourceColor(string key)
    {
        var resources = Application.Current?.Resources;
        if ((resources is not null) && resources.TryGetValue(key, out var value) && (value is Color color))
        {
            return color;
        }

        return Colors.White;
    }
}
