namespace Template.MobileApp;

using Microsoft.Data.Sqlite;
using Microsoft.Extensions.DependencyInjection;

using Template.MobileApp.Diagnostics;
using Template.MobileApp.Services;

#pragma warning disable CA1724
public sealed partial class App
{
    private readonly IServiceProvider serviceProvider;

    private readonly ILogger<App> log;

    public App(IServiceProvider serviceProvider, ILogger<App> log)
    {
        this.serviceProvider = serviceProvider;
        this.log = log;

        InitializeComponent();
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        return new Window(serviceProvider.GetRequiredService<MainPage>());
    }

    // ReSharper disable once AsyncVoidMethod
    protected override async void OnStart()
    {
        // Report previous exception
        await CrashReport.ShowReport();

        // Initialize database
        var initializeError = await InitializeDataAsync();
        if (initializeError is not null)
        {
            var page = Current?.Windows[0].Page;
            if (page is not null)
            {
                await page.DisplayAlertAsync("Initialize error", $"Failed to initialize database.\r\n{initializeError.Message}", "Exit");
            }

            Current?.Quit();
            return;
        }

        // Start
        log.InfoApplicationStart(typeof(App).Assembly.GetName().Version, Environment.Version);

        // Permissions
        await Permissions.RequestCameraAsync();
        await Permissions.RequestLocationAsync();
    }

    private async Task<Exception?> InitializeDataAsync()
    {
        try
        {
            var dataService = serviceProvider.GetRequiredService<DataService>();
            await dataService.RebuildAsync();

            // Dummy data
            await PrepareDummyDataAsync(dataService);

            return null;
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException or SqliteException)
        {
            log.ErrorDatabaseInitializeFailed(ex);
            return ex;
        }
    }

    private static async ValueTask PrepareDummyDataAsync(DataService dataService)
    {
        await dataService.InsertWorkEnumerableAsync(
        [
            new WorkEntity { Id = 1, Name = "Sample-1" },
            new WorkEntity { Id = 2, Name = "Sample-2" },
            new WorkEntity { Id = 3, Name = "Sample-3" },
            new WorkEntity { Id = 4, Name = "Sample-4" }
        ]);
    }
}
#pragma warning restore CA1724
