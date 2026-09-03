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

#if DEBUG
        if (services is BunnyTail.DependencyInjection.GeneratedServiceProvider generatedProvider)
        {
            foreach (var line in BunnyTail.DependencyInjection.Diagnostics.ServiceFactoryReportExtensions.DescribeRuntimeFallbacks(generatedProvider).Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries))
            {
                System.Diagnostics.Debug.WriteLine(line);
            }
        }
#endif
        var settings = services.GetRequiredService<Settings>();

        // Initial setting
        if (String.IsNullOrEmpty(settings.ApiEndPoint) && !String.IsNullOrEmpty(EmbeddedProperty.ApiEndPoint))
        {
            settings.ApiEndPoint = EmbeddedProperty.ApiEndPoint;
        }

        // Setting
        if (String.IsNullOrEmpty(settings.UniqueId))
        {
            var uniqueId = Guid.NewGuid();
            settings.UniqueId = uniqueId.ToString();
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
