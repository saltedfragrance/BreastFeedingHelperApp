using Mde.Project.Mobile.Domain.Enums;
using Mde.Project.Mobile.Domain.Models.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mde.Project.Mobile.Domain.Models
{
    public class Event : BaseModel
    {
        public string Description { get; set; }
        public TimeLineCategories Category { get; set; }
        public string Image { get; set; }
        public DateTime Date { get; set; }
    }
}
