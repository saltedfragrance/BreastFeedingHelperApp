using Mde.Project.Mobile.Domain.Models.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mde.Project.Mobile.Domain.Models
{
    public class Baby : BaseModel
    {
        public string FirstName { get; set; }
        public int Height { get; set; }
        public int Weight { get; set; }
        public ICollection<Memory> Memories { get; set; }
    }
}
