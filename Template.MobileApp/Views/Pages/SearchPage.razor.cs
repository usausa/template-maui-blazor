namespace Template.MobileApp.Views.Pages;

using Template.MobileApp.Services;

public sealed partial class SearchPage : AppComponentBase
{
    private static readonly StoreCategory[] Categories =
    [
        StoreCategory.Restaurant,
        StoreCategory.Convenience,
        StoreCategory.Drugstore,
        StoreCategory.Supermarket,
        StoreCategory.Other
    ];

    private string? keyword;

    private StoreCategory? category;

    private List<StoreEntity>? stores;

    //--------------------------------------------------------------------------------
    // Property
    //--------------------------------------------------------------------------------

    [Inject]
    public required DataService DataService { get; set; }

    //--------------------------------------------------------------------------------
    // Initialize
    //--------------------------------------------------------------------------------

    protected override Task OnInitializedAsync()
    {
        return SearchAsync();
    }

    //--------------------------------------------------------------------------------
    // Action
    //--------------------------------------------------------------------------------

    private Task SelectCategoryAsync(StoreCategory? value)
    {
        category = value;
        return SearchAsync();
    }

    private async Task SearchAsync()
    {
        stores = await DataService.QueryStoreListAsync(keyword, category);
    }
}
