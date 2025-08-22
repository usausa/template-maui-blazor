namespace Template.MobileApp;

using Microsoft.Extensions.DependencyInjection;

using Smart.Mvvm.Resolver;

using Template.MobileApp.Services;

public sealed class ApplicationInitializer : IMauiInitializeService
{
    // ReSharper disable once AsyncVoidMethod
    public async void Initialize(IServiceProvider services)
    {
        // Setup provider
        ResolveProvider.Default.Provider = services;

        var settings = services.GetRequiredService<Settings>();

        // Initial setting
        if (String.IsNullOrEmpty(settings.ApiEndPoint) && !String.IsNullOrEmpty(EmbeddedProperty.ApiEndPoint))
        {
            settings.ApiEndPoint = EmbeddedProperty.ApiEndPoint;
        }

        // Setting
        if (String.IsNullOrEmpty(settings.UniqId))
        {
            var uniqId = Guid.NewGuid();
            settings.UniqId = uniqId.ToString();
        }

        // Service
        var dataService = services.GetRequiredService<DataService>();
        await dataService.RebuildAsync();

        var apiContext = services.GetRequiredService<ApiContext>();
        if (!String.IsNullOrEmpty(settings.ApiEndPoint))
        {
            apiContext.BaseAddress = new Uri(settings.ApiEndPoint);
        }
    }
}
