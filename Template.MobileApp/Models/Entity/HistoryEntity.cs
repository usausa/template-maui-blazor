namespace Template.MobileApp.Models.Entity;

using Smart.Data.Accessor.Attributes;

using Template.MobileApp.Helpers.Data;

[Name("History")]
public sealed class HistoryEntity
{
    [Key]
    public long Id { get; set; }

    public HistoryKind Kind { get; set; }

    public string Title { get; set; } = default!;

    public int Amount { get; set; }

    [TypeHandler(typeof(DateTimeTicksConverter))]
    public DateTime OccurredAt { get; set; }
}
