namespace Template.MobileApp;

internal static partial class Log
{
    // Startup

    [LoggerMessage(Level = LogLevel.Information, Message = "Application start. version=[{version}], runtime=[{runtime}]")]
    public static partial void InfoApplicationStart(this ILogger logger, Version? version, Version runtime);

    [LoggerMessage(Level = LogLevel.Error, Message = "Database initialize failed.")]
    public static partial void ErrorDatabaseInitializeFailed(this ILogger logger, Exception exception);

    // State

    [LoggerMessage(Level = LogLevel.Debug, Message = "Screen state changed. state=[{on}]")]
    public static partial void DebugScreenStateChanged(this ILogger logger, bool on);

    [LoggerMessage(Level = LogLevel.Debug, Message = "Battery info changed. level=[{chargeLevel}], state=[{state}], source=[{powerSource}]")]
    public static partial void DebugBatteryState(this ILogger logger, double chargeLevel, Microsoft.Maui.Devices.BatteryState state, BatteryPowerSource powerSource);

    [LoggerMessage(Level = LogLevel.Debug, Message = "Connectivity changed. profile=[{profile}], access=[{access}]")]
    public static partial void DebugConnectivityState(this ILogger logger, NetworkProfile profile, NetworkAccess access);

    // WebView

    [LoggerMessage(Level = LogLevel.Information, Message = "WebView creating. package=[{package}], version=[{version}], count=[{count}]")]
    public static partial void InfoWebViewCreating(this ILogger logger, string? package, string? version, int count);

    [LoggerMessage(Level = LogLevel.Information, Message = "WebView started. elapsed=[{elapsed}]")]
    public static partial void InfoWebViewStarted(this ILogger logger, long elapsed);

    [LoggerMessage(Level = LogLevel.Error, Message = "WebView failed. failure=[{failure}], detail=[{detail}]")]
    public static partial void ErrorWebViewFailed(this ILogger logger, BlazorWebViewStatus failure, string? detail);

    // View

    [LoggerMessage(Level = LogLevel.Error, Message = "Unhandled component exception.")]
    public static partial void ErrorComponentException(this ILogger logger, Exception exception);
}
