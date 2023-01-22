
using FireSharp;
using FireSharp.Config;
using FireSharp.Interfaces;
using ProxyServerDotNet.Models;

namespace ProxyServerDotNet.Manager
{
    public class FireBaseManager
    {
        static public IFirebaseClient initialConfig()
        {
            IFirebaseConfig config = new FirebaseConfig
            {
                AuthSecret = Environment.GetEnvironmentVariable("AuthSecret"),
                BasePath = Environment.GetEnvironmentVariable("BasePath")
            };

            return new FirebaseClient(config);
        }

        static async public Task<List<Dish>> GetAllDishes()
        {
            var client = initialConfig();
            var response = await client.GetAsync("dishesList");

            List<Dish> dishes = response.ResultAs<List<Dish>>();

            return  dishes;
        }
    }
}
