using FreshMvvm;
using Mde.Project.Mobile.Domain.Models;
using Mde.Project.Mobile.Domain.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Mde.Project.Mobile.ViewModels
{
    public class BabyViewModel : FreshBasePageModel
    {
        private readonly IBabyService _babyService;
        private readonly IMotherService _motherService;

        public BabyViewModel(IBabyService babyService, IMotherService motherService)
        {
            _babyService = babyService;
            _motherService = motherService;
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

        private ObservableCollection<Baby> babies;

        public ObservableCollection<Baby> Babies
        {
            get { return babies; }
            set
            {
                babies = value;
                RaisePropertyChanged(nameof(Babies));
            }
        }

        private bool hasBabies;

        public bool HasBabies
        {
            get { return hasBabies; }
            set
            {
                hasBabies = value;
                RaisePropertyChanged(nameof(HasBabies));
            }
        }

        private bool hasNoBabies;

        public bool HasNoBabies
        {
            get { return hasNoBabies; }
            set
            {
                hasNoBabies = value;
                RaisePropertyChanged(nameof(HasNoBabies));
            }
        }

        public override void Init(object initData)
        {
            PageTitle = "Babies";
        }


        public override async void ReverseInit(object returnedData)
        {
            await RefreshBabies();
            await CoreMethods.PushPageModel<BabyViewModel>();
        }


        protected async override void ViewIsAppearing(object sender, EventArgs e)
        {
            base.ViewIsAppearing(sender, e);

            await RefreshBabies();
        }
        public ICommand AddBaby => new Command(
           async () =>
           {
               await CoreMethods.PushPageModel<AddBabyViewModel>(null, true);
           });
        private async Task RefreshBabies()
        {
            var babies = await _babyService.GetBabies();
            Babies = new ObservableCollection<Baby>();
            var babiesOfMother = babies.ToList().Where(b => b.MotherId.ToString() == _motherService.CurrentMother.Id.ToString()).ToList();
            if (babiesOfMother.Count() != 0)
            {
                babiesOfMother.ForEach(baby => Babies.Add(baby));
                HasBabies = true;
                hasNoBabies = false;
            }
            else HasNoBabies = true;
        }
    }
}
