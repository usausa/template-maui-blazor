namespace Template.MobileApp.Markup;

using Template.MobileApp.Converters;
using Template.MobileApp.Views;

[AcceptEmptyServiceProvider]
public sealed class PageToColorExtension : IMarkupExtension<PageToColorConverter>
{
    public SelectPage Page { get; set; }

    public Color Default { get; set; } = default!;

    public Color Selected { get; set; } = default!;

    public PageToColorConverter ProvideValue(IServiceProvider serviceProvider) =>
        new() { Page = Page, Default = Default, Selected = Selected };

    object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider) => ProvideValue(serviceProvider);
}

[AcceptEmptyServiceProvider]
public sealed class PageToImageSourceExtension : IMarkupExtension<PageToImageSourceConverter>
{
    public SelectPage Page { get; set; }

    public ImageSource Default { get; set; } = default!;

    public ImageSource Selected { get; set; } = default!;

    public PageToImageSourceConverter ProvideValue(IServiceProvider serviceProvider) =>
        new() { Page = Page, Default = Default, Selected = Selected };

    object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider) => ProvideValue(serviceProvider);
}
