using Mde.Project.Mobile.Domain.Models.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mde.Project.Mobile.Domain.Models
{
    public class Baby : BaseModel
    {
        public string FirstName { get; set; }
        public double Height { get; set; }
        public double Weight { get; set; }
        public Guid MotherId { get; set; }
        public Mother Mother { get; set; }
        public DateTime DateOfBirth { get; set; }
        public ICollection<Memory> Memories { get; set; }
    }
}
