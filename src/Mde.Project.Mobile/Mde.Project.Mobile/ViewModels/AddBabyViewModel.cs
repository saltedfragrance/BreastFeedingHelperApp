using Acr.UserDialogs;
using FluentValidation;
using FreshMvvm;
using Mde.Project.Mobile.Domain.Enums;
using Mde.Project.Mobile.Domain.Models;
using Mde.Project.Mobile.Domain.Services.Interfaces;
using Mde.Project.Mobile.Domain.Validators;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Threading.Tasks;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Mde.Project.Mobile.ViewModels
{
    public class AddBabyViewModel : FreshBasePageModel
    {
        private readonly IMotherService _motherService;
        private readonly IBabyService _babyService;
        private readonly IValidator _babyValidator;

        public AddBabyViewModel(IMotherService motherService, IBabyService babyService)
        {
            _motherService = motherService;
            _babyService = babyService;
            _babyValidator = new BabyValidator();
        }

        private string pageTitle;
        public string PageTitle
        {
            get { return pageTitle; }
            set
            {
                pageTitle = value;
                RaisePropertyChanged(nameof(pageTitle));
            }
        }

        private string weightError;

        public string WeightError
        {
            get { return weightError; }
            set
            {
                weightError = value;
                RaisePropertyChanged(nameof(WeightError));
                RaisePropertyChanged(nameof(WeightErrorVisible));
            }
        }

        public bool WeightErrorVisible
        {
            get { return !string.IsNullOrWhiteSpace(WeightError); }
        }

        private string heightError;

        public string HeightError
        {
            get { return heightError; }
            set
            {
                heightError = value;
                RaisePropertyChanged(nameof(HeightError));
                RaisePropertyChanged(nameof(HeightErrorVisible));
            }
        }

        public bool HeightErrorVisible
        {
            get { return !string.IsNullOrWhiteSpace(HeightError); }
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

        private bool edit;
        public bool Edit
        {
            get { return edit; }
            set
            {
                edit = value;
                RaisePropertyChanged(nameof(Edit));
            }
        }


        private string birthDateError;

        public string BirthDateError
        {
            get { return birthDateError; }
            set
            {
                birthDateError = value;
                RaisePropertyChanged(nameof(BirthDateError));
                RaisePropertyChanged(nameof(BirthDateErrorVisible));
            }
        }

        public bool BirthDateErrorVisible
        {
            get { return !string.IsNullOrWhiteSpace(BirthDateError); }
        }


        private bool add;
        public bool Add
        {
            get { return add; }
            set
            {
                add = value;
                RaisePropertyChanged(nameof(Add));
            }
        }

        private Guid? id;

        public Guid? Id
        {
            get { return id; }
            set { id = value; }
        }


        private DateTime birthDate = DateTime.Today;
        public DateTime BirthDate
        {
            get
            {
                return birthDate;
            }
            set
            {
                birthDate = value;
                RaisePropertyChanged(nameof(BirthDate));
            }
        }

        private string firstName;
        public string FirstName
        {
            get
            {
                return firstName;
            }
            set
            {
                firstName = value;
                RaisePropertyChanged(nameof(FirstName));
            }
        }

        private double weight;
        public double Weight
        {
            get
            {
                return weight;
            }
            set
            {
                weight = value;
                RaisePropertyChanged(nameof(Weight));
            }
        }

        private double height;
        public double Height
        {
            get
            {
                return height;
            }
            set
            {
                height = value;
                RaisePropertyChanged(nameof(Height));
            }
        }


        public async override void Init(object initData)
        {
            if (initData != null)
            {
                Add = false;
                Edit = true;
                var id = initData as Guid?;
                Id = id;
                var baby = await _babyService.GetBaby(Id.ToString());
                BirthDate = baby.DateOfBirth;
                FirstName = baby.FirstName;
                Weight = baby.Weight;
                Height = baby.Height;

            }
            else
            {
                Add = true;
                Edit = false;
            }

            base.Init(initData);
            PageTitle = "Add baby";
        }

        public ICommand PreviousPage => new Command(
            async () =>
            {
                await CoreMethods.PopPageModel(true, true);
            });

        public ICommand AddBaby => new Command(
            async () =>
            {
                Baby babyToAdd = new Baby { FirstName = FirstName, DateOfBirth = BirthDate, Weight = Weight, Height = Height };
                if (Validate(babyToAdd))
                {
                    UserDialogs.Instance.ShowLoading("Adding baby...");
                    await _babyService.CreateBaby(Guid.NewGuid().ToString(), FirstName, Height, Weight, _motherService.CurrentMother.Id.ToString(), BirthDate.ToString());
                    var babies = await _babyService.GetBabies();
                    await _motherService.AddEventToTimeLine($"A new baby is born! Welcome {babies.Last().FirstName}!", TimeLineCategories.AddedBabyMessage);
                    await _motherService.RefreshCurrentMother();
                    PreviousPageModel.ReverseInit(new Baby());
                    await CoreMethods.PopPageModel(true, true);
                    UserDialogs.Instance.HideLoading();
                }
            });

        public ICommand EditBaby => new Command(
            async () =>
            {
                Baby babyToUpdate = new Baby { FirstName = FirstName, DateOfBirth = BirthDate, Weight = Weight, Height = Height };
                if (Validate(babyToUpdate))
                {
                    await _babyService.UpdateBaby(Id.ToString(), FirstName, BirthDate.ToString(), Weight, Height, _motherService.CurrentMother.Id.ToString());
                    PreviousPageModel.ReverseInit(new Baby());
                    await CoreMethods.PopPageModel(true, true);
                }
            });

        private bool Validate(Baby baby)
        {
            FirstNameError = "";
            BirthDateError = "";
            WeightError = "";
            HeightError = "";

            var validationContext = new ValidationContext<Baby>(baby);
            var validationResult = _babyValidator.Validate(validationContext);

            foreach (var error in validationResult.Errors)
            {
                if (error.PropertyName == nameof(baby.FirstName))
                {
                    FirstNameError = error.ErrorMessage;
                }
                if (error.PropertyName == nameof(baby.DateOfBirth))
                {
                    BirthDateError = error.ErrorMessage;
                }
                if (error.PropertyName == nameof(baby.Weight))
                {
                    WeightError = error.ErrorMessage;
                }
                if (error.PropertyName == nameof(baby.Height))
                {
                    HeightError = error.ErrorMessage;
                }
            }

            return validationResult.IsValid;
        }
    }
}
