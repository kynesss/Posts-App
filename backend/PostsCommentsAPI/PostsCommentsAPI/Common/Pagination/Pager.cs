namespace PostsCommentsAPI.Common.Pagination;

public class Pager
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string Sort { get; set; } = "CreatedAt";
    public string Order { get; set; } = "desc";
}