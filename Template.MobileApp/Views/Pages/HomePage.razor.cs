namespace Template.MobileApp.Views.Pages;

using Microsoft.FluentUI.AspNetCore.Components;

using Template.MobileApp.Interop;
using Template.MobileApp.Services;

using Icons = Microsoft.FluentUI.AspNetCore.Components.Icons;

public sealed partial class HomePage : AppComponentBase
{
    // Field

    private const int ChargeAmount = 1000;

    private const int PhotoMaxSize = 640;

    private const int ReminderSeconds = 10;

    private const int RecentCount = 5;

    private const string SampleText = "見本です";

    private const string SampleNoticeTitle = "通知の見本です";

    private const string SampleNoticeBody = "ホームの「通知」から足したお知らせです。";

    private static readonly CampaignBanner[] Banners =
    [
        new("🎁", "今月のポイント還元", "対象のお店で最大 10% が戻ります"),
        new("☕", "カフェ ことりが加わりました", "3% のポイントが戻ります"),
        new("💊", "ことぶき薬局のクーポン", "100 円引きで使えます")
    ];

    private readonly HomeAction[] walletActions;

    private readonly HomeAction[] nativeActions;

    private readonly HomeAction[] sampleActions;

    private readonly List<RecentActivity> recentActivities = [];

    private AccountSummary? summary;

    // Property

    [Inject]
    public required IPlatformInterop PlatformInterop { get; set; }

    [Inject]
    public required DataService DataService { get; set; }

    //--------------------------------------------------------------------------------
    // Constructor
    //--------------------------------------------------------------------------------

    public HomePage()
    {
        walletActions =
        [
            new("支払う", new Icons.Regular.Size24.QrCode(), PayAsync),
            new("読み取る", new Icons.Regular.Size24.ScanQrCode(), ScanAsync),
            new("チャージ", new Icons.Regular.Size24.AddCircle(), ChargeAsync),
            new("送る", new Icons.Regular.Size24.Send(), ShowSampleAsync, true)
        ];
        nativeActions =
        [
            new("QR 読み取り", new Icons.Regular.Size24.ScanCamera(), ScanAsync),
            new("カメラ", new Icons.Regular.Size24.Camera(), CaptureAsync),
            new("通知", new Icons.Regular.Size24.Alert(), NotifyAsync),
            new("リマインダー", new Icons.Regular.Size24.ClockAlarm(), RemindAsync)
        ];
        sampleActions =
        [
            new("クーポン", new Icons.Regular.Size24.TicketDiagonal(), ShowSampleAsync),
            new("送金", new Icons.Regular.Size24.MoneyHand(), ShowSampleAsync),
            new("ショップ", new Icons.Regular.Size24.BuildingShop(), ShowSampleAsync),
            new("もっと見る", new Icons.Regular.Size24.Apps(), ShowSampleAsync)
        ];
    }

    protected override async Task OnInitializedAsync()
    {
        summary = await DataService.QueryAccountSummaryAsync();
    }

    //--------------------------------------------------------------------------------
    // Operation
    //--------------------------------------------------------------------------------

    private Task PayAsync() =>
        ExecuteAsync(async () => await PlatformInterop.DisplayBarcodeAsync());

    private Task ScanAsync() =>
        ExecuteAsync(async () =>
        {
            var result = await PlatformInterop.ScanBarcodeAsync();
            if (result is not null)
            {
                AddRecent(new Icons.Regular.Size24.ScanQrCode(), result.Value, result.Format);
            }
        });

    private Task CaptureAsync() =>
        ExecuteAsync(async () =>
        {
            var image = await PlatformInterop.CapturePhotoAsync(PhotoMaxSize);
            if (image is not null)
            {
                AddRecent(new Icons.Regular.Size24.Camera(), "撮った写真", imageUrl: $"data:image/jpeg;base64,{Convert.ToBase64String(image)}");
            }
        });

    private Task ChargeAsync() =>
        ExecuteAsync(async () =>
        {
            var now = DateTime.Now;
            var title = $"{ViewHelper.FormatYen(ChargeAmount)}をチャージしました";
            await DataService.InsertHistoryAsync(new HistoryEntity { Kind = HistoryKind.Charge, Title = "チャージ", Amount = ChargeAmount, OccurredAt = now });
            await DataService.InsertNoticeAsync(new NoticeEntity { Category = NoticeCategory.Payment, Title = title, Body = "チャージが完了しました。残高はアカウントの画面で確かめられます。", PublishedAt = now });
            summary = await DataService.QueryAccountSummaryAsync();
            AddRecent(new Icons.Regular.Size24.AddCircle(), title);
            StateHasChanged();

            await PlatformInterop.ShowNotificationAsync(title, $"残高は {ViewHelper.FormatYen(summary.Balance)}です。");
        });

    private Task NotifyAsync() =>
        ExecuteAsync(async () =>
        {
            await DataService.InsertNoticeAsync(new NoticeEntity { Category = NoticeCategory.Information, Title = SampleNoticeTitle, Body = SampleNoticeBody, PublishedAt = DateTime.Now });
            AddRecent(new Icons.Regular.Size24.Alert(), SampleNoticeTitle, "お知らせに足しました");
            StateHasChanged();

            await PlatformInterop.ShowNotificationAsync(SampleNoticeTitle, SampleNoticeBody);
        });

    private Task RemindAsync() =>
        ExecuteAsync(async () =>
        {
            if (await PlatformInterop.ScheduleNotificationAsync("リマインダー", $"{ReminderSeconds} 秒前に予約した通知です。", TimeSpan.FromSeconds(ReminderSeconds)))
            {
                AddRecent(new Icons.Regular.Size24.ClockAlarm(), $"{ReminderSeconds} 秒後の通知を予約しました");
            }
        });

    private Task ShowSampleAsync() =>
        ExecuteAsync(async () => await PlatformInterop.ShowToastAsync(SampleText));

    //--------------------------------------------------------------------------------
    // Helper
    //--------------------------------------------------------------------------------

    private void AddRecent(Icon icon, string text, string? subText = null, string? imageUrl = null)
    {
        recentActivities.Insert(0, new RecentActivity(icon, text, subText, imageUrl, DateTime.Now));
        if (recentActivities.Count > RecentCount)
        {
            recentActivities.RemoveAt(RecentCount);
        }
    }

    //--------------------------------------------------------------------------------
    // Model
    //--------------------------------------------------------------------------------

    private sealed record HomeAction(string Name, Icon Icon, Func<Task> Execute, bool IsSample = false);

    private sealed record CampaignBanner(string Emoji, string Title, string Text);

    private sealed record RecentActivity(Icon Icon, string Text, string? SubText, string? ImageUrl, DateTime OccurredAt);
}
