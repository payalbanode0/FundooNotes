using CommonLayer;
using Microsoft.Extensions.Configuration;
using RepositoryLayer.Entities;
using RepositoryLayer.FundooContext;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Services
{
    public class NoteRL : INoteRL
    {
        //instances of fundoocontext message
        FundooDBContext fundooDBContext;

        public IConfiguration configuration { get; }
        public NoteRL(FundooDBContext fundooDBContext, IConfiguration configuration)
        {
            this.fundooDBContext = fundooDBContext;
            this.configuration = configuration;

        }
        public async Task AddNote(int userId, NotePostModel notepostmodel)
        {
            try
            {
                Note note = new Note();
                note.NoteId = new Note().NoteId;
                note.UserId = userId;
                note.Title = notepostmodel.Title;
                note.Description = notepostmodel.Description;
                note.colour = notepostmodel.colour;
                note.IsPin = false;
                note.IsArchieve = false;
                note.IsRemainder = false;
                note.IsTrash = false;
                note.RegisterDate = DateTime.Now;
                note.ModifyDate = DateTime.Now;
                fundooDBContext.Add(note);
                await fundooDBContext.SaveChangesAsync();

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
