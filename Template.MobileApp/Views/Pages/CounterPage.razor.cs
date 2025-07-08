namespace Template.MobileApp.Views.Pages;

public sealed partial class CounterPage : AppComponentBase
{
    private int currentCount;

    private void IncrementCount()
    {
        currentCount++;
    }
}
