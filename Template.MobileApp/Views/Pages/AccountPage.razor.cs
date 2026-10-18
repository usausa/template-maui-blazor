namespace Template.MobileApp.Views.Pages;

using Template.MobileApp.Services;

public sealed partial class AccountPage : AppComponentBase
{
    // Field

    private const string UserName = "見本 太郎";

    private const string UserInitials = "見";

    private const string UserId = "0000-0000";

    private const int HistoryCount = 10;

    private AccountSummary? summary;

    private List<HistoryEntity>? histories;

    // Property

    [Inject]
    public required DataService DataService { get; set; }

    protected override async Task OnInitializedAsync()
    {
        summary = await DataService.QueryAccountSummaryAsync();
        histories = await DataService.QueryHistoryListAsync(HistoryCount);
    }
}
