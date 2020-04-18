using System.Linq;

namespace FarmAppServer.Services.Paging
{
    public class PagedResult<T> : PagedResultBase where T : class
    {
        public IQueryable<T> Results { get; set; }
    }
}