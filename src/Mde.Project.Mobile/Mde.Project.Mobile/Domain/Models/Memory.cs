using Mde.Project.Mobile.Domain.Models.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mde.Project.Mobile.Domain.Models
{
    public class Memory : BaseModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public Guid MotherId { get; set; }
        public Mother Mother { get; set; }
        public Guid BabyId { get; set; }
        public Mother Baby { get; set; }
    }
}
