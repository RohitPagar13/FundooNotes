using BusinessLayer.Interface;
using BusinessLayer.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelLayer;
using RepositoryLayer.CustomException;

namespace Fundoo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NoteController : ControllerBase
    {
        private readonly INoteBL noteBL;
        private readonly ResponseML responseML;

        public NoteController(INoteBL noteBL)
        {
            this.noteBL = noteBL;
            responseML = new ResponseML();
        }

        [HttpPost("Add")]
        public IActionResult addNote(NoteInputModel noteModel)
        {
            try
            {
                int userId = Convert.ToInt32(User.FindFirst("Id")?.Value);
                var result = noteBL.addNote(noteModel, userId);
                if (result != null)
                {
                    responseML.Success = true;
                    responseML.Message = "Created successfully with id: " + result.Id;
                    responseML.Data = result;
                }
                return StatusCode(201, responseML);
            }
            catch (UserException ex)
            {
                Console.WriteLine(ex.ErrorCode + ": " + ex.Message);
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

        [HttpDelete("Delete")]
        [Route("/{id}")]
        public IActionResult DeleteNoteById(int id)
        {
            try
            {
                var result = noteBL.removeNote(id);
                if (result != null)
                {
                    responseML.Success = true;
                    responseML.Message = "Note deleted Successfully with id: " + result.Id;
                    responseML.Data = result;
                }
                return StatusCode(204, responseML);
            }
            catch (UserException ex)
            {
                Console.WriteLine(ex.ErrorCode + ": " + ex.Message);
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

        [HttpGet("Get")]
        [Route("/{id}")]
        public IActionResult GetNoteById(int id)
        {
            try
            {
                var result = noteBL.getNoteById(id);

                if (result != null)
                {
                    responseML.Success = true;
                    responseML.Message = "Request successful for id: " + result.Id;
                    responseML.Data = result;
                }
                return StatusCode(200, responseML);
            }
            catch (UserException ex)
            {
                Console.WriteLine(ex.ErrorCode + ": " + ex.Message);
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
