using CommonLayer;
using RepositoryLayer.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interfaces
{
    public interface INoteBL
    {
        Task AddNote( int userId,NotePostModel notepostmodel);
        Task<Note> UpdateNote(int userId, int noteId, NoteUpdateModel noteUpdateModel);
        Task DeleteNote(int userId, int noteId);


        Task ChangeColour(int userId, int noteId, string colour);

        Task ArchiveNote(int userId, int noteId);
        Task Trash(int userId, int noteId);
        Task Pin(int userId, int noteId);
    }
}
