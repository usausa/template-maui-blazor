namespace Template.MobileApp;

public sealed partial class MainPage
{
    public MainPage()
    {
        InitializeComponent();
    }

    protected override bool OnBackButtonPressed()
    {
        AndroidHelper.MoveTaskToBack();

        return true;
    }
}
