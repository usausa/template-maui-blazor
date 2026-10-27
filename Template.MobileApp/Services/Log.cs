namespace Template.MobileApp.Services;

using Rester;

internal static partial class Log
{
    // Network

    [LoggerMessage(Level = LogLevel.Warning, Message = "Network operation failed. result=[{restResult}], statusCode=[{statusCode}]")]
    public static partial void WarnNetworkOperationFailed(this ILogger logger, RestResult restResult, int statusCode, Exception? exception);

    [LoggerMessage(Level = LogLevel.Warning, Message = "API problem. method=[{method}], path=[{path}], statusCode=[{statusCode}], title=[{title}], traceId=[{traceId}]")]
    public static partial void WarnApiProblem(this ILogger logger, HttpMethod method, string? path, int statusCode, string? title, string? traceId);

    [LoggerMessage(Level = LogLevel.Information, Message = "Login. id=[{id}], expires=[{expires}]")]
    public static partial void InfoLogin(this ILogger logger, string id, DateTime? expires);

    [LoggerMessage(Level = LogLevel.Warning, Message = "Login failed. id=[{id}], result=[{restResult}], statusCode=[{statusCode}]")]
    public static partial void WarnLoginFailed(this ILogger logger, string id, RestResult restResult, int statusCode);
}
