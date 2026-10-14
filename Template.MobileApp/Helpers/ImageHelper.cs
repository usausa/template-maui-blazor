namespace Template.MobileApp.Helpers;

public static class ImageHelper
{
    public static SKBitmap ToNormalizeBitmap(Stream stream)
    {
        using var codec = SKCodec.Create(stream);
        return codec.DecodeOriented();
    }

    public static SKBitmap Resize(SKBitmap bitmap, double factor) => bitmap.Resize((float)factor);
}
