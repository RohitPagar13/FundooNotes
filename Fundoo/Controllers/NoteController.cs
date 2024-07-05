using BusinessLayer.Interface;
using BusinessLayer.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelLayer;
using RepositoryLayer.CustomException;
using RepositoryLayer.Utilities;

namespace Fundoo.Controllers
{
    [Route("api/Note")]
    [ApiController]
    
    public class NoteController : ControllerBase
    {
        private readonly INoteBL noteBL;
        private readonly ResponseML responseML;
        private readonly KafkaProduser kafkaProduser;

        public NoteController(INoteBL noteBL)
        {
            this.noteBL = noteBL;
            responseML = new ResponseML();
        }

        [HttpPost]
        [Route("Add")]
        [Authorize]
        public IActionResult addNote(NoteInputModel noteModel)
        {
            try
            {
                int userId = Convert.ToInt32(User.FindFirst("Id")?.Value);
                var result = noteBL.addNote(noteModel, userId);
                if (result != null)
                {
                    responseML.Success = true;
                    responseML.Message = "Created successfully with id: " + result.Id +" "+ userId;
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
        [Route("Delete/{id}")]
        [Authorize]
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

        //[HttpGet]
        //[Route("Get/{id}")]
        //[Authorize]
        //public IActionResult GetNoteById(int id)
        //{
        //    try
        //    {
        //        var result = noteBL.getNoteById(id);

        //        if (result != null)
        //        {
        //            responseML.Success = true;
        //            responseML.Message = "Request successful for id: " + result.Id;
        //            responseML.Data = result;
        //        }
        //        return StatusCode(200, responseML);
        //    }
        //    catch (UserException ex)
        //    {
        //        Console.WriteLine(ex.ErrorCode + ": " + ex.Message);
        //        responseML.Success = false;
        //        responseML.Message = ex.ErrorCode + ": " + ex.Message;
        //        return StatusCode(202, responseML);
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //        responseML.Success = false;
        //        responseML.Message = ex.Message;
        //        return StatusCode(400, responseML);
        //    }
        //}

        //[HttpGet]
        //[Route("GetAll")]
        //[Authorize]
        //public IActionResult GetNotes()
        //{
        //    try
        //    {
        //        int userid = Convert.ToInt32(User.FindFirst("Id")?.Value);
        //        var result = noteBL.GetNotes(userid);
        //        if (result != null)
        //        {
        //            responseML.Success = true;
        //            responseML.Message = "Request successful fetch Notes" + " " + userid;
        //            responseML.Data = result;
        //        }
        //        return StatusCode(200, responseML);
        //    }
        //    catch (UserException ex)
        //    {
        //        Console.WriteLine(ex.ErrorCode + ": " + ex.Message);
        //        responseML.Success = false;
        //        responseML.Message = ex.ErrorCode + ": " + ex.Message;
        //        return StatusCode(400, responseML);
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //        responseML.Success = false;
        //        responseML.Message = ex.Message;
        //        return StatusCode(400, responseML);
        //    }
        //}

        [HttpPut]
        [Route("Update/{id}")]
        [Authorize]
        public IActionResult UpdateCustomer(int id, NoteInputModel model)
        {
            try
            {
                var result = noteBL.updateNoteById(id, model);

                if (result != null)
                {
                    responseML.Success = true;
                    responseML.Message = "Request successful for Updation";
                    responseML.Data = result;
                }
                return StatusCode(200, responseML);
            }
            catch (UserException ex)
            {
                Console.WriteLine(ex.ErrorCode + ": " + ex.Message);
                responseML.Success = false;
                responseML.Message = ex.ErrorCode + ": " + ex.Message;
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

        [HttpPut]
        [Route("Archive/{NoteId}")]
        [Authorize]
        public IActionResult Archived(int NoteId)
        {
            try
            {
                var result = noteBL.archived(NoteId);

                if (result != null)
                {
                    responseML.Success = true;
                    responseML.Message = "Request successful for Archive";
                    responseML.Data = result;
                }
                return StatusCode(200, responseML);
            }
            catch (UserException ex)
            {
                Console.WriteLine(ex.ErrorCode + ": " + ex.Message);
                responseML.Success = false;
                responseML.Message = ex.ErrorCode + ": " + ex.Message;
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

        [HttpPut]
        [Route("Trash/{NoteId}")]
        [Authorize]
        public async Task<IActionResult> Trashed(int NoteId)
        {
            try
            {
                var result = noteBL.trashed(NoteId);

                if (result != null)
                {
                    responseML.Success = true;
                    responseML.Message = "Request successful for Trash";
                    responseML.Data = result;
                    int partitionKey;
                    if(NoteId%3==0)
                    {
                        partitionKey = 0;
                    }
                    else if(NoteId%3==1)
                    {
                        partitionKey = 1;
                    }
                    else
                    {
                        partitionKey = 2;
                    }
                    await kafkaProduser.CreateMessageAsync(responseML.Message, User.FindFirst("Email")?.Value, partitionKey);
                }
                return StatusCode(200, responseML);
            }
            catch (UserException ex)
            {
                Console.WriteLine(ex.ErrorCode + ": " + ex.Message);
                responseML.Success = false;
                responseML.Message = ex.ErrorCode + ": " + ex.Message;
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

        //[HttpGet]
        //[Route("GetAllArchived")]
        //[Authorize]
        //public IActionResult GetArchivedNotes()
        //{
        //    try
        //    {
        //        int userid = Convert.ToInt32(User.FindFirst("Id")?.Value);
        //        var result = noteBL.getArchived(userid);
        //        if (result != null)
        //        {
        //            responseML.Success = true;
        //            responseML.Message = "Request successful for Archived Notes" + " " + userid;
        //            responseML.Data = result;
        //        }
        //        return StatusCode(200, responseML);
        //    }
        //    catch (UserException ex)
        //    {
        //        Console.WriteLine(ex.ErrorCode + ": " + ex.Message);
        //        responseML.Success = false;
        //        responseML.Message = ex.ErrorCode + ": " + ex.Message;
        //        return StatusCode(400, responseML);
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //        responseML.Success = false;
        //        responseML.Message = ex.Message;
        //        return StatusCode(400, responseML);
        //    }
        
        //}

        //[HttpGet]
        //[Route("GetAllTrashed")]
        //[Authorize]
        //public IActionResult GetTrashedNotes()
        //{
        //    try
        //    {
        //        int userid = Convert.ToInt32(User.FindFirst("Id")?.Value);
        //        var result = noteBL.getTrashed(userid);
        //        if (result != null)
        //        {
        //            responseML.Success = true;
        //            responseML.Message = "Request successful for Trashed Notes" + " " + userid;
        //            responseML.Data = result;
        //        }
        //        return StatusCode(200, responseML);
        //    }
        //    catch (UserException ex)
        //    {
        //        Console.WriteLine(ex.ErrorCode + ": " + ex.Message);
        //        responseML.Success = false;
        //        responseML.Message = ex.ErrorCode + ": " + ex.Message;
        //        return StatusCode(400, responseML);
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //        responseML.Success = false;
        //        responseML.Message = ex.Message;
        //        return StatusCode(400, responseML);
        //    }
        //}
    }
}
