using Mde.Project.Mobile.Domain.Models.Base;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;

namespace Mde.Project.Mobile.Domain.Models
{
    public class Mother : BaseModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PassWord { get; set; }
        public int MidWifePhoneNumber { get; set; }
        public Location Location { get; set; }
        public ICollection<Baby> Babies { get; set; }
        public ICollection<Memory> Memories { get; set; }
    }
}
