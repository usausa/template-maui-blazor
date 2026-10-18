namespace Template.MobileApp.Views.Pages;

using Template.MobileApp.Services;

public sealed partial class NotificationPage : AppComponentBase
{
    // Field

    private List<NoticeEntity>? notices;

    // Property

    [Inject]
    public required DataService DataService { get; set; }

    private bool HasUnread => notices?.Exists(static x => !x.IsRead) ?? false;

    protected override async Task OnInitializedAsync()
    {
        notices = await DataService.QueryNoticeListAsync();
    }

    private async Task ExpandAsync(NoticeEntity notice, bool expanded)
    {
        if (!expanded || notice.IsRead)
        {
            return;
        }

        await DataService.UpdateNoticeReadAsync(notice.Id);
        notice.IsRead = true;
    }

    private Task ReadAllAsync()
    {
        return ExecuteAsync(async () =>
        {
            await DataService.UpdateNoticeReadAllAsync();
            notices = await DataService.QueryNoticeListAsync();
        });
    }
}
