namespace Template.MobileApp.Interop.Dialogs;

using System.Globalization;
using System.Security.Cryptography;

public sealed partial class BarcodeDisplayViewModel : DialogViewModelBase
{
    // Field

    private const int PaymentSeconds = 30;

    // Property

    [ObservableProperty]
    public partial string PaymentCode { get; set; } = string.Empty;

    [ObservableProperty]
    public partial int PaymentRemain { get; set; }

    [ObservableProperty]
    public partial double PaymentProgress { get; set; }

    //--------------------------------------------------------------------------------
    // Constructor
    //--------------------------------------------------------------------------------

    public BarcodeDisplayViewModel(IDispatcher dispatcher)
    {
        var timer = dispatcher.CreateTimer();
        timer.Interval = TimeSpan.FromSeconds(1);
        Disposables.Add(timer.TickAsObservable().Subscribe(_ => TickPayment()));
        Disposables.Add(new DelegateDisposable(timer.Stop));

        RenewPaymentCode();
        timer.Start();
    }

    //--------------------------------------------------------------------------------
    // Operation
    //--------------------------------------------------------------------------------

    private void TickPayment()
    {
        if (PaymentRemain <= 1)
        {
            RenewPaymentCode();
        }
        else
        {
            PaymentRemain--;
            PaymentProgress = (double)PaymentRemain / PaymentSeconds;
        }
    }

    //--------------------------------------------------------------------------------
    // Helper
    //--------------------------------------------------------------------------------

    private void RenewPaymentCode()
    {
        PaymentCode = String.Join(' ', Enumerable.Range(0, 5).Select(static _ => RandomNumberGenerator.GetInt32(10000).ToString("D4", CultureInfo.InvariantCulture)));
        PaymentRemain = PaymentSeconds;
        PaymentProgress = 1;
    }
}
