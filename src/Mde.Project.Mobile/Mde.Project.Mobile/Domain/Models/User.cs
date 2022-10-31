using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;

namespace Mde.Project.Mobile.Domain.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PassWord { get; set; }
        public int MidWifePhoneNumber { get; set; }
        public Location Location { get; set; }
    }
}
