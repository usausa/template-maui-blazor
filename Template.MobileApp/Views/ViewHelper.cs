namespace Template.MobileApp.Views;

using Microsoft.FluentUI.AspNetCore.Components;

using Color = Microsoft.FluentUI.AspNetCore.Components.Color;

public static class ViewHelper
{
    public const string SubTextColor = StylesVariables.Colors.Neutral.Foreground3;

    public const string OnBrandTextColor = StylesVariables.Colors.Neutral.ForegroundOnBrand;

    public const string BrandIconColor = StylesVariables.Colors.Brand.Foreground1;

    public const string TileBackgroundColor = StylesVariables.Colors.Brand.Background2;

    public const string SampleTileBackgroundColor = StylesVariables.Colors.Neutral.Background3;

    private const string WalletActionBackground = StylesVariables.Colors.Neutral.Background1;

    private const string WalletSampleBackground = "color-mix(in srgb, var(--colorNeutralForegroundOnBrand) 25%, transparent)";

    //--------------------------------------------------------------------------------
    // Format
    //--------------------------------------------------------------------------------

    public static string FormatYen(long value) =>
        $"{value.ToString("N0", CultureInfo.InvariantCulture)} 円";

    public static string FormatPoint(long value) =>
        $"{value.ToString("N0", CultureInfo.InvariantCulture)} pt";

    public static string FormatDate(DateTime value) =>
        value.ToLocalTime().ToString("MM/dd", CultureInfo.InvariantCulture);

    public static string FormatDateTime(DateTime value) =>
        value.ToLocalTime().ToString("MM/dd HH:mm", CultureInfo.InvariantCulture);

    //--------------------------------------------------------------------------------
    // Select
    //--------------------------------------------------------------------------------

    public static ButtonAppearance SelectAppearance(bool selected) =>
        selected ? ButtonAppearance.Primary : ButtonAppearance.Outline;

    //--------------------------------------------------------------------------------
    // Home
    //--------------------------------------------------------------------------------

    public static string WalletActionBackgroundColor(bool sample) =>
        sample ? WalletSampleBackground : WalletActionBackground;

    public static string WalletActionIconColor(bool sample) =>
        sample ? OnBrandTextColor : BrandIconColor;

    //--------------------------------------------------------------------------------
    // Store
    //--------------------------------------------------------------------------------

    public static string StoreCategoryText(StoreCategory category) => category switch
    {
        StoreCategory.Restaurant => "飲食",
        StoreCategory.Convenience => "コンビニ",
        StoreCategory.Drugstore => "ドラッグストア",
        StoreCategory.Supermarket => "スーパー",
        _ => "その他"
    };

    public static string StoreCategoryEmoji(StoreCategory category) => category switch
    {
        StoreCategory.Restaurant => "🍜",
        StoreCategory.Convenience => "🏪",
        StoreCategory.Drugstore => "💊",
        StoreCategory.Supermarket => "🛒",
        _ => "🏬"
    };

    public static string FormatPointRate(int rate) =>
        $"{rate}% 還元";

    //--------------------------------------------------------------------------------
    // Notice
    //--------------------------------------------------------------------------------

    public static string NoticeCategoryText(NoticeCategory category) => category switch
    {
        NoticeCategory.Campaign => "キャンペーン",
        NoticeCategory.Payment => "お支払い",
        _ => "お知らせ"
    };

    public static BadgeColor NoticeCategoryColor(NoticeCategory category) =>
        category == NoticeCategory.Information ? BadgeColor.Informative : BadgeColor.Brand;

    public static BadgeAppearance NoticeCategoryAppearance(NoticeCategory category) =>
        category == NoticeCategory.Campaign ? BadgeAppearance.Filled : BadgeAppearance.Tint;

    public static TextWeight NoticeTitleWeight(bool isRead) =>
        isRead ? TextWeight.Regular : TextWeight.Semibold;

    //--------------------------------------------------------------------------------
    // History
    //--------------------------------------------------------------------------------

    public static string FormatHistoryAmount(HistoryEntity history) => history.Kind switch
    {
        HistoryKind.Payment => $"-{FormatYen(history.Amount)}",
        HistoryKind.Charge => $"+{FormatYen(history.Amount)}",
        _ => $"+{FormatPoint(history.Amount)}"
    };

    public static Color HistoryAmountColor(HistoryKind kind) =>
        kind == HistoryKind.Payment ? Color.Default : Color.Primary;

    //--------------------------------------------------------------------------------
    // Diagnostics
    //--------------------------------------------------------------------------------

    public static string FormatMilliseconds(double? value) =>
        value is null ? "-" : $"{value.Value.ToString("N1", CultureInfo.InvariantCulture)} ms";

    public static string FormatTimes(int value) =>
        $"{value} 回";

    public static string FormatStorage(int count, long size) =>
        $"{count} 件・{(size / 1024d).ToString("N1", CultureInfo.InvariantCulture)} KB";

    public static string NetworkAccessText(NetworkAccess access) => access switch
    {
        NetworkAccess.Internet => "インターネット",
        NetworkAccess.ConstrainedInternet => "制限付きのインターネット",
        NetworkAccess.Local => "ローカルだけ",
        NetworkAccess.None => "接続なし",
        _ => "不明"
    };

    public static string OnlineText(bool online) =>
        online ? "オンライン" : "オフライン";
}
