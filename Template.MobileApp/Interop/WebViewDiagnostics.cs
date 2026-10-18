namespace Template.MobileApp.Interop;

public sealed record WebViewDiagnostics(
    string? PackageName,
    string? PackageVersion,
    TimeSpan? StartupTime,
    int RecreateCount,
    NetworkAccess NetworkAccess);
