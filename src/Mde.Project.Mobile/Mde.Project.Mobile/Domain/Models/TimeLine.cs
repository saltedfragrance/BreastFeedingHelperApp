using Mde.Project.Mobile.Domain.Models.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mde.Project.Mobile.Domain.Models
{
    public class TimeLine : BaseModel
    {
        public Guid MotherId { get; set; }
        public List<Event> Events { get; set; }
    }
}
 