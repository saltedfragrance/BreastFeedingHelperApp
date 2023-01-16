using Firebase.Auth;
using Firebase.Database;
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
        private readonly IFireBaseService _fireBaseService;

        public bool IsLoggedIn { get; set; } = false;
        public User User { get; set; }

        public UserService(IMotherService motherService, IFireBaseService fireBaseService)
        {
            _motherService = motherService;
            _fireBaseService = fireBaseService;
        }

        public async Task<bool> Login(string email, string passWord)
        {
            try
            {
                FirebaseAuthLink response = await _fireBaseService.AuthProvider.SignInWithEmailAndPasswordAsync(email, passWord);
                User = response.User;
                var mothers = await _motherService.GetMothers();
                _motherService.CurrentMother = mothers.Where(m => m.Email == email).FirstOrDefault();
                IsLoggedIn = true;
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        public Task<bool> Logout()
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Register(string firstName, string lastName, string email, string passWord, int midWifePhoneNumber, bool isUpdating = false)
        {
            try
            {
                if (isUpdating)
                {
                    if (!(await _motherService.GetMothers()).Any(m => m.Email == email)) await _fireBaseService.AuthProvider.ChangeUserEmail(User.LocalId, email);
                    await _fireBaseService.AuthProvider.ChangeUserPassword(User.LocalId, passWord);
                }
                else await _fireBaseService.AuthProvider.CreateUserWithEmailAndPasswordAsync(email, passWord);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("EMAIL_EXISTS")) return false;
            }

            await _motherService.CreateMother(firstName, lastName, email, passWord, midWifePhoneNumber);
            return true;
        }
    }
}
