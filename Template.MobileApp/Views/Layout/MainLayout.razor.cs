namespace Template.MobileApp.Views.Layout;

using Microsoft.AspNetCore.Components.Web;
using Microsoft.FluentUI.AspNetCore.Components;

public sealed partial class MainLayout : LayoutComponentBase
{
    private const string ThemeColor = "#E91E63";

    private ErrorBoundary? errorBoundary;

    [Inject]
    public required IThemeService ThemeService { get; set; }

    [Inject]
    public required StartupState Startup { get; set; }

    [Inject]
    public required WebViewState WebView { get; set; }

    protected override Task OnInitializedAsync()
    {
        return Startup.Completed;
    }

    protected override void OnParametersSet()
    {
        errorBoundary?.Recover();
    }

    protected override Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            WebView.NotifyStarted();
            return ThemeService.SetThemeAsync(new ThemeSettings(ThemeColor, Mode: ThemeMode.Light, IsExact: true));
        }

        return Task.CompletedTask;
    }

    private void Recover()
    {
        errorBoundary?.Recover();
    }
}
