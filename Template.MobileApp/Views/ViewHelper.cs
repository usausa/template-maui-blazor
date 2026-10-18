namespace Template.MobileApp.Views;

using Microsoft.FluentUI.AspNetCore.Components;

using Color = Microsoft.FluentUI.AspNetCore.Components.Color;

public static class ViewHelper
{
    public const string SubTextColor = StylesVariables.Colors.Neutral.Foreground3;

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
}
