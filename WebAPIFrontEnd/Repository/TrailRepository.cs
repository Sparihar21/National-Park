using WebAPIFrontEnd.Models;
using WebAPIFrontEnd.Repository.IRepository;

namespace WebAPIFrontEnd.Repository
{
    public class TrailRepository:Repository<Trails>,ITrailRepository
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public TrailRepository(IHttpClientFactory httpClientFactory):base(httpClientFactory) 
        {
            _httpClientFactory = httpClientFactory;
        }
    }
}
