namespace SyspharmaApi.Contracts.DTO
{
    public class Paginator<T>(IEnumerable<T> responseList, int page = 0, int pagesize = 10) where T: class
    {
        public int Page { get; set; } = page;
        public int Pagesize { get; set; } = pagesize;
        public IEnumerable<T> ResponseList { get; set; } = responseList;
    }
}
