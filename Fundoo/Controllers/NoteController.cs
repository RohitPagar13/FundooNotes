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
        public ActionResult addNote(NoteInputModel noteModel)
        {
            try
            {
                var result = noteBL.addNote(noteModel);
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
    }
}
