namespace Portfolio.API.DTOS
{
    public class PagedResponse<T>
    {
        public int TotalCount { get; set; }
        public IReadOnlyList<T> Data { get; set; }
    }


}
