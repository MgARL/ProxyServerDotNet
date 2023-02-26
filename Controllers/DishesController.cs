using FireSharp.Response;
using Microsoft.AspNetCore.Authorization;
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
        [Route("auth/login")]
        public async Task<IActionResult> LogIn([FromBody] AuthInput input)
        {
            try
            {
                var authProvider = FireBaseAuthManager.initialConfig();
                var userCredential = await authProvider.SignInWithEmailAndPasswordAsync(input.Email, input.Password);

                if (userCredential != null)
                {

                    return Ok(userCredential);
                }
                return BadRequest("Wrong Credentials");

            }
            catch (Exception err)
            {
                
               return BadRequest(err.Message);
            }
        }

        /// <summary>
        /// Authorize valid user
        /// </summary>
        /// <param name="input"></param>
        [HttpGet]
        [Route("auth")]
        [Authorize]
        public IActionResult Authorize()
        {
            try
            {
                var userEmail = this.User.Identities.ToList()[0].Claims.ToList()[1].Value;
                if (userEmail != null)
                {
                    var rtn = new AuthInput { Email = userEmail };
                    return Ok(rtn);
                }
                return Unauthorized();
            }
            catch (Exception err)
            {

                return BadRequest(err.Message);
            }
        }

        /// <summary>
        /// reset password
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("auth/reset")]
        [Authorize]
        public IActionResult ResetPassword()
        {
           return Ok();
        }

        /// <summary>
        /// Get All Recepies in DB
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("all")]
        [Authorize]
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
                if (dishes.Any())
                {
                    return Ok(dishes);
                }
                return BadRequest("No Dishes Found");
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
        [Authorize]
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

        /// <summary>
        /// Updates dish with Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="updatedDish"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateDish(int id, [FromBody] Dish updatedDish)
        {
            try
            {
                var res = await DishesManger.UpdateDish(id, updatedDish);
                if (res != null)
                {
                    return Ok(res.StatusCode);
                }
                return BadRequest("No Record Updated");
            }
            catch (Exception err)
            {

                return BadRequest(err);
            }
        }

        /// <summary>
        /// Deletes specific dish form DB
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete("{id}")]
        [Authorize]
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
