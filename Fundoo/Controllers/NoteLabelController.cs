using BusinessLayer.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using ModelLayer;
using RepositoryLayer.CustomException;
using RepositoryLayer.Entities;

namespace Fundoo.Controllers
{
    [Route("api/NoteLabel")]
    [ApiController]
    public class NoteLabelController : ControllerBase
    {
        private readonly INoteLabelBL noteLabelBL;
        private readonly ResponseML responseML;

        public NoteLabelController(INoteLabelBL noteLabelBL)
        {
            this.noteLabelBL = noteLabelBL;
            responseML = new ResponseML();
        }

        [HttpPost("AddNoteToLabel")]
        [Authorize]
        public IActionResult AddLabelToNote(NoteLabel nl)
        {
            try
            {
                var result = noteLabelBL.AddLabelToNote(nl);

                responseML.Success = true;
                responseML.Message = "Label added to note successfully";
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

        [HttpGet("getnotesbylabel/{LabelID}")]
        [Authorize]
        public IActionResult GetNotesFromLabel(int LabelID)
        {
            try
            {
                var result = noteLabelBL.GetNotesFromLabel(LabelID);

                responseML.Success = true;
                responseML.Message = "Notes fetched successfully";
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

        [HttpGet("getlabelsbyNote/{NoteID}")]
        [Authorize]
        public IActionResult GetLabelsFromNote(int NoteID)
        {
            try
            {
                var result = noteLabelBL.GetLabelsFromNote(NoteID);

                responseML.Success = true;
                responseML.Message = "Label fetched successfully";
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

        [HttpDelete("deleteLabelFromNote")]
        [Authorize]
        public IActionResult RemoveLabelFromNote(NoteLabel model)
        {
            try
            {
                var result = noteLabelBL.RemoveLabelFromNote(model);

                responseML.Success = true;
                responseML.Message = "Label removed from note successfully";
                responseML.Data = result;

                return StatusCode(200, responseML);
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

        [HttpGet("getnoteswithlabels")]
        [Authorize]
        public async Task<IActionResult> GetNotesWithLabels()
        {
            try
            {
                int userid = Convert.ToInt32(User.FindFirst("Id")?.Value);
                var result = await noteLabelBL.getNotesWithLabels(userid);

                responseML.Success = true;
                responseML.Message = "Notes including Label fetched successfully";
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

        [HttpGet("getTrashedwithlabels")]
        [Authorize]
        public IActionResult GetTrashedWithLabels()
        {
            try
            {
                int userid = Convert.ToInt32(User.FindFirst("Id")?.Value);
                var result = noteLabelBL.getTrashedWithLabels(userid);

                responseML.Success = true;
                responseML.Message = "Notes including Label fetched successfully";
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

        [HttpGet("getArchivedwithlabels")]
        [Authorize]
        public IActionResult GetArchivedWithLabels()
        {
            try
            {
                int userid = Convert.ToInt32(User.FindFirst("Id")?.Value);
                var result = noteLabelBL.getArchivedWithLabels(userid);

                responseML.Success = true;
                responseML.Message = "Notes including Label fetched successfully";
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

        [HttpGet]
        [Route("getNotewithlabelsByNoteId/{id}")]
        [Authorize]
        public IActionResult GetNoteWithLabelsByNoteId(int id)
        {
            try
            {
                var result = noteLabelBL.getNoteWithLabelsByNoteId(id);

                responseML.Success = true;
                responseML.Message = "Notes including Label fetched successfully";
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
    }
}