using System;
using System.Collections.Generic;
using System.Text;

namespace Mde.Project.Mobile.Domain.Models
{
    public class Memory
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public Uri MediaUri { get; set; }
        public Mother Mother { get; set; }
        public ICollection<Baby> Baby { get; set; }
    }
}
