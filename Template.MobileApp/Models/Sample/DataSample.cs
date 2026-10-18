namespace Template.MobileApp.Models.Sample;

public static class DataSample
{
    public static IReadOnlyList<WorkEntity> LoadWorks() =>
    [
        new() { Id = 1, Name = "Sample-1" },
        new() { Id = 2, Name = "Sample-2" },
        new() { Id = 3, Name = "Sample-3" },
        new() { Id = 4, Name = "Sample-4" }
    ];
}
