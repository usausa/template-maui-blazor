namespace Template.MobileApp.Views.Layout;

using Microsoft.AspNetCore.Components.Web;
using Microsoft.FluentUI.AspNetCore.Components;

public sealed partial class MainLayout : LayoutComponentBase
{
    private const string ThemeColor = "#E91E63";

    private ErrorBoundary? errorBoundary;

    //--------------------------------------------------------------------------------
    // Property
    //--------------------------------------------------------------------------------

    [Inject]
    public required IThemeService ThemeService { get; set; }

    [Inject]
    public required StartupState Startup { get; set; }

    //--------------------------------------------------------------------------------
    // Initialize
    //--------------------------------------------------------------------------------

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
            return ThemeService.SetThemeAsync(new ThemeSettings(ThemeColor, Mode: ThemeMode.Light, IsExact: true));
        }

        return Task.CompletedTask;
    }

    //--------------------------------------------------------------------------------
    // Action
    //--------------------------------------------------------------------------------

    private void Recover()
    {
        errorBoundary?.Recover();
    }
}
