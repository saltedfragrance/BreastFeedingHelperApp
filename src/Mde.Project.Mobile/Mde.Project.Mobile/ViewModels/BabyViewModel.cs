using Acr.UserDialogs;
using FreshMvvm;
using Mde.Project.Mobile.Domain.Enums;
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
        private readonly IMemoryService _memoryService;

        public BabyViewModel(IBabyService babyService, IMotherService motherService, IMemoryService memoryService)
        {
            _babyService = babyService;
            _motherService = motherService;
            _memoryService = memoryService;
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

        private ObservableCollection<Baby> babies;

        public ObservableCollection<Baby> Babies
        {
            get
            {
                return babies;
            }
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

        public override async void ReverseInit(object returnedData)
        {
            await RefreshBabies();
        }


        protected async override void ViewIsAppearing(object sender, EventArgs e)
        {
            PageTitle = "Babies";
            base.ViewIsAppearing(sender, e);

            await RefreshBabies();
        }

        public ICommand AddOrEditBaby => new Command<Guid?>(
           async (Guid? id) =>
           {
               if (id == Guid.Empty) await CoreMethods.PushPageModel<AddBabyViewModel>(null, true);
               else await CoreMethods.PushPageModel<AddBabyViewModel>(id, true);
           });

        public ICommand DeleteBaby => new Command<Guid>(
            async (Guid id) =>
            {
                bool answer = await CoreMethods.DisplayAlert("Attention", "Are you sure wish to delete this baby?", "Yes", "No");
                if (answer)
                {
                    if (Device.RuntimePlatform == Device.Android)
                    {
                        UserDialogs.Instance.ShowLoading("Deleting baby...");
                    }
                    var baby = await _babyService.GetBaby(id.ToString());
                    baby.Memories = (await _memoryService.GetMemories()).Where(m => m.BabyId== id).ToList();
                    var babyMemoriesIds = baby.Memories.Select(m => m.Id.ToString()).ToList();
                    await _motherService.AddEventToTimeLine($"You deleted {(await _babyService.GetBaby(id.ToString())).FirstName}!", TimeLineCategories.DeletedBabyMessage);
                    await _memoryService.DeleteMemories(babyMemoriesIds);
                    await _babyService.DeleteBaby(id.ToString());
                    await RefreshBabies();
                }
                await _motherService.RefreshCurrentMother();
                if (Device.RuntimePlatform == Device.Android)
                {
                    UserDialogs.Instance.HideLoading();
                }

            });

        public ICommand EditWeight => new Command<Guid>(
            async (Guid id) =>
            {
                var weight = await CurrentPage.DisplayPromptAsync("Edit weight", "Please enter a weight between 1 and 10kg", "Ok,", "Cancel",
                    null, 10, Keyboard.Numeric, "1");
                await _motherService.AddEventToTimeLine($"You changed the weight of {(await _babyService.GetBaby(id.ToString())).FirstName} to {weight}!", TimeLineCategories.BabyWeightGainMessage);
                await _babyService.UpdateWeight(id.ToString(), weight);
                await RefreshBabies();
                await _motherService.RefreshCurrentMother();
            });

        public ICommand EditHeight => new Command<Guid>(
            async (Guid id) =>
            {
                var height = await CurrentPage.DisplayPromptAsync("Edit height", "Please enter a height between 1 and 80cm", "Ok,", "Cancel",
                    null, 10, Keyboard.Numeric, "1");
                await _motherService.AddEventToTimeLine($"You changed the weight of {(await _babyService.GetBaby(id.ToString())).FirstName} to {height}!", TimeLineCategories.BabyHeightGainMessage);
                await _babyService.UpdateHeight(id.ToString(), height);
                await RefreshBabies();
                await _motherService.RefreshCurrentMother();
            });

        private async Task RefreshBabies()
        {
            var babies = await _babyService.GetBabies();
            Babies = new ObservableCollection<Baby>();
            babies.Where(baby => baby.MotherId == _motherService.CurrentMother.Id).ToList().ForEach(b => Babies.Add(b));

            if (Babies.Count() != 0)
            {
                HasBabies = true;
                HasNoBabies = false;
            }
            else
            {
                Babies.Clear();
                HasBabies = false;
                HasNoBabies = true;
            }
        }

        public ICommand AccountPage => new Command(
            async () =>
            {
                await CoreMethods.PushPageModel<RegistrationViewModel>(null, true);
            });
        public ICommand HelpPage => new Command(
            async () =>
            {
                await CoreMethods.PushPageModel<HelpViewModel>(null, true);
            });
    }
}
