using BusinessLayer.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelLayer;
using RepositoryLayer.CustomException;

namespace Fundoo.Controllers
{
    [Route("api/User")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserBL userBL;

        public UserController(IUserBL userBL)
        {
            this.userBL = userBL;
        }

        [HttpPost]
        public ActionResult AddCustomer([FromBody] UserModel userModel)
        {
            try
            {
                var result = userBL.RegisterUser(userModel);

                return Created("Created successfully", result);
            }
            catch (UserException ex)
            {
                Console.WriteLine(ex.ErrorCode + ": " + ex.Message);
                return Accepted(ex.ErrorCode + ": " + ex.Message); ;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return BadRequest(ex.Message);
            }
        }
    }
}
