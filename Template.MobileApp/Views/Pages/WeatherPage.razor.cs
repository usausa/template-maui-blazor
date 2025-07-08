namespace Template.MobileApp.Views.Pages;

public sealed partial class WeatherPage : AppComponentBase
{
    private IQueryable<WeatherForecast>? forecasts;

#pragma warning disable CA5394
    protected override async Task OnInitializedAsync()
    {
        // Simulate asynchronous loading to demonstrate a loading indicator
        await Task.Delay(500);

        var startDate = DateOnly.FromDateTime(DateTime.Now);
        var summaries = new[] { "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching" };
        forecasts = Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = startDate.AddDays(index),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = summaries[Random.Shared.Next(summaries.Length)]
        }).AsQueryable();
    }
#pragma warning restore CA5394

    private sealed class WeatherForecast
    {
        public DateOnly Date { get; init; }
        public int TemperatureC { get; init; }
        public string? Summary { get; init; }
        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
    }
}
