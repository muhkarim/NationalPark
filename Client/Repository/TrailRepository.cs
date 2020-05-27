using Client.Models;
using Client.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Client.Repository
{
    

    public class TrailRepository : Repository<Trail>, ITrailRepository
    {
        private IHttpClientFactory _clientFactory;


        public TrailRepository(IHttpClientFactory clientFactory) : base(clientFactory)
        {
            _clientFactory = clientFactory;
        }
    }
}
