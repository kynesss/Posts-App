using AutoMapper;

namespace PostsCommentsAPI.Common.Pagination;

public static class QueryableMappingExtensions
{
    public static MappedQuery<TSource, TDestination> Map<TSource, TDestination>(
        this IQueryable<TSource> query,
        IMapper mapper)
    {
        return new MappedQuery<TSource, TDestination>(query, mapper.ConfigurationProvider);
    }
}
