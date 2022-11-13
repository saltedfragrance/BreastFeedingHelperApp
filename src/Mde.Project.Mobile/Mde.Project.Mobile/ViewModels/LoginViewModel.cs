using FluentValidation;
using FreshMvvm;
using Mde.Project.Mobile.Domain.Models;
using Mde.Project.Mobile.Domain.Services;
using Mde.Project.Mobile.Domain.Services.Interfaces;
using Mde.Project.Mobile.Domain.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Mde.Project.Mobile.ViewModels
{
    public class LoginViewModel : FreshBasePageModel
    {
        private readonly IMotherService _motherService;
        private readonly IUserService _userService;
        private readonly IValidator userValidator;
        public LoginViewModel(IMotherService motherService, IUserService userService)
        {
            _motherService = motherService;
            userValidator = new UserValidator(false);
            _userService = userService;
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

        public bool PassWordErrorVisible
        {
            get { return !string.IsNullOrWhiteSpace(PassWordError); }
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

        protected async override void ViewIsAppearing(object sender, EventArgs e)
        {
            base.ViewIsAppearing(sender, e);
        }

        public ICommand Login => new Command(
            async () =>
            {
                if (Email != null && PassWord != null)
                {
                    List<Mother> mothers = await _motherService.GetMothers();
                    _motherService.CurrentMother = mothers.FirstOrDefault(m => m.Email == Email
                                                        && m.PassWord == PassWord);
                    if (_motherService.CurrentMother == null) _motherService.CurrentMother = new Mother { Email = this.Email, PassWord = this.PassWord };
                }
                else
                {
                    EmailError = "Credentials incorrect!";
                    PassWordError = "Credentials incorrect!";
                }

                if (_motherService.CurrentMother != null)
                {
                    if (Validate(_motherService.CurrentMother) && (await _userService.Login(Email, PassWord) == true))
                    {
                        CoreMethods.SwitchOutRootNavigation(Constants.MainContainer);
                        await CoreMethods.PushPageModel<TimeLineViewModel>();
                    }
                    else if (Validate(_motherService.CurrentMother) && (await _userService.Login(Email, PassWord) == false))
                    {
                        EmailError = "Credentials incorrect!";
                        PassWordError = "Credentials incorrect!";
                    }

                }

            });

        public ICommand RegistrationPage => new Command(
            async () =>
            {
                bool isRegistering = true;
                await CoreMethods.PushPageModel<RegistrationViewModel>(isRegistering, true);
            });

        private bool Validate(Mother mother)
        {
            EmailError = "";
            PassWordError = "";

            var validationContext = new ValidationContext<Mother>(mother);
            var validationResult = userValidator.Validate(validationContext);

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
            }

            return validationResult.IsValid;
        }
    }
}
