using BusinessLayer.Interface;
using BusinessLayer.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelLayer;
using RepositoryLayer.CustomException;
using RepositoryLayer.Service;

namespace Fundoo.Controllers
{
    [Route("api/Collaborator")]
    [ApiController]
    public class CollaboratorController : ControllerBase
    {
        private readonly ICollaboratorBL collaboratorBL;
        private readonly ResponseML responseML;

        public CollaboratorController(ICollaboratorBL collaboratorBL)
        {
            this.collaboratorBL = collaboratorBL;
            responseML = new ResponseML();
        }

        [HttpPost]
        [Route("AddCollaborator")]
        [Authorize]
        public IActionResult addCollaborator(CollaboratorModel model)
        {
            try
            {
                var result = collaboratorBL.addCollaborator(model);
                if (result != null)
                {
                    responseML.Success = true;
                    responseML.Message = "Added successfully with id: " + result.Id;
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

        [HttpGet]
        [Route("GetCollaborators")]
        [Authorize]
        public IActionResult getCollaborators(int NoteId)
        {
            try
            {
                var result = collaboratorBL.getCollaboratorsByNoteId(NoteId);
                if (result != null)
                {
                    responseML.Success = true;
                    responseML.Message = "Request Successful";
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

        [HttpDelete]
        [Route("RemoveCollaborator")]
        [Authorize]
        public IActionResult removeCollaborator(string collaboratorEmail)
        {
            try
            {
                var result = collaboratorBL.removeCollaborator(collaboratorEmail);
                if (result != null)
                {
                    responseML.Success = true;
                    responseML.Message = "Collaborator removed successfully with id: " + result.Id;
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
