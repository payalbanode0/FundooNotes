using BusinessLayer.Interfaces;
using CommonLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using RepositoryLayer.Entities;
using RepositoryLayer.FundooContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FundooNotes.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class NoteController : ControllerBase
    {
        FundooDBContext fundooDBContext;
        INoteBL noteBL;
        private readonly IMemoryCache memoryCache;
        private readonly IDistributedCache distributedCache;

        public NoteController(INoteBL noteBL, FundooDBContext fundooDBContext, IMemoryCache memoryCache, IDistributedCache distributedCache)
        {
            this.noteBL = noteBL;
            this.fundooDBContext = fundooDBContext;
            this.memoryCache = memoryCache;
            this.distributedCache = distributedCache;

        }
        [Authorize]
        [HttpPost("AddNote")]
        public async Task<ActionResult> AddNote(NotePostModel notePostModel)
        {
            try
            {
                var userid = User.Claims.FirstOrDefault(x => x.Type.ToString().Equals("userID", StringComparison.InvariantCultureIgnoreCase));
                int UserId = Int32.Parse(userid.Value);

                await this.noteBL.AddNote(UserId, notePostModel);
                return this.Ok(new { success = true, message = "Note Added Successfully " });
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        [Authorize]
        [HttpPut("Update/{noteId}")]
        public async Task<ActionResult> UpdateNote(int noteId, NoteUpdateModel noteUpdateModel)
        {
            try
            {
                var userid = User.Claims.FirstOrDefault(x => x.Type.ToString().Equals("userID", StringComparison.InvariantCultureIgnoreCase));
                int UserId = Int32.Parse(userid.Value);

                var note = fundooDBContext.Notes.FirstOrDefault(u => u.UserId == UserId && u.NoteId == noteId);
                if (note == null)
                {
                    return this.BadRequest(new { success = false, message = "Failed to Update note" });
                }
                await this.noteBL.UpdateNote(UserId, noteId, noteUpdateModel);
                return this.Ok(new { success = true, message = "Note Updated successfully!!!" });
            }
            catch (Exception)
            {

                throw;
            }
        }

        [Authorize]
        [HttpDelete("Delete/{noteId}")]
        public async Task<ActionResult> DeleteNote(int noteId)
        {
            try
            {
                var userid = User.Claims.FirstOrDefault(x => x.Type.ToString().Equals("userID", StringComparison.InvariantCultureIgnoreCase));
                int UserId = Int32.Parse(userid.Value);
                var note = fundooDBContext.Notes.FirstOrDefault(u => u.UserId == UserId && u.NoteId == noteId);
                if (note == null)
                {
                    return this.BadRequest(new { success = false, message = "Oops! This note is not available " });

                }
                await this.noteBL.DeleteNote(noteId, UserId);
                return this.Ok(new { success = true, message = "Note Deleted Successfully" });
            }
            catch (Exception)
            {

                throw;
            }
        }

        [Authorize]
        [HttpPut("ChangeColour/{noteId}/{colour}")]
        public async Task<ActionResult> ChangeColour(int noteId, string colour)
        {
            try
            {
                var userid = User.Claims.FirstOrDefault(x => x.Type.ToString().Equals("userID", StringComparison.InvariantCultureIgnoreCase));
                int UserId = Int32.Parse(userid.Value);

                var note = fundooDBContext.Notes.FirstOrDefault(u => u.UserId == UserId && u.NoteId == noteId);
                if (note == null)
                {
                    return this.BadRequest(new { success = false, message = "Sorry! Note does not exist" });
                }

                await this.noteBL.ChangeColour(UserId, noteId, colour);
                return this.Ok(new { success = true, message = "Note Colour Changed Successfully " });
            }
            catch (Exception)
            {

                throw;
            }
        }

        [Authorize]
        [HttpPut("ArchieveNote/{noteId}")]
        public async Task<ActionResult> IsArchieveNote(int noteId)
        {
            try
            {
                var userid = User.Claims.FirstOrDefault(x => x.Type.ToString().Equals("userID", StringComparison.InvariantCultureIgnoreCase));
                int userId = Int32.Parse(userid.Value);

                var note = fundooDBContext.Notes.FirstOrDefault(u => u.UserId == userId && u.NoteId == noteId);
                if (note == null)
                {
                    return this.BadRequest(new { success = false, message = "Failed to archieve note or Id does not exists" });
                }
                await this.noteBL.ArchiveNote(userId, noteId);
                return this.Ok(new { success = true, message = "Note Archieved successfully!!!" });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [Authorize]
        [HttpPut("IsTrash/{noteId}")]
        public async Task<ActionResult> IsTrash(int noteId)
        {
            try
            {
                var userid = User.Claims.FirstOrDefault(x => x.Type.ToString().Equals("userID", StringComparison.InvariantCultureIgnoreCase));
                int userId = Int32.Parse(userid.Value);

                var note = fundooDBContext.Notes.FirstOrDefault(u => u.UserId == userId && u.NoteId == noteId);
                if (note == null)
                {
                    return this.BadRequest(new { success = false, message = "Failed to Trash note" });
                }
                await this.noteBL.Trash(userId, noteId);
                return this.Ok(new { success = true, message = "Trash successfully!!!" });
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        [Authorize]
        [HttpPut("IsPin/{noteId}")]
        public async Task<ActionResult> IsPin(int noteId)
        {
            try
            {
                var userid = User.Claims.FirstOrDefault(x => x.Type.ToString().Equals("userID", StringComparison.InvariantCultureIgnoreCase));
                int userId = Int32.Parse(userid.Value);

                var note = fundooDBContext.Notes.FirstOrDefault(u => u.UserId == userId && u.NoteId == noteId);
                if (note == null)
                {
                    return this.BadRequest(new { success = false, message = "Failed to Pin note" });
                }
                await this.noteBL.Pin(userId, noteId);
                return this.Ok(new { success = true, message = "Pin Add successfully!!!" });
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        [Authorize]
        [HttpPut("Reminder/{noteId}/{ReminderDate}")]
        public async Task<ActionResult> IsReminder(int userId, int noteId, DateTime ReminderDate)
        {
            try
            {
                var userid = User.Claims.FirstOrDefault(x => x.Type.ToString().Equals("userID", StringComparison.InvariantCultureIgnoreCase));
                int userID = Int32.Parse(userid.Value);
                var note = fundooDBContext.Notes.FirstOrDefault(u => u.UserId == userId && u.NoteId == noteId);
                if (note == null)
                {
                    return this.BadRequest(new { success = false, message = "Failed to Set ReminderDate or Id does not exists" });
                }
                await this.noteBL.Remainder(userId, noteId, ReminderDate);
                return this.Ok(new { success = true, message = "ReminderDate is set successfully!!!" });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Authorize]
        [HttpGet("GetAllNote")]
        public async Task<ActionResult> GetAllNote()
        {
            try
            {
                var userid = User.Claims.FirstOrDefault(x => x.Type.ToString().Equals("userID", StringComparison.InvariantCultureIgnoreCase));
                int userId = Int32.Parse(userid.Value);
                List<Note> result = new List<Note>();
                result = await this.noteBL.GetNote(userId);
                return this.Ok(new { success = true, message = $"Below are all notes", data = result });
            }
            catch (Exception ex)
            {
                throw ex;

            }



        }
        [Authorize]
        [HttpGet("GetAllNotesRedis")]
        public async Task<ActionResult> GetNote()
        {
            try
            {
                string serializeNoteList;
                string key = "Notelist1";
                var noteList = new List<Note>();
                var redisNoteList = await distributedCache.GetAsync(key);
                if (redisNoteList != null)
                {
                    serializeNoteList = Encoding.UTF8.GetString(redisNoteList);
                    noteList = JsonConvert.DeserializeObject<List<Note>>(serializeNoteList);
                }
                else
                {
                    var userid = User.Claims.FirstOrDefault(x => x.Type.ToString().Equals("userID", StringComparison.InvariantCultureIgnoreCase));
                    int userID = Int32.Parse(userid.Value);
                    noteList = await this.noteBL.GetNote(userID);
                    serializeNoteList = JsonConvert.SerializeObject(noteList);
                    redisNoteList = Encoding.UTF8.GetBytes(serializeNoteList);
                    var option = new DistributedCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(20)).SetAbsoluteExpiration(TimeSpan.FromHours(6));
                    
                    await distributedCache.SetAsync(key, redisNoteList, option);

                    
                }
                return this.Ok(new { success = true, message = "Get note successful!!!", data = noteList });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



    }
            
}
    

