using Mde.Project.Mobile.Domain.Models.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mde.Project.Mobile.Domain.Models
{
    public class Reminder : BaseModel
    {
        public string MotherId { get; set; }
        public string IntervalTime { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public string Type { get; set; }
    }
}
