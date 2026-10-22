namespace Template.MobileApp.Views.Pages;

using Microsoft.FluentUI.AspNetCore.Components;

using Template.MobileApp.Services;
using Template.MobileApp.Views.Dialogs;

public sealed partial class AccountPage : AppComponentBase
{
    private const string UserName = "見本 太郎";

    private const string UserInitials = "見";

    private const string UserId = "0000-0000";

    private const int HistoryCount = 10;

    private const string DiagnosticsTitle = "WebView の診断";

    private AccountSummary? summary;

    private List<HistoryEntity>? histories;

    //--------------------------------------------------------------------------------
    // Property
    //--------------------------------------------------------------------------------

    [Inject]
    public required DataService DataService { get; set; }

    [Inject]
    public required IDialogService DialogService { get; set; }

    // ReSharper disable once UnassignedGetOnlyAutoProperty
    private bool DiagnosticsEnabled { get; }

    //--------------------------------------------------------------------------------
    // Initialize
    //--------------------------------------------------------------------------------

    public AccountPage()
    {
#if DEBUG
        DiagnosticsEnabled = true;
#endif
    }

    protected override async Task OnInitializedAsync()
    {
        summary = await DataService.QueryAccountSummaryAsync();
        histories = await DataService.QueryHistoryListAsync(HistoryCount);
    }

    //--------------------------------------------------------------------------------
    // Action
    //--------------------------------------------------------------------------------

    private async Task ShowDiagnosticsAsync()
    {
        await DialogService.ShowDialogAsync<WebViewDiagnosticsDialog>(static options =>
        {
            options.Header.Title = DiagnosticsTitle;
            options.Header.CloseAction.Visible = true;
        });
    }
}
