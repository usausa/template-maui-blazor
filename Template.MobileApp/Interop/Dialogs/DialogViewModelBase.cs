namespace Template.MobileApp.Interop.Dialogs;

using Smart.Mvvm.Resolver;

[ObservableGeneratorOption(Reactive = true, ViewModel = true)]
public abstract class DialogViewModelBase : ExtendViewModelBase, IValidatable
{
    private List<ValidationResult>? validationResults;

    private IAccessor? propertyAccessor;

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);

        System.Diagnostics.Debug.WriteLine($"{GetType()} is Disposed");
    }

    public void Validate(string name)
    {
        propertyAccessor ??= AccessorRegistry.FindAccessor(GetType());
        if (propertyAccessor is null)
        {
            throw new InvalidOperationException($"Accessor is not supported. type=[{GetType()}]");
        }

        var value = propertyAccessor.GetValue(this, name);
        var context = new ValidationContext(this, DefaultResolveProvider.Default, null)
        {
            MemberName = name
        };
        validationResults ??= new List<ValidationResult>();

        if (!Validator.TryValidateProperty(value, context, validationResults))
        {
            Errors.AddError(name, validationResults[0].ErrorMessage!);
        }

        validationResults.Clear();
    }
}
