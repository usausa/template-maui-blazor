namespace Template.MobileApp;

public sealed partial class MainPage
{
    public MainPage()
    {
        InitializeComponent();
    }

    protected override bool OnBackButtonPressed()
    {
        // TODO
        //if (BindingContext is MainPageViewModel { BusyState.IsBusy: false } context)
        //{
        //}

        return true;
    }
}
