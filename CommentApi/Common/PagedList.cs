namespace CommentApi.Common
{
    public record PagedList<T>(List<T> Items, int Page, int PageSize, int TotalCount)
    {
        public bool HasNextPage => Page * PageSize < TotalCount;
        public bool HasPreviousPage => Page > 1;
    }
}
