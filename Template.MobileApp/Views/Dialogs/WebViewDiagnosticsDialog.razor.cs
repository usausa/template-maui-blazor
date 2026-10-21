namespace Template.MobileApp.Views.Dialogs;

using System.Diagnostics;

using Microsoft.JSInterop;

using Template.MobileApp.Interop;

public sealed record WebViewScriptInformation(string UserAgent, bool Online, int StorageCount, long StorageSize);

public sealed partial class WebViewDiagnosticsDialog : AppDialogBase, IAsyncDisposable
{
    // Field

    private const string ModulePath = "./js/diagnostics.js";

    private const int PingCount = 10;

    private WebViewDiagnostics diagnostics = default!;

    private IJSObjectReference? module;

    private WebViewScriptInformation? script;

    private double? pingTime;

    // Property

    [Inject]
    public required IJSRuntime JsRuntime { get; set; }

    [Inject]
    public required IPlatformInterop PlatformInterop { get; set; }

    protected override void OnInitialized()
    {
        base.OnInitialized();
        diagnostics = PlatformInterop.GetWebViewDiagnostics();
    }

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
    // Operation
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

    private async Task ReloadAsync(bool clearCache)
    {
        await Dialog.CloseAsync();
        PlatformInterop.ReloadWebView(clearCache);
    }

    private async Task StopRendererAsync()
    {
        await Dialog.CloseAsync();
        PlatformInterop.StopWebViewRenderer();
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
