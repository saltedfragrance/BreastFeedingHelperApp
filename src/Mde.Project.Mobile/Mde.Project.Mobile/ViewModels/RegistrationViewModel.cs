using FreshMvvm;
using Mde.Project.Mobile.Domain.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Mde.Project.Mobile.ViewModels
{
    public class RegistrationViewModel : FreshBasePageModel
    {
        private readonly IUserService _userService;

        public RegistrationViewModel(IUserService userService)
        {
            _userService = userService;
        }

        private string firstName;

        public string FirstName
        {
            get { return firstName; }
            set { firstName = value; }
        }

        private string lastName;

        public string LastName
        {
            get { return lastName; }
            set { lastName = value; }
        }

        private string email;

        public string Email
        {
            get { return email; }
            set { email = value; }
        }

        private string passWord;

        public string PassWord
        {
            get { return passWord; }
            set { passWord = value; }
        }

        private int midWifePhoneNumber;

        public int MidWifePhoneNumber
        {
            get { return midWifePhoneNumber; }
            set { midWifePhoneNumber = value; }
        }

        public ICommand Register => new Command(
            async () =>
            {
                await _userService.Register(FirstName, LastName, Email, PassWord, MidWifePhoneNumber);

            });
    }
}
