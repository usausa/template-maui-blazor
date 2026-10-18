namespace Template.MobileApp.Models.Entity;

using Smart.Data.Accessor.Attributes;

[Name("Store")]
public sealed class StoreEntity
{
    [Key]
    public long Id { get; set; }

    public string Name { get; set; } = default!;

    public StoreCategory Category { get; set; }

    public string Area { get; set; } = default!;

    public int PointRate { get; set; }
}
