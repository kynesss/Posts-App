using System.ComponentModel;

namespace PostsCommentsAPI.Common.Pagination;

public class Pager
{
    [DefaultValue(1)]
    public int? Page { get; set; }

    [DefaultValue(10)]
    public int? PageSize { get; set; }

    [DefaultValue("id")]
    public string? Sort { get; set; }

    [DefaultValue("asc")]
    public string? Order { get; set; }
}