using BusinessLayer.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
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
        private readonly ResponseML responseML;

        public UserController(IUserBL userBL)
        {
            this.userBL = userBL;
            responseML = new ResponseML();
        }

        [HttpPost("Register")]
        public ActionResult RegisterUser(UserModel userModel)
        {
            try
            {
                var result = userBL.RegisterUser(userModel);
                if (result != null)
                {
                    responseML.Success = true;
                    responseML.Message = "Created successfully with id: "+result.Id;
                    responseML.Data = result;
                }
                return StatusCode(201, responseML);
            }
            catch (UserException ex)
            {
                Console.WriteLine(ex.ErrorCode +": "+ex.Message);
                responseML.Success = false;
                responseML.Message = ex.ErrorCode + ": " + ex.Message;
                return StatusCode(202, responseML);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                responseML.Success = false;
                responseML.Message = ex.Message;
                return StatusCode(400, responseML);
            }
        }

        [HttpPost("Login")]
        public ActionResult LoginUser(UserLoginModel userLoginModel)
        {
            try
            {
                var result = userBL.LoginUser(userLoginModel);

                if (result != null)
                {
                    responseML.Success = true;
                    responseML.Message = "Login successful with id: "+result.ID; 
                    responseML.Data = result;
                }
                return StatusCode(201, responseML);
            }
            catch (UserException ex)
            {
                Console.WriteLine(ex.Message);
                responseML.Success = false;
                responseML.Message = ex.ErrorCode + ": " + ex.Message;
                return StatusCode(202, responseML);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                responseML.Success = false;
                responseML.Message = ex.Message;
                return StatusCode(400, responseML);
            }
        }
    }
}
