namespace Template.MobileApp.Interop.Dialogs;

using Template.MobileApp.Helpers;

public sealed class CameraCaptureViewModel : DialogViewModelBase, IPopupInitialize<int>
{
    // Field

    private const int JpegQuality = 85;

    private readonly IDialog dialog;

    private int maxSize;

    // Property

    public CameraController Controller { get; } = new();

    // Command

    public IObserveCommand CancelCommand { get; }

    public IObserveCommand CaptureCommand { get; }

    //--------------------------------------------------------------------------------
    // Constructor
    //--------------------------------------------------------------------------------

    public CameraCaptureViewModel(IDialog dialog)
    {
        this.dialog = dialog;

        CancelCommand = MakeAsyncCommand(async () => await CloseAsync());
        CaptureCommand = MakeAsyncCommand(CaptureAsync);
    }

    //--------------------------------------------------------------------------------
    // Operation
    //--------------------------------------------------------------------------------

    public void Initialize(int parameter) => maxSize = parameter;

    private async Task CaptureAsync()
    {
        await using var input = await Controller.CaptureWithTimeoutAsync();
        if (input is null)
        {
            await dialog.Toast("撮影できませんでした。もう一度押してください。");
            return;
        }

        var image = await Task.Run(() => Encode(input));
        await CloseAsync(image);
    }

    //--------------------------------------------------------------------------------
    // Helper
    //--------------------------------------------------------------------------------

    private byte[] Encode(Stream input)
    {
        using var bitmap = ImageHelper.ToNormalizeBitmap(input);
        var factor = Math.Min(1d, (double)maxSize / Math.Max(bitmap.Width, bitmap.Height));
        using var resized = ImageHelper.Resize(bitmap, factor);
        using var data = resized.Encode(SKEncodedImageFormat.Jpeg, JpegQuality);
        return data.ToArray();
    }
}
