namespace Template.MobileApp;

using System.Net.Http.Headers;
using System.Text.Encodings.Web;
using System.Text.Json.Serialization;
using System.Text.Unicode;

using BarcodeScanning;

using BunnyTail.DependencyInjection;

using CommunityToolkit.Maui;

using Fonts;

using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.Maui.LifecycleEvents;

using Rester;

using Shiny;

using SkiaSharp.Views.Maui.Controls.Hosting;

using Smart.Data.Mapper;

using Syncfusion.Maui.Toolkit.Hosting;

using Template.MobileApp.Behaviors;
using Template.MobileApp.Components;
using Template.MobileApp.Helpers;
using Template.MobileApp.Helpers.Data;
using Template.MobileApp.Interop;
using Template.MobileApp.Interop.Dialogs;
using Template.MobileApp.Services;
using Template.MobileApp.Usecase;

public static partial class MauiProgram
{
    private const string DialogsNamespace = "Template.MobileApp.Interop.Dialogs";

    public static MauiApp CreateMauiApp() =>
        MauiApp.CreateBuilder()
            .UseMauiApp<App>()
            .ConfigureFonts(ConfigureFonts)
            .ConfigureLifecycleEvents(ConfigureLifecycleEvents)
            .ConfigureEssentials(ConfigureEssentials)
            .ConfigureLogging()
            .ConfigureGlobalSettings()
            .ConfigureSyncfusionToolkit()
            .UseBlazor()
            .UseSkiaSharp()
            .UseMauiCommunityToolkit(ConfigureMauiCommunityToolkit)
            .UseMauiCommunityToolkitCamera()
            .UseBarcodeScanning()
            .UseShiny()
            .UseMauiServices()
            .UseMauiComponents()
            .UseCommunityToolkitServices()
            .UseCustomView()
            .ConfigureContainer()
            .Build();

    // ------------------------------------------------------------
    // Blazor
    // ------------------------------------------------------------

    private static MauiAppBuilder UseBlazor(this MauiAppBuilder builder)
    {
        builder.Services.AddMauiBlazorWebView();
        builder.Services.AddFluentUIComponents();
        return builder;
    }

    // ------------------------------------------------------------
    // Logging
    // ------------------------------------------------------------

    private static MauiAppBuilder ConfigureLogging(this MauiAppBuilder builder)
    {
        // Debug
#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
        builder.Logging.AddDebug();
#endif

        // Android
#if ANDROID
        builder.Logging.AddAndroidLogger(static options => options.ShortCategory = true);
#endif
        // File
        builder.Logging.AddFileLogger(static options =>
            {
#if ANDROID
                options.Directory = Path.Combine(AndroidHelper.GetExternalFilesDir(), "log");
#endif
                options.RetainDays = 7;
            })
            .AddFilter(typeof(MauiProgram).Namespace, LogLevel.Debug);

        return builder;
    }

    // ------------------------------------------------------------
    // Application
    // ------------------------------------------------------------

    // ReSharper disable UnusedParameter.Local
    private static void ConfigureLifecycleEvents(ILifecycleBuilder effects)
    {
    }
    // ReSharper restore UnusedParameter.Local

    // ReSharper disable UnusedParameter.Local
    private static void ConfigureEssentials(IEssentialsBuilder config)
    {
    }
    // ReSharper restore UnusedParameter.Local

    private static void ConfigureMauiCommunityToolkit(Options options)
    {
        options.SetPopupDefaults(new DefaultPopupSettings
        {
            CanBeDismissedByTappingOutsideOfPopup = true,
            Padding = 0
        });
        options.SetPopupOptionsDefaults(new DefaultPopupOptionsSettings
        {
            CanBeDismissedByTappingOutsideOfPopup = true,
            Shadow = null,
            Shape = null
        });
    }

    private static MauiAppBuilder ConfigureGlobalSettings(this MauiAppBuilder builder)
    {
        // Config DataMapper
        SqlMapperConfig.Default.ConfigureTypeHandlers(static config =>
        {
            config[typeof(DateTime)] = new DateTimeTypeHandler();
            config[typeof(Guid)] = new GuidTypeHandler();
        });

        // Config Rest
        RestConfig.Default.UseJsonSerializer(static config =>
        {
            config.Converters.Add(new Template.MobileApp.Helpers.Json.DateTimeConverter());
            config.Encoder = JavaScriptEncoder.Create(UnicodeRanges.All);
            config.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        });

        // Crash dump
        CrashReport.Start();

        return builder;
    }

    private static MauiAppBuilder UseCustomView(this MauiAppBuilder builder)
    {
        // Behaviors
        builder.ConfigureCustomBehaviors();

        return builder;
    }

    // ------------------------------------------------------------
    // Design
    // ------------------------------------------------------------

    private static void ConfigureFonts(IFontCollection fonts)
    {
        fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
        fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
        fonts.AddFont("FluentSystemIcons-Regular.ttf", FluentUI.FontFamily);
        fonts.AddFont("MaterialIcons-Regular.ttf", MaterialIcons.FontFamily);
        fonts.AddFont("851Gkktt_005.ttf", "Gkktt");
        fonts.AddFont("DSEG7Classic-Regular.ttf", "DSEG7");
    }

    private static void ConfigureDialogDesign(DialogConfig config)
    {
        var resources = Application.Current!.Resources;
        config.IndicatorColor = resources.FindResource<Microsoft.Maui.Graphics.Color>("BlueAccent2");
        config.LoadingMessageFontSize = 28;
        config.ProgressCircleColor1 = resources.FindResource<Microsoft.Maui.Graphics.Color>("BlueAccent2");
        config.ProgressCircleColor2 = resources.FindResource<Microsoft.Maui.Graphics.Color>("GrayLighten2");

        // Avoiding conflicts with progress
        config.LockBackgroundColor = Colors.Transparent;
        config.LoadingBackgroundColor = Colors.Transparent;
        config.ProgressBackgroundColor = Colors.Transparent;
    }

    // ------------------------------------------------------------
    // Container
    // ------------------------------------------------------------

    private static MauiAppBuilder ConfigureContainer(this MauiAppBuilder builder)
    {
        builder.ConfigureContainer(
            new GeneratedServiceProviderFactory(static options => options.TrackTransientDisposables = false),
            ConfigureContainer);
        return builder;
    }

    private static void ConfigureContainer(IServiceCollection services)
    {
        // View & ViewModel
        services.AddTransient<MainPage>();
        services.AddTransient<MainPageViewModel>();
        services.AddDialogViews();
        services.AddDialogViewModels();

        // MauiComponents
        services.AddComponentsDialog(static c =>
        {
            ConfigureDialogDesign(c);
            c.EnablePromptEnterAction = true;
            c.EnablePromptSelectAll = true;
        });
        services.AddComponentsPopup(static c => c.AutoRegister(DialogSource()));
        services.AddComponentsScreen();
        services.AddComponentsLocation();
        services.AddComponentsSpeech();
        services.AddCommunication();

        // Messenger
        services.AddSingleton<IReactiveMessenger>(ReactiveMessenger.Default);

        // Components
        services.AddSingleton<IStorageManager, StorageManager>();

        // Bluetooth
        services.AddBluetoothLE();
        services.AddBluetoothLeHosting();

        // Resource
        services.AddSingleton<ResourceDictionary>(static _ => Application.Current!.Resources);

        // State
        services.AddSingleton<IBusyState>(BusyState.Default);
        services.AddSingleton<DeviceState>();
        services.AddSingleton<Session>();
        services.AddSingleton<Settings>();

        // HttpClient
        services
            .AddHttpClient(ApiNames.Default, SetupHttpClient)
            .ConfigurePrimaryHttpMessageHandler(CreateHttpMessageHandler)
            .AddHttpMessageHandler<ApiDelegatingHandler>();
        services.AddTransient<ApiDelegatingHandler>();
        services.AddSingleton<ApiContext>();

        // Service
        services.AddSingleton(static p =>
        {
            var storage = p.GetRequiredService<IStorageManager>();
            return new DataServiceOptions
            {
#if DEBUG
                Path = Path.Combine(storage.PublicFolder, "data.db")
#else
                Path = Path.Combine(storage.PrivateFolder, "data.db")
#endif
            };
        });
        services.AddSingleton<DataService>();

        services.AddSingleton<HttpService>();

        // Usecase
        services.AddSingleton<NetworkOperator>();
        services.AddSingleton<NetworkUsecase>();

        // Interop
        services.AddSingleton<IPlatformInterop, PlatformInterop>();

        // Startup
        services.AddSingleton<IMauiInitializeService, ApplicationInitializer>();
    }

    // ------------------------------------------------------------
    // Network
    // ------------------------------------------------------------

    private static void SetupHttpClient(IServiceProvider provider, HttpClient client)
    {
        client.BaseAddress = provider.GetRequiredService<ApiContext>().BaseAddress;
        client.Timeout = TimeSpan.FromSeconds(30);
        client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
        client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("deflate"));
    }

    private static HttpMessageHandler CreateHttpMessageHandler()
    {
        var handler = new SocketsHttpHandler
        {
            AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
            PooledConnectionLifetime = TimeSpan.FromMinutes(1)
        };
//#pragma warning disable CA5359
//      handler.SslOptions.RemoteCertificateValidationCallback = static (_, _, _, _) => true;
//#pragma warning restore CA5359
        return handler;
    }

    // ------------------------------------------------------------
    // View & ViewModel
    // ------------------------------------------------------------

    // ReSharper disable UnusedMethodReturnValue.Local
    [ComponentRegistration(Lifetime.Transient, "View$", Namespace = DialogsNamespace)]
    private static partial IServiceCollection AddDialogViews(this IServiceCollection services);

    [ComponentRegistration(Lifetime.Transient, "ViewModel$", Namespace = DialogsNamespace)]
    private static partial IServiceCollection AddDialogViewModels(this IServiceCollection services);
    // ReSharper restore UnusedMethodReturnValue.Local

    // ------------------------------------------------------------
    // View & Dialog
    // ------------------------------------------------------------

    [PopupSource]
    public static partial IEnumerable<KeyValuePair<DialogId, Type>> DialogSource();
}
