namespace Template.MobileApp;

using System.Text.Encodings.Web;
using System.Text.Json.Serialization;
using System.Text.Unicode;

using BarcodeScanning;

using CommunityToolkit.Maui;

using Fonts;

using MauiComponents.Resolver;

using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.Maui.LifecycleEvents;

using Rester;

using Shiny;

using Smart.Data.Mapper;
using Smart.Resolver;

using Template.MobileApp.Behaviors;
using Template.MobileApp.Components;
using Template.MobileApp.Helpers;
using Template.MobileApp.Helpers.Data;
using Template.MobileApp.Interop;
using Template.MobileApp.Services;
using Template.MobileApp.Shell;
using Template.MobileApp.Usecase;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp() =>
        MauiApp.CreateBuilder()
            .UseMauiApp<App>()
            .ConfigureFonts(ConfigureFonts)
            .ConfigureLifecycleEvents(ConfigureLifecycle)
            .ConfigureEssentials(ConfigureEssentials)
            .ConfigureLogging()
            .ConfigureGlobalSettings()
            .UseBlazor()
            .UseMauiCommunityToolkit()
            .UseMauiCommunityToolkitCamera()
            .UseBarcodeScanning()
            .UseShiny()
            .UseMauiServices()
            .UseMauiComponents()
            .UseCommunityToolkitServices()
            .UseCustomView()
            .ConfigureHttpClient()
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
    private static void ConfigureLifecycle(ILifecycleBuilder effects)
    {
    }
    // ReSharper restore UnusedParameter.Local

    // ReSharper disable UnusedParameter.Local
    private static void ConfigureEssentials(IEssentialsBuilder config)
    {
    }
    // ReSharper restore UnusedParameter.Local

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

        // Busy
        builder.UseCustomBusyOverlay();

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
    // Components
    // ------------------------------------------------------------

    private static MauiAppBuilder ConfigureComponents(this MauiAppBuilder builder)
    {
        // Components
        builder.Services.AddBluetoothLE();
        builder.Services.AddBluetoothLeHosting();

        return builder;
    }

    private static MauiAppBuilder ConfigureContainer(this MauiAppBuilder builder)
    {
        builder.ConfigureContainer(new SmartServiceProviderFactory(), ConfigureContainer);

        return builder;
    }

    private static void ConfigureContainer(ResolverConfig config)
    {
        config
            .UseAutoBinding()
            .UseArrayBinding()
            .UseAssignableBinding()
            .UsePropertyInjector();

        // MauiComponents
        config.AddComponentsDialog(static c =>
        {
            ConfigureDialogDesign(c);
            c.EnablePromptEnterAction = true;
            c.EnablePromptSelectAll = true;
        });
        //config.AddComponentsPopup(static c => c.AutoRegister(DialogSource()));
        config.AddComponentsSerializer();
        config.AddComponentsScreen();
        config.AddComponentsLocation();
        config.AddComponentsSpeech();

        // Messenger
        config.BindSingleton<IReactiveMessenger>(ReactiveMessenger.Default);

        // Components
        config.BindSingleton<IStorageManager, StorageManager>();

        // State
        config.BindSingleton<DeviceState>();
        config.BindSingleton<Session>();
        config.BindSingleton<Settings>();

        // Service
        config.BindSingleton(static p =>
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
        config.BindSingleton<DataService>();

        config.BindSingleton<NetworkService>();

        // Usecase
        config.BindSingleton<NetworkOperator>();

        config.BindSingleton<NetworkUsecase>();

        // Interop
        config.BindSingleton<IPlatformInterop, PlatformInterop>();

        // Startup
        config.BindSingleton<IMauiInitializeService, ApplicationInitializer>();
    }

    // ------------------------------------------------------------
    // View & Dialog
    // ------------------------------------------------------------

    //[PopupSource]
    //public static partial IEnumerable<KeyValuePair<DialogId, Type>> DialogSource();
}
