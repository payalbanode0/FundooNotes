using BusinessLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RepositoryLayer.FundooContext;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FundooNotes.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class LabelController : ControllerBase
    {
        FundooDBContext fundooDBContext;
        ILabelBL labelBL;
        public LabelController(ILabelBL labelBL, FundooDBContext fundooDBContext)
        {
            this.labelBL = labelBL;
            this.fundooDBContext = fundooDBContext;
        }

        [Authorize]
        [HttpPost("AddLabel/{NoteId}/{LabelName}")]
        public async Task<ActionResult> AddLabel(int NoteId, string LabelName)
        {
            try
            {
                var userid = User.Claims.FirstOrDefault(x => x.Type.ToString().Equals("userId", StringComparison.InvariantCultureIgnoreCase));
                int userId = Int32.Parse(userid.Value);
                await this.labelBL.AddLabel(userId, NoteId, LabelName);
                return this.Ok(new { success = true, message = $"Label added successfully" });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [Authorize]
        [HttpPut("UpdateLabel/{LabelId}/{LabelName}")]
        public async Task<ActionResult> UpdateLabel(string LabelName, int LabelId)
        {
            try
            {
                var userid = User.Claims.FirstOrDefault(x => x.Type.ToString().Equals("userId", StringComparison.InvariantCultureIgnoreCase));
                int userId = Int32.Parse(userid.Value);
                var result = await this.labelBL.UpdateLabel(userId, LabelId, LabelName);
                if (result == null)
                {
                    return this.BadRequest(new { success = false, message = " Label failed" });
                }
                return this.Ok(new { success = true, message = $"Label updated successfully", data = result });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }




        [Authorize]
        [HttpDelete("DeleteLabel/{LabelId}")]
        public async Task<ActionResult> DeleteLabel(int LabelId)
        {
            try
            {
                var userid = User.Claims.FirstOrDefault(x => x.Type.ToString().Equals("userId", StringComparison.InvariantCultureIgnoreCase));
                int userId = Int32.Parse(userid.Value);
                await this.labelBL.DeleteLabel(LabelId, userId);
                return this.Ok(new { success = true, message = $"Label Deleted successfully" });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}

