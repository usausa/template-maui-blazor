namespace Template.MobileApp.Views.Dialogs;

using System.Diagnostics;

using Microsoft.JSInterop;

public sealed record WebViewScriptInformation(string UserAgent, bool Online, int StorageCount, long StorageSize);

public sealed partial class WebViewDiagnosticsDialog : AppDialogBase, IAsyncDisposable
{
    private const string ModulePath = "./js/diagnostics.js";

    private const int PingCount = 10;

    private IJSObjectReference? module;

    private WebViewScriptInformation? script;

    private double? pingTime;

    //--------------------------------------------------------------------------------
    // Property
    //--------------------------------------------------------------------------------

    [Inject]
    public required IJSRuntime JsRuntime { get; set; }

    //--------------------------------------------------------------------------------
    // Initialize
    //--------------------------------------------------------------------------------

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            module = await JsRuntime.InvokeAsync<IJSObjectReference>("import", ModulePath);
            script = await module.InvokeAsync<WebViewScriptInformation>("getInformation");
            pingTime = await MeasurePingAsync(module);
            StateHasChanged();
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (module is not null)
        {
            try
            {
                await module.DisposeAsync();
            }
            catch (JSDisconnectedException)
            {
                // Ignore
            }
        }

        Dispose();
    }

    //--------------------------------------------------------------------------------
    // Action
    //--------------------------------------------------------------------------------

    private Task MeasureAsync()
    {
        return ExecuteAsync(async () =>
        {
            if (module is not null)
            {
                pingTime = await MeasurePingAsync(module);
            }
        });
    }

    //--------------------------------------------------------------------------------
    // Helper
    //--------------------------------------------------------------------------------

    private static async Task<double> MeasurePingAsync(IJSObjectReference module)
    {
        var start = Stopwatch.GetTimestamp();
        for (var i = 0; i < PingCount; i++)
        {
            await module.InvokeVoidAsync("ping");
        }

        return Stopwatch.GetElapsedTime(start).TotalMilliseconds / PingCount;
    }
}
