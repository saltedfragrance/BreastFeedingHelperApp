using Mde.Project.Mobile.Domain.Models;
using Mde.Project.Mobile.Domain.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;

namespace Mde.Project.Mobile.Domain.Services
{
    public class UserService : IUserService
    {
        private readonly IMotherService _motherService;
        public bool IsLoggedIn { get; set; } = false;

        public UserService(IMotherService motherService)
        {
            _motherService = motherService;
        }

        public async Task<bool> Login(string email, string passWord)
        {
            var mothers = await _motherService.GetMothers();
            if (mothers.Any(m => m.Email == email && m.PassWord == passWord))
            {
                IsLoggedIn = true;
                return true;
            }
            else return false;
        }

        public Task<bool> Logout()
        {
            throw new NotImplementedException();
        }

        public async Task Register(string firstName, string lastName, string email, string passWord, int midWifePhoneNumber)
        {
            _motherService.CreateMother(firstName, lastName, email, passWord, midWifePhoneNumber);
        }
    }
}
