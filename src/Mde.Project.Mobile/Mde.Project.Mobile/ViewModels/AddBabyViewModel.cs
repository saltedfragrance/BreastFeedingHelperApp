using FreshMvvm;
using Mde.Project.Mobile.Domain.Models;
using Mde.Project.Mobile.Domain.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
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

        public ICommand PreviousPage => new Command(
            async () =>
            {
                await CoreMethods.PopPageModel(true, true);
            });

        public ICommand AddBaby => new Command(
            async () =>
            {
                await _babyService.CreateBaby(FirstName, Height, Weight, _motherService.CurrentMother.Id);
                PreviousPageModel.ReverseInit(new Baby());
                await CoreMethods.PopPageModel(true, true);
            });

        public ICommand DeleteBaby => new Command<Guid>(
    async (Guid id) =>
    {
        await _babyService.DeleteBaby(id);
    });
    }
}
