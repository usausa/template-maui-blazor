namespace Template.MobileApp.Markup;

using Template.MobileApp.Converters;
using Template.MobileApp.Views;

[AcceptEmptyServiceProvider]
public sealed class SelectedToColorExtension : IMarkupExtension<SelectedToColorConverter>
{
    public SelectedPage Page { get; set; }

    public Color Default { get; set; } = default!;

    public Color Selected { get; set; } = default!;

    public SelectedToColorConverter ProvideValue(IServiceProvider serviceProvider) =>
        new() { Page = Page, Default = Default, Selected = Selected };

    object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider) => ProvideValue(serviceProvider);
}

[AcceptEmptyServiceProvider]
public sealed class SelectedToImageSourceExtension : IMarkupExtension<SelectedToImageSourceConverter>
{
    public SelectedPage Page { get; set; }

    public ImageSource Default { get; set; } = default!;

    public ImageSource Selected { get; set; } = default!;

    public SelectedToImageSourceConverter ProvideValue(IServiceProvider serviceProvider) =>
        new() { Page = Page, Default = Default, Selected = Selected };

    object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider) => ProvideValue(serviceProvider);
}
