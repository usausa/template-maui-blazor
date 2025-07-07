namespace Template.MobileApp.Converters;

using Template.MobileApp.Views;

public abstract class SelectedToConverter<T> : IValueConverter
{
    public SelectedPage Page { get; set; }

    public T Default { get; set; } = default!;

    public T Selected { get; set; } = default!;

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return Equals(value, Page) ? Selected : Default;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => throw new NotSupportedException();
}

public sealed class SelectedToColorConverter : SelectedToConverter<Color>
{
}

public sealed class SelectedToImageSourceConverter : SelectedToConverter<ImageSource>
{
}
