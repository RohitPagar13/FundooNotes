using BusinessLayer.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelLayer;
using RepositoryLayer.CustomException;

namespace Fundoo.Controllers
{
    [Route("api/Label")]
    [ApiController]
    public class LabelController : ControllerBase
    {
        private readonly ILabelBL labelBL;
        private readonly ResponseML responseML;

        public LabelController(ILabelBL labelBL)
        {
            this.labelBL = labelBL;
            responseML = new ResponseML();
        }

        [HttpPost("createLabel")]
        [Authorize]
        public IActionResult CreateLabel(string labelName)
        {
            try
            {
                var result = labelBL.createLabel(labelName);

                responseML.Success = true;
                responseML.Message = "Label added successfully";
                responseML.Data = result;

                return StatusCode(201, responseML);
            }
            catch (UserException ex)
            {
                responseML.Success = false;
                responseML.Message = ex.Message;

                return StatusCode(400, responseML);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                responseML.Success = false;
                responseML.Message = ex.Message;
                return StatusCode(400, responseML);
            }
        }

        [HttpPut("updateLabel")]
        [Authorize]
        public IActionResult UpdateLabel(int id, string newLabelName)
        {
            try
            {
                var result = labelBL.UpdateLabel(id, newLabelName);

                responseML.Success = true;
                responseML.Message = "Label updated successfully";
                responseML.Data = result;

                return StatusCode(200, responseML);
            }
            catch (UserException ex)
            {
                responseML.Success = false;
                responseML.Message = ex.Message;

                return StatusCode(404, responseML);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                responseML.Success = false;
                responseML.Message = ex.Message;
                return StatusCode(400, responseML);
            }
        }

        [HttpGet("getById/{id}")]
        [Authorize]
        public IActionResult GetLabelById(int id)
        {
            try
            {
                var result = labelBL.GetLabelById(id);

                responseML.Success = true;
                responseML.Message = $"Label ID : {id} fetched successfully";
                responseML.Data = result;

                return StatusCode(200, responseML);
            }
            catch (UserException ex)
            {
                responseML.Success = false;
                responseML.Message = ex.Message;

                return StatusCode(404, responseML);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                responseML.Success = false;
                responseML.Message = ex.Message;
                return StatusCode(400, responseML);
            }
        }

        [HttpGet("getallLabels")]
        [Authorize]
        public IActionResult GetAllLabels()
        {
            try
            {
                var result = labelBL.GetLabels();

                responseML.Success = true;
                responseML.Message = $"All Label fetched successfully";
                responseML.Data = result;

                return StatusCode(200, responseML);
            }
            catch (UserException ex)
            {
                responseML.Success = false;
                responseML.Message = ex.Message;

                return StatusCode(404, responseML);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                responseML.Success = false;
                responseML.Message = ex.Message;
                return StatusCode(400, responseML);
            }
        }

        [HttpDelete("deleteById/{id}")]
        [Authorize]
        public IActionResult DeleteLabel(int id)
        {
            try
            {
                labelBL.removeLabel(id);

                responseML.Success = true;
                responseML.Message = $"Label ID : {id} deleted successfully";

                return StatusCode(200, responseML);
            }
            catch (UserException ex)
            {
                responseML.Success = false;
                responseML.Message = ex.Message;

                return StatusCode(404, responseML);
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
