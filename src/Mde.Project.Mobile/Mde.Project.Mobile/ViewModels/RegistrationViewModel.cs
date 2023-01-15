using Acr.UserDialogs;
using FluentValidation;
using FreshMvvm;
using Mde.Project.Mobile.Domain.Models;
using Mde.Project.Mobile.Domain.Services.Interfaces;
using Mde.Project.Mobile.Domain.Validators;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Mde.Project.Mobile.ViewModels
{
    public class RegistrationViewModel : FreshBasePageModel
    {
        private readonly IUserService _userService;
        private readonly IValidator _userValidator;
        private readonly IMotherService _motherService;

        public RegistrationViewModel(IUserService userService, IMotherService motherService)
        {
            _userValidator = new UserValidator(true);
            _userService = userService;
            _motherService = motherService;
        }

        private bool isRegistering;
        public bool IsRegistering
        {
            get { return isRegistering; }
            set
            {
                isRegistering = value;
                RaisePropertyChanged(nameof(IsRegistering));
            }
        }

        private bool isEditingAccount;
        public bool IsEditingAccount
        {
            get { return isEditingAccount; }
            set
            {
                isEditingAccount = value;
                RaisePropertyChanged(nameof(IsEditingAccount));
            }
        }

        private string pageTitle;
        public string PageTitle
        {
            get { return pageTitle; }
            set
            {
                pageTitle = value;
                RaisePropertyChanged(nameof(PageTitle));
            }
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

        private bool onAndroid;
        public bool OnAndroid
        {
            get { return onAndroid; }
            set
            {
                onAndroid = value;
                RaisePropertyChanged(nameof(OnAndroid));
            }
        }

        private bool onUwp;

        public bool OnUwp
        {
            get { return onUwp; }
            set
            {
                onUwp = value;
                RaisePropertyChanged(nameof(OnUwp));
            }
        }

        public async override void Init(object initData)
        {
            if (HelperMethods.CheckOs()) OnAndroid = true;
            else OnUwp = true;

            if (initData != null)
            {
                IsRegistering = true;
                IsEditingAccount = false;
            }
            else
            {
                IsEditingAccount = true;
                isRegistering = false;
                if (_userService.IsLoggedIn)
                {
                    PopulateControls();
                }
            }
            base.Init(initData);

            if (isEditingAccount) PageTitle = "Account overview";
            else PageTitle = "Registration page";
        }

        private void PopulateControls()
        {
            FirstName = _motherService.CurrentMother.FirstName;
            LastName = _motherService.CurrentMother.LastName;
            Email = _motherService.CurrentMother.Email;
            PassWord = _motherService.CurrentMother.PassWord;
            RepeatPassWord = _motherService.CurrentMother.PassWord;
            MidWifePhoneNumber = _motherService.CurrentMother.MidWifePhoneNumber.ToString();

        }

        public ICommand RegisterOrUpdate => new Command(
            async () =>
            {
                var mother = new Mother
                {
                    FirstName = FirstName,
                    LastName = LastName,
                    Email = Email,
                    PassWord = PassWord,
                    MidWifePhoneNumber = (MidWifePhoneNumber != null ? int.Parse(MidWifePhoneNumber) : 0),
                };
                if (Validate(mother) && !_userService.IsLoggedIn)
                {
                    if (OnAndroid)
                    {
                        UserDialogs.Instance.ShowLoading("Registering account...");
                    }
                    await _userService.Register(FirstName, LastName, Email, PassWord, int.Parse(MidWifePhoneNumber));

                    if (OnAndroid)
                    {
                        UserDialogs.Instance.HideLoading();
                    }
                    await CoreMethods.DisplayAlert("Success", "You can now login", "Continue");
                    await CoreMethods.PopPageModel(true, true);
                }
                else if (Validate(mother) && _userService.IsLoggedIn)
                {
                    if (OnAndroid)
                    {
                        UserDialogs.Instance.ShowLoading("Updating account...");
                    }
                    mother.Id = _motherService.CurrentMother.Id;
                    mother.TimeLineId = _motherService.CurrentMother.TimeLineId;
                    await _motherService.UpdateMother(_motherService.CurrentMother.Id.ToString(), mother);
                    PopulateControls();
                    if (OnAndroid)
                    {
                        UserDialogs.Instance.HideLoading();
                    }
                    await CoreMethods.DisplayAlert("Success", "Account updated", "Continue");
                    await CoreMethods.PopPageModel(true, true);
                }
            });
        public ICommand PreviousPage => new Command(
            async () =>
            {
                await CoreMethods.PopPageModel(true, true);
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
