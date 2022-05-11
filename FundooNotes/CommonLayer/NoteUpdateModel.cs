using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CommonLayer
{
    public class NoteUpdateModel
    {
        
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string colour { get; set; }

        public bool IsPin { get; set; }
        public bool IsArchieve { get; set; }
        public bool IsRemainder { get; set; }
        public bool IsTrash { get; set; }

    }
}
