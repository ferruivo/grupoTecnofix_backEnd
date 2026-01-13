namespace GrupoTecnofix_Api.Dtos
{
    public class PagedResult<T>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize);
        public List<T> Items { get; set; } = new();
    }

    public class PagedRequest
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;

        public void Normalize(int maxPageSize = 200)
        {
            if (Page < 1) Page = 1;
            if (PageSize < 1) PageSize = 20;
            if (PageSize > maxPageSize) PageSize = maxPageSize;
        }
    }
}
