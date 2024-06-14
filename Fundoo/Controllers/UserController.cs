using BusinessLayer.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ModelLayer;
using RepositoryLayer.CustomException;
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
                    responseML.Message = "Created successfully with id: "+result.ID;
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
                return Ok(responseML);
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

        [HttpGet("GetUser")]
        [Authorize]
        public IActionResult GetUser()
        {
            try
            {
                LoginResponse lr = new LoginResponse();
                lr.ID = Convert.ToInt32(User.FindFirst("Id")?.Value);
                lr.FirstName = User.FindFirst("FirstName").Value;
                lr.LastName = User.FindFirst("LastName").Value;
                lr.Email = User.FindFirst("Email").Value;
                lr.Phone = User.FindFirst("Phone").Value;
                lr.BirthDate = User.FindFirst("BirthDate").Value;

                if (lr != null)
                {
                    responseML.Success = true;
                    responseML.Message = "Request Successful";
                    responseML.Data = lr;
                    return Ok(responseML);
                }
                else
                {
                    responseML.Success = true;
                    responseML.Message = "Some Error occurred!!";
                    responseML.Data = lr;
                    return StatusCode(400, responseML);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                responseML.Success = false;
                responseML.Message = ex.Message;
                return StatusCode(400, responseML);
            }
        }

        [HttpGet("ForgotUser")]
        [Route("ForgotUser/email")]
        public IActionResult ForgotUser(string email)
        {
            try
            {
                userBL.ForgetPassword(email);

                    responseML.Success = true;
                    responseML.Message = "Request Successful, email has been sent";
                    return Ok(responseML);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                responseML.Success = false;
                responseML.Message = ex.Message;
                return StatusCode(400, responseML);
            }
        }

        [HttpGet("ResetUser")]
        [Authorize]
        [Route("ResetUser/password")]
        public IActionResult ResetUser(string password)
        {
            try
            {
                string Email = User.FindFirst("Email").Value;
                userBL.ResetPassword(Email, password);
                    responseML.Success = true;
                    responseML.Message = "Request Successful, password reset successfu;";
                    return Ok(responseML);
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