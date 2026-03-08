namespace IdentityService.src.Web.Common.TemplateResponses
{
    public class PagedResponse<T>
    {
        public IEnumerable<T> Data { get; set; }
        public int TotalCount { get; set; }

        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
