namespace PostsCommentsAPI.Common.Pagination;

public sealed class MappedQuery<TSource, TDestination>
{
    public MappedQuery(IQueryable<TSource> query, AutoMapper.IConfigurationProvider configurationProvider)
    {
        Query = query;
        ConfigurationProvider = configurationProvider;
    }

    public IQueryable<TSource> Query { get; }
    public AutoMapper.IConfigurationProvider ConfigurationProvider { get; }
}