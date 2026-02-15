using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Reflection;

namespace PostsCommentsAPI.Common.Pagination;

public static class PaginationExtensions
{
    private const int DefaultPage = 1;
    private const int DefaultPageSize = 10;
    private const int MaxPageSize = 100;

    private const string DefaultSort = "CreatedAt";
    private const string Asc = "asc";
    private const string Oldest = "oldest";

    public static async Task<Pagination<T>> PaginateAsync<T>(
        this IQueryable<T> query,
        Pager pager,
        CancellationToken cancellationToken = default)
    {
        return await query.PaginateAsync(pager, item => item, cancellationToken);
    }

    public static async Task<Pagination<TProjection>> PaginateAsync<TSource, TProjection>(
        this IQueryable<TSource> query,
        Pager pager,
        Expression<Func<TSource, TProjection>> selector,
        CancellationToken cancellationToken = default)
    {
        var page = pager.Page ?? DefaultPage;
        var pageSize = pager.PageSize ?? DefaultPageSize;
        var normalizedPage = page < 1 ? DefaultPage : page;
        var normalizedPageSize = pageSize < 1 ? DefaultPageSize : Math.Min(pageSize, MaxPageSize);

        var sortedQuery = ApplySorting(query, pager);

        var totalCount = await sortedQuery.CountAsync(cancellationToken);
        var items = await sortedQuery
            .Skip((normalizedPage - 1) * normalizedPageSize)
            .Take(normalizedPageSize)
            .Select(selector)
            .ToListAsync(cancellationToken);

        return new Pagination<TProjection>(items, normalizedPage, normalizedPageSize, totalCount);
    }

    public static async Task<Pagination<TDestination>> PaginateAsync<TSource, TDestination>(
        this MappedQuery<TSource, TDestination> mappedQuery,
        Pager pager,
        CancellationToken cancellationToken = default)
    {
        var page = pager.Page ?? DefaultPage;
        var pageSize = pager.PageSize ?? DefaultPageSize;
        var normalizedPage = page < 1 ? DefaultPage : page;
        var normalizedPageSize = pageSize < 1 ? DefaultPageSize : Math.Min(pageSize, MaxPageSize);

        var sortedQuery = ApplySorting(mappedQuery.Query, pager);

        var totalCount = await sortedQuery.CountAsync(cancellationToken);
        var items = await sortedQuery
            .Skip((normalizedPage - 1) * normalizedPageSize)
            .Take(normalizedPageSize)
            .ProjectTo<TDestination>(mappedQuery.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return new Pagination<TDestination>(items, normalizedPage, normalizedPageSize, totalCount);
    }

    private static IQueryable<T> ApplySorting<T>(IQueryable<T> query, Pager pager)
    {
        var sortBy = string.IsNullOrWhiteSpace(pager.Sort) ? DefaultSort : pager.Sort.Trim();
        var isDescending = IsDescending(pager.Order);

        if (TryApplyOrder(query, sortBy, isDescending, out var sorted))
        {
            return sorted;
        }

        if (!string.Equals(sortBy, DefaultSort, StringComparison.OrdinalIgnoreCase) &&
            TryApplyOrder(query, DefaultSort, isDescending, out var fallbackSorted))
        {
            return fallbackSorted;
        }

        return query;
    }

    private static bool IsDescending(string? sort)
    {
        if (string.IsNullOrWhiteSpace(sort))
        {
            return true;
        }

        return !string.Equals(sort, Asc, StringComparison.OrdinalIgnoreCase) &&
               !string.Equals(sort, Oldest, StringComparison.OrdinalIgnoreCase);
    }

    private static bool TryApplyOrder<T>(
        IQueryable<T> query,
        string sortBy,
        bool descending,
        out IQueryable<T> sortedQuery)
    {
        var property = typeof(T)
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .FirstOrDefault(prop => string.Equals(prop.Name, sortBy, StringComparison.OrdinalIgnoreCase));

        if (property is null)
        {
            sortedQuery = query;
            return false;
        }

        var parameter = Expression.Parameter(typeof(T), "x");
        var propertyAccess = Expression.Property(parameter, property);
        var keySelector = Expression.Lambda(propertyAccess, parameter);

        var methodName = descending ? nameof(Queryable.OrderByDescending) : nameof(Queryable.OrderBy);
        var method = typeof(Queryable)
            .GetMethods(BindingFlags.Public | BindingFlags.Static)
            .Single(m => m.Name == methodName && m.GetParameters().Length == 2)
            .MakeGenericMethod(typeof(T), property.PropertyType);

        sortedQuery = (IQueryable<T>)method.Invoke(null, [query, keySelector])!;
        return true;
    }
}
