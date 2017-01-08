using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace RealEstate.App_Start
{
    public class RealEstateContext
    {
        private IConfigurationRoot _config;
        public IMongoDatabase Database { get; set; }

        public RealEstateContext(IConfigurationRoot config) {
            _config = config;

            var client = new MongoClient(_config["ConnectionStrings:RealEstateConnection"]);
            Database = client.GetDatabase(_config["Databases:RealEstateDatabaseName"]);
        }
    }
}