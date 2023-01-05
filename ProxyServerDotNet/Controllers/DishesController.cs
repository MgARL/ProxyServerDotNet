using FireSharp;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using Microsoft.AspNetCore.Mvc;
using ProxyServerDotNet.Manager;
using ProxyServerDotNet.Managers;
using ProxyServerDotNet.Models;
using ProxyServerDotNet.Models.Inputs;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ProxyServerDotNet.Controllers
{
    [Route("api/dishes")]
    [ApiController]
    public class DishesController : ControllerBase
    {
        /// <summary>
        /// Signs In User
        /// </summary>
        /// <param name="input"></param>
        [HttpPost]
        [Route("auth")]
        public void LogIn([FromBody] AuthInput input)
        {
            if (input != null)
            {

                Ok("ff");
            }
        }

        /// <summary>
        /// Signs Out User
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("auth")]
        public IActionResult LogOut()
        {
            var myRes = new MyContent
            {
                Result = Environment.GetEnvironmentVariable("MY_SECRET"),
                Message = "Response Successfull"
            };
            return Ok(myRes);
        }

        /// <summary>
        /// Get All Recepies in DB
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("all")]
        public async Task<IActionResult> GetDishesAsync()
        {
            try
            {
                var client = FireBaseManager.initialConfig();
                FirebaseResponse response = await client.GetAsync("dishesList");

                List<Dish> myRes = response.ResultAs<List<Dish>>();

                return Ok(myRes);
            }
            catch (Exception err)
            {
                return BadRequest(err);
            }
        }
        
        /// <summary>
        /// Gets five random meals for each day of week.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("five")]
        public async Task<IActionResult> GetFiveRandomDishesAsync()
        {
            try
            {
                var dishes = await DishesManger.GenerateFiveRandomDishes();

                return Ok(dishes);
            }
            catch (Exception err)
            {
                return BadRequest(err);
            }
        }

        /// <summary>
        /// Adds new meal to DB
        /// </summary>
        /// <param name="newDish"></param>
        [HttpPost]
        public async Task<IActionResult> AddNewDish([FromBody] Dish newDish)
        {
            try
            {
                var res = await DishesManger.AddDish(newDish);

                return Ok(res.StatusCode);
            }
            catch (Exception err)
            {
                return BadRequest(err);
            }
        }

        // PUT api/<dishes>/5
        //[HttpPut("{id}")]
        //public void UpdateDish(int id, [FromBody] string value)
        //{
        //}

        /// <summary>
        /// Deletes specific dish form DB
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletDish(int id)
        {
            try
            {
                var res = await DishesManger.DeleteDish(id);

                return Ok(res.StatusCode);

            }
            catch (Exception err)
            {
                return BadRequest(err);
            }
        }
    }
}
