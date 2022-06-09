using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RepositoryLayer.Entities;
using RepositoryLayer.FundooContext;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Services
{
    public class LabelRL : ILabelRL

    {


        FundooDBContext fundooDBContext;
        public IConfiguration Configuration { get; }

        //Creating constructor for initialization
        public LabelRL(FundooDBContext fundooDBContext, IConfiguration configuration)
        {
            this.fundooDBContext = fundooDBContext;
            this.Configuration = configuration;
        }

        public async Task AddLabel(int userId, int noteId, string LabelName)
        {
            try
            {
                var user = fundooDBContext.Users.FirstOrDefault(u => u.UserId == userId);
                var note = fundooDBContext.Notes.FirstOrDefault(b => b.NoteId == noteId);
                Label label = new Label
                {
                    User = user,
                    note = note
                };
                label.LabelName = LabelName;
                fundooDBContext.Labels.Add(label);
                await fundooDBContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        

        public async Task<Label> UpdateLabel(int userId, int LabelId, string LabelName)
        {

            try
            {

                Entities.Label reuslt = fundooDBContext.Labels.FirstOrDefault(u => u.LabelId == LabelId && u.UserId == userId);

                if (reuslt != null)
                {
                    reuslt.LabelName = LabelName;
                    await fundooDBContext.SaveChangesAsync();
                    var result = fundooDBContext.Labels.Where(u => u.LabelId == LabelId).FirstOrDefault();
                    return result;
                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task DeleteLabel(int LabelId, int userId)
        {
            try
            {
                var result = fundooDBContext.Labels.FirstOrDefault(u => u.LabelId == LabelId && u.UserId == userId);
                fundooDBContext.Labels.Remove(result);
                await fundooDBContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<Label>> GetLabel(int userId)
        {
            try
            {
                List<Label> reuslt = await fundooDBContext.Labels.Where(u => u.UserId == userId).Include(u => u.User).Include(u => u.User).ToListAsync();
                
                return reuslt;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task createLabel(int userId, string LabelName)
        {
            try
            {
                var user = fundooDBContext.Users.FirstOrDefault(u => u.UserId == userId);
                
                Label label = new Label
                {
                    User = user,
                   
                };
                label.LabelName = LabelName;
                fundooDBContext.Labels.Add(label);
                await fundooDBContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

}
    







