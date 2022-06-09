using RepositoryLayer.Entities;
using System;
using System.Collections.Generic;

using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Interfaces
{
    public interface ILabelRL
    {
        Task AddLabel(int userId, int noteId, string LabelName);
        Task<List<Label>> GetLabel(int userId);
        
        Task<Label> UpdateLabel(int userId, int LabelId, string LabelName);
        Task DeleteLabel(int LabelId, int userId);
        Task createLabel(int userId,  string LabelName);

    }
}
