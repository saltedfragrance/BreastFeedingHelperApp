using Acr.UserDialogs;
using FreshMvvm;
using Mde.Project.Mobile.Domain.Enums;
using Mde.Project.Mobile.Domain.Models;
using Mde.Project.Mobile.Domain.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Threading.Tasks;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Mde.Project.Mobile.ViewModels
{
    public class AddBabyViewModel : FreshBasePageModel
    {
        private readonly IMotherService _motherService;
        private readonly IBabyService _babyService;

        public AddBabyViewModel(IMotherService motherService, IBabyService babyService)
        {
            _motherService = motherService;
            _babyService = babyService;
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


        private DateTime birthDate;
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
                UserDialogs.Instance.ShowLoading("Adding baby...");
                await _babyService.CreateBaby(Guid.NewGuid().ToString(), FirstName, Height, Weight, _motherService.CurrentMother.Id.ToString(), BirthDate.ToString());
                var babies = await _babyService.GetBabies();
                await _motherService.AddEventToTimeLine($"A new baby is born! Welcome {babies.Last().FirstName}!", TimeLineCategories.AddedBabyMessage);
                await _motherService.RefreshCurrentMother();
                PreviousPageModel.ReverseInit(new Baby());
                await CoreMethods.PopPageModel(true, true);
                UserDialogs.Instance.HideLoading();
            });

        public ICommand EditBaby => new Command(
            async () =>
            {
                await _babyService.UpdateBaby(Id.ToString(), FirstName, BirthDate.ToString(), Weight, Height, _motherService.CurrentMother.Id.ToString());
                PreviousPageModel.ReverseInit(new Baby());
                await CoreMethods.PopPageModel(true, true);
            });
    }
}
