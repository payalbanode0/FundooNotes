using CommonLayer;
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
                //note.NoteId = new Note().NoteId;
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

        public async Task DeleteNote(int noteId, int userId)
        {
            try
            {
                var note = fundooDBContext.Notes.FirstOrDefault(u => u.NoteId == noteId && u.UserId == userId);
                if (note != null)
                {
                    fundooDBContext.Notes.Remove(note);
                    await fundooDBContext.SaveChangesAsync();

                }


            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task ChangeColour(int userId, int noteId, string colour)
        {
            try
            {
                var note = fundooDBContext.Notes.FirstOrDefault(u => u.UserId == userId && u.NoteId == noteId);
                if (note != null)
                {
                    note.colour = colour;
                    await fundooDBContext.SaveChangesAsync();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task ArchiveNote(int userId, int noteId)
        {
            try
            {
                var note = fundooDBContext.Notes.FirstOrDefault(u => u.UserId == userId && u.NoteId == noteId);
                if (note != null)
                {
                    if (note.IsArchieve == true)
                    {
                        note.IsArchieve = false;
                    }

                    if (note.IsArchieve == false)
                    {
                        note.IsArchieve = true;
                    }
                }

                await fundooDBContext.SaveChangesAsync();

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<Note> UpdateNote(int userId, int noteId, NoteUpdateModel noteUpdateModel)
        {
            try
            {
                var note = fundooDBContext.Notes.FirstOrDefault(u => u.UserId == userId && u.NoteId == noteId);
                if (note != null)
                {
                    note.Title = noteUpdateModel.Title;
                    note.Description = noteUpdateModel.Description;
                    note.IsArchieve = noteUpdateModel.IsArchieve;
                    note.colour = noteUpdateModel.colour;
                    note.IsPin = noteUpdateModel.IsPin;
                    note.IsRemainder = noteUpdateModel.IsRemainder;
                    note.IsTrash = noteUpdateModel.IsTrash;

                    await fundooDBContext.SaveChangesAsync();

                }
                return await fundooDBContext.Notes
                .Where(u => u.UserId == u.UserId && u.NoteId == noteId)
                .Include(u => u.User)
                .FirstOrDefaultAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task Trash(int userId, int noteId)
        {
            try
            {
                var note = fundooDBContext.Notes.FirstOrDefault(u => u.UserId == userId && u.NoteId == noteId);
                if (note != null)
                {
                    if (note.IsTrash == true)
                    {
                        note.IsTrash = false;
                    }

                    if (note.IsTrash == false)
                    {
                        note.IsTrash = true;
                    }
                }

                await fundooDBContext.SaveChangesAsync();

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task Pin(int userId, int noteId)
        {
            try
            {
                var note = fundooDBContext.Notes.FirstOrDefault(u => u.UserId == userId && u.NoteId == noteId);
                if (note != null)
                {
                    if (note.IsPin == true)
                    {
                        note.IsPin = false;
                    }

                    if (note.IsPin == false)
                    {
                        note.IsPin = true;
                    }
                }

                await fundooDBContext.SaveChangesAsync();

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task Remainder(int userId, int noteId, DateTime remainder)
        {
            try
            {
                var note = fundooDBContext.Notes.FirstOrDefault(u => u.UserId == userId && u.NoteId == noteId);
                if (note != null)
                {
                    if (note.IsRemainder == true)
                    {
                        note.RemainderDate = remainder;
                    }


                }
                await fundooDBContext.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}