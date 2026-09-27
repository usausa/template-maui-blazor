namespace Template.MobileApp.Markup;

using Fonts;

using Template.MobileApp.Views;

public static class AppIcons
{
    private const double SelectSize = 36d;

    //--------------------------------------------------------------------------------
    // Center (Material / 36 / White)
    //--------------------------------------------------------------------------------

    public static readonly FontImageSource Pay = Create(MaterialIcons.Qr_code, SelectSize, Colors.White);

    //--------------------------------------------------------------------------------
    // Bottom navigation (Material / 36)
    //--------------------------------------------------------------------------------

    public static readonly FontImageSource Home = Create(MaterialIcons.Home, SelectSize, ResourceColor("PinkAccent3"));

    public static readonly FontImageSource Search = Create(MaterialIcons.Search, SelectSize, ResourceColor("GrayDefault"));

    public static readonly FontImageSource Notifications = Create(MaterialIcons.Notifications_none, SelectSize, ResourceColor("GrayDefault"));

    public static readonly FontImageSource Account = Create(MaterialIcons.Account_circle, SelectSize, ResourceColor("GrayDefault"));

    public static void SetNavigationSelection(SelectPage selected)
    {
        var selectedColor = ResourceColor("PinkAccent3");
        var unselectedColor = ResourceColor("GrayDefault");
        Home.Color = selected == SelectPage.Home ? selectedColor : unselectedColor;
        Search.Color = selected == SelectPage.Search ? selectedColor : unselectedColor;
        Notifications.Color = selected == SelectPage.Notifications ? selectedColor : unselectedColor;
        Account.Color = selected == SelectPage.Account ? selectedColor : unselectedColor;
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
