namespace Template.MobileApp.Models.Entity;

using Smart.Data.Accessor.Attributes;

using Template.MobileApp.Helpers.Data;

[Name("Notice")]
public sealed class NoticeEntity
{
    [Key]
    public long Id { get; set; }

    public NoticeCategory Category { get; set; }

    public string Title { get; set; } = default!;

    public string Body { get; set; } = default!;

    [TypeHandler(typeof(DateTimeTicksConverter))]
    public DateTime PublishedAt { get; set; }

    public bool IsRead { get; set; }
}
