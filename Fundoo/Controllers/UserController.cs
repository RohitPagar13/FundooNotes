using BusinessLayer.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ModelLayer;
using RepositoryLayer.CustomException;
using RepositoryLayer.Utilities;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;

namespace Fundoo.Controllers
{
    [Route("api/User")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserBL userBL;
        private readonly ResponseML responseML;
        private readonly RabbitMQProducer _rabbitMQProducer;
        public UserController(IUserBL userBL, RabbitMQProducer rabbitMQProducer)
        {
            this.userBL = userBL;
            responseML = new ResponseML();
            _rabbitMQProducer = rabbitMQProducer;
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
                    responseML.Message = "Created successfully with id: "+result.ID;
                    responseML.Data = result;
                    _rabbitMQProducer.SendMessage(responseML.Message, result.Email, "Registration Successful");
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
                if (userLoginModel == null || string.IsNullOrEmpty(userLoginModel.Email) || string.IsNullOrEmpty(userLoginModel.Password))
                {
                    return BadRequest("Invalid client request");
                }

                string token = userBL.LoginUser(userLoginModel);
                if (token != null)
                {
                    responseML.Success = true;
                    responseML.Message = "Login successful";
                    responseML.Data = token;
                    
                }
                return StatusCode(200, responseML);
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

        [HttpGet]
        [Route("Login/GetUser")]
        [Authorize]
        [EnableCors("getUserPolicy")]
        public IActionResult GetUser()
        {
            try
            {
                var result = userBL.getUser(User.FindFirst("Email").Value);
                if (result != null)
                {
                    responseML.Success = true;
                    responseML.Message = "Request Successful";
                    responseML.Data = result;
                    return StatusCode(200, responseML);
                }
                else
                {
                    responseML.Success = true;
                    responseML.Message = "Some Error occurred!!";
                    responseML.Data = result;
                    return StatusCode(400, responseML);
                }
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

        [HttpGet]
        [Route("ForgotUser/{email}")]
        public IActionResult ForgotUser(string email)
        {
            try
            {
                userBL.ForgetPassword(email);

                    responseML.Success = true;
                    responseML.Message = "Request Successful, email has been sent";
                    return StatusCode(200, responseML); 
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                responseML.Success = false;
                responseML.Message = ex.Message;
                return StatusCode(400, responseML);
            }
        }

        [HttpPut]
        [Authorize]
        [Route("ResetUser")]
        public IActionResult ResetUser([FromBody]string password)
        {
            try
            {
                string Email = User.FindFirst("Email").Value;
                userBL.ResetPassword(Email, password);
                    responseML.Success = true;
                    responseML.Message = "Request Successful, password reset successful";
                _rabbitMQProducer.SendMessage(responseML.Message, Email, "Password Reset Successful");
                return StatusCode(200, responseML); 
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