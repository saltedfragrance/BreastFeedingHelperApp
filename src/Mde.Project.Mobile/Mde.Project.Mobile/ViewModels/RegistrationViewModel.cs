﻿using FluentValidation;
using FreshMvvm;
using Mde.Project.Mobile.Domain.Models;
using Mde.Project.Mobile.Domain.Services.Interfaces;
using Mde.Project.Mobile.Domain.Validators;
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
        private readonly IValidator _userValidator;

        public RegistrationViewModel(IUserService userService)
        {
            _userValidator = new UserValidator(true);
            _userService = userService;
        }

        private string firstName;
        public string FirstName
        {
            get { return firstName; }
            set
            {
                firstName = value;
                RaisePropertyChanged(nameof(FirstName));
            }
        }

        private string firstNameError;
        public string FirstNameError
        {
            get { return firstNameError; }
            set
            {
                firstNameError = value;
                RaisePropertyChanged(nameof(FirstNameError));
                RaisePropertyChanged(nameof(FirstNameErrorVisible));
            }
        }

        public bool FirstNameErrorVisible
        {
            get { return !string.IsNullOrWhiteSpace(FirstNameError); }
        }

        private string lastName;
        public string LastName
        {
            get { return lastName; }
            set
            {
                lastName = value;
                RaisePropertyChanged(nameof(LastName));
            }
        }
        public bool LastNameErrorVisible
        {
            get { return !string.IsNullOrWhiteSpace(LastNameError); }
        }
        private string lastNameError;
        public string LastNameError
        {
            get { return lastNameError; }
            set
            {
                lastNameError = value;
                RaisePropertyChanged(nameof(LastNameError));
                RaisePropertyChanged(nameof(LastNameErrorVisible));
            }
        }

        private string email;
        public string Email
        {
            get { return email; }
            set
            {
                email = value;
                RaisePropertyChanged(nameof(Email));
            }
        }
        private string emailError;
        public string EmailError
        {
            get { return emailError; }
            set
            {
                emailError = value;
                RaisePropertyChanged(nameof(EmailError));
                RaisePropertyChanged(nameof(EmailErrorVisible));
            }
        }
        public bool EmailErrorVisible
        {
            get { return !string.IsNullOrWhiteSpace(EmailError); }
        }

        private string passWord;
        public string PassWord
        {
            get { return passWord; }
            set
            {
                passWord = value;
                RaisePropertyChanged(nameof(PassWord));
            }
        }

        private string passWordError;
        public string PassWordError
        {
            get { return passWordError; }
            set
            {
                passWordError = value;
                RaisePropertyChanged(nameof(PassWordError));
                RaisePropertyChanged(nameof(PassWordErrorVisible));
            }
        }
        public bool PassWordErrorVisible
        {
            get { return !string.IsNullOrWhiteSpace(PassWordError); }
        }


        private string repeatPassWord;
        public string RepeatPassWord
        {
            get { return repeatPassWord; }
            set
            {
                repeatPassWord = value;
                RaisePropertyChanged(nameof(RepeatPassWord));
            }
        }

        private string repeatPassWordError;
        public string RepeatPassWordError
        {
            get { return repeatPassWordError; }
            set
            {
                repeatPassWordError = value;
                RaisePropertyChanged(nameof(RepeatPassWordError));
                RaisePropertyChanged(nameof(RepeatPassWordErrorVisible));
            }
        }

        public bool RepeatPassWordErrorVisible
        {
            get { return !string.IsNullOrWhiteSpace(RepeatPassWordError); }
        }

        private string midWifePhoneNumber;
        public string MidWifePhoneNumber
        {
            get { return midWifePhoneNumber; }
            set
            {
                midWifePhoneNumber = value;
                RaisePropertyChanged(nameof(MidWifePhoneNumber));
            }
        }

        private string midWifePhoneNumberError;
        public string MidWifePhoneNumberError
        {
            get { return midWifePhoneNumberError; }
            set
            {
                midWifePhoneNumberError = value;
                RaisePropertyChanged(nameof(MidWifePhoneNumberError));
                RaisePropertyChanged(nameof(MidWifePhoneNumberErrorVisible));
            }
        }

        public bool MidWifePhoneNumberErrorVisible
        {
            get { return !string.IsNullOrEmpty(MidWifePhoneNumberError); }
        }

        public ICommand Register => new Command(
            async () =>
            {
                var mother = new Mother { FirstName = this.FirstName, LastName = this.LastName, Email = this.Email, PassWord = this.PassWord, MidWifePhoneNumber = int.Parse(this.MidWifePhoneNumber) };
                if (Validate(mother))
                {
                    await _userService.Register(FirstName, LastName, Email, PassWord, int.Parse(MidWifePhoneNumber));
                    await CoreMethods.DisplayAlert("Success", "You can now login", "Continue");
                    await CoreMethods.PopPageModel();
                }
            });

        private bool Validate(Mother mother)
        {
            EmailError = "";
            PassWordError = "";
            RepeatPassWordError = "";
            FirstNameError = "";
            LastNameError = "";
            MidWifePhoneNumberError = "";

            var validationContext = new ValidationContext<Mother>(mother);
            var validationResult = _userValidator.Validate(validationContext);

            foreach (var error in validationResult.Errors)
            {
                if (error.PropertyName == nameof(mother.Email))
                {
                    EmailError = error.ErrorMessage;
                }
                if (error.PropertyName == nameof(mother.PassWord))
                {
                    PassWordError = error.ErrorMessage;
                }
                if (error.PropertyName == nameof(mother.FirstName))
                {
                    FirstNameError = error.ErrorMessage;
                }
                if (error.PropertyName == nameof(mother.LastName))
                {
                    LastNameError = error.ErrorMessage;
                }
                if (error.PropertyName == nameof(mother.MidWifePhoneNumber))
                {
                    MidWifePhoneNumberError = error.ErrorMessage;
                }
                if (string.IsNullOrEmpty(RepeatPassWord)) RepeatPassWordError = "Repeat password cannot be empty";
                if (RepeatPassWord != PassWord)
                {
                    PassWordError = "Passwords don't match";
                    RepeatPassWordError = "Passwords don't match";
                }
            }

            return validationResult.IsValid;
        }
    }
}
