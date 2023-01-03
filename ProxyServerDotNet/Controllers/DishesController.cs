using Microsoft.AspNetCore.Mvc;
using ProxyServerDotNet.Models.Inputs;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ProxyServerDotNet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DishesController : ControllerBase
    {
        /// <summary>
        /// Signs In User
        /// </summary>
        /// <param name="input"></param>
        [HttpPost]
        [Route("/auth")]
        public void LogIn([FromBody] AuthInput input)
        {
            if (input != null)
            {

                Ok("ff");
            }
        }

        // GET: api/<dishes>
        [HttpGet]
        [Route("/auth")]
        public IActionResult LogOut()
        {
            var myRes = new MyContent
            {
                Result = Environment.GetEnvironmentVariable("MY_SECRET"),
                Message = "Response Successfull"
            };
            return Ok(myRes);
        }

        // GET: api/<dishes>
        [HttpGet]
        public IEnumerable<string> GetDishes()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<dishes>/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        // POST api/<dishes>
        [HttpPost]
        public void AddNewDish([FromBody] string value)
        {
        }

        // PUT api/<dishes>/5
        [HttpPut("{id}")]
        public void UpdateDish(int id, [FromBody] string value)
        {
        }

        // DELETE api/<dishes>/5
        [HttpDelete("{id}")]
        public void DeletDish(int id)
        {
        }
    }
}
