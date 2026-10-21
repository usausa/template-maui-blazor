namespace Template.MobileApp.Views;

using Microsoft.FluentUI.AspNetCore.Components;

public abstract class AppDialogBase : AppComponentBase
{
    private IDisposable? backButtonSubscription;

    [CascadingParameter]
    public required IDialogInstance Dialog { get; set; }

    [Inject]
    public required BackButtonState BackButton { get; set; }

    protected override void OnInitialized()
    {
        backButtonSubscription = BackButton.Subscribe(OnBackButton);
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            backButtonSubscription?.Dispose();
            backButtonSubscription = null;
        }

        base.Dispose(disposing);
    }

    protected virtual bool OnBackButton()
    {
        if (!BusyState.IsBusy && (Dialog.Options.PreventDismissOnEscape != true))
        {
            _ = InvokeAsync(Dialog.CancelAsync);
        }

        return true;
    }
}
