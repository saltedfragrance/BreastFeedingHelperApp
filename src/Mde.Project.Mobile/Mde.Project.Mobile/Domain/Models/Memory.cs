using System;
using System.Collections.Generic;
using System.Text;

namespace Mde.Project.Mobile.Domain.Models
{
    public class Memory
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public Uri MediaUri { get; set; }
        public Mother Mother { get; set; }
        public Baby Baby { get; set; }
    }
}
