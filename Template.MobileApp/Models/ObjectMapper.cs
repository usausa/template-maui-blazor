namespace Template.MobileApp.Models;

using Smart.Mapper;

public static partial class ObjectMapper
{
    [Mapper]
    public static partial WorkEntity ToWorkEntity(DataListResponseEntry source);
}
