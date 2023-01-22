using FireSharp.Response;
using ProxyServerDotNet.Manager;
using ProxyServerDotNet.Models;
using System.Collections.Generic;

namespace ProxyServerDotNet.Managers
{
    public class DishesManger
    {
        static async public Task<List<Dish>> GenerateFiveRandomDishes()
        {
            var dishes = await FireBaseManager.GetAllDishes();
            var  randomDishes = new List<Dish>();
            if (dishes.Any())
            {
                var random = new Random();
                randomDishes = dishes.OrderBy(d => random.Next()).ToList();

                randomDishes.RemoveRange(5, randomDishes.Count - 5);
            }

            return randomDishes;
        }

        static async public Task<SetResponse> AddDish(Dish newDish)
        {
            var client = FireBaseManager.initialConfig();
            var allDishes = await FireBaseManager.GetAllDishes();
            allDishes.Add(newDish);

            return await client.SetAsync("dishesList", allDishes); ;
        }

        static async public Task<SetResponse> DeleteDish(int id)
        {
            var client = FireBaseManager.initialConfig();
            var allDishes = await FireBaseManager.GetAllDishes();
            allDishes.RemoveAt(id); 

            return await client.SetAsync("dishesList", allDishes); ;
        }

        static async public Task<FirebaseResponse> UpdateDish(int id, Dish updatedDish)
        {
            var client = FireBaseManager.initialConfig();

            var res = await client.UpdateAsync($"dishesList/{id}", updatedDish);
            return res;
        }
    }
}
