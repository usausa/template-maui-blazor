namespace Template.MobileApp;

public sealed partial class MainPage
{
    private readonly BackButtonState backButton;

    public MainPage(BackButtonState backButton)
    {
        this.backButton = backButton;
        InitializeComponent();
    }

    protected override bool OnBackButtonPressed()
    {
        if (!backButton.Publish())
        {
            AndroidHelper.MoveTaskToBack();
        }

        return true;
    }
}
