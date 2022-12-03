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
        private static readonly string webApiKey = "AIzaSyCwbYQx5eBLQU4ZCC6OTXyuOpwkS0iSlvM";
        FirebaseAuthProvider authProvider = new FirebaseAuthProvider(new FirebaseConfig(webApiKey));

        public bool IsLoggedIn { get; set; } = false;

        public UserService(IMotherService motherService)
        {
            _motherService = motherService;
        }

        public async Task<bool> Login(string email, string passWord)
        {
            try
            {
                FirebaseAuthLink token = await authProvider.SignInWithEmailAndPasswordAsync(email, passWord);
                var mothers = await _motherService.GetMothers();
                _motherService.CurrentMother = mothers.Where(m => m.Email == email).FirstOrDefault();
                IsLoggedIn = true;
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public Task<bool> Logout()
        {
            throw new NotImplementedException();
        }

        public async Task Register(string firstName, string lastName, string email, string passWord, int midWifePhoneNumber)
        {
            await authProvider.CreateUserWithEmailAndPasswordAsync(email, passWord);
            await _motherService.CreateMother(firstName, lastName, email, passWord, midWifePhoneNumber);
        }
    }
}
