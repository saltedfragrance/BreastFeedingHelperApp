using Acr.UserDialogs;
using FreshMvvm;
using Mde.Project.Mobile.Domain.Enums;
using Mde.Project.Mobile.Domain.Models;
using Mde.Project.Mobile.Domain.Services;
using Mde.Project.Mobile.Domain.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Mde.Project.Mobile.ViewModels
{
    public class MemoriesViewModel : FreshBasePageModel
    {
        private IMotherService _motherService;
        private IMemoryService _memoryService;
        public MemoriesViewModel(IMotherService motherService, IMemoryService memoryService)
        {
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

        private int rotation;
        public int Rotation
        {
            get { return rotation; }
            set
            {
                rotation = value;
                RaisePropertyChanged(nameof(Rotation));
            }
        }

        private bool hasMemories;
        public bool HasMemories
        {
            get { return hasMemories; }
            set
            {
                hasMemories = value;
                RaisePropertyChanged(nameof(HasMemories));
            }
        }

        private bool hasNoMemories;
        public bool HasNoMemories
        {
            get { return hasNoMemories; }
            set
            {
                hasNoMemories = value;
                RaisePropertyChanged(nameof(HasNoMemories));
            }
        }

        private ObservableCollection<Memory> memories;

        public ObservableCollection<Memory> Memories
        {
            get
            {
                return memories;
            }
            set
            {
                memories = value;
                if (memories != null) memories = new ObservableCollection<Memory>(memories.OrderBy(t => t.Date).Reverse());
                RaisePropertyChanged(nameof(Memories));
            }
        }
        protected override void ViewIsAppearing(object sender, EventArgs e)
        {
            PageTitle = "Memories";
            base.ViewIsAppearing(sender, e);
            RefreshMemories();
        }

        public override void ReverseInit(object returnedData)
        {
            RefreshMemories();
            base.ReverseInit(returnedData);
        }

        private void RefreshMemories()
        {
            if (_motherService.CurrentMother.Memories != null)
            {
                HasMemories = true;
                HasNoMemories = false;
                Memories = new ObservableCollection<Memory>(_motherService.CurrentMother.Memories);
            }
            else
            {
                HasMemories = false;
                HasNoMemories = true;
            }
        }

        public ICommand AddMemory => new Command(
           async () =>
           {
               if (_motherService.CurrentMother.Babies.Count != 0 || _motherService.CurrentMother.Babies == null)
               {
                   await CoreMethods.PushPageModel<AddMemoryViewModel>(null, true);
               }
               else
               {
                   await CoreMethods.DisplayAlert("No babies yet!", "You must add at least one baby before you can start adding memories.", "Continue");
               }
           });

        public ICommand DeleteMemory => new Command<Guid>(
            async (Guid id) =>
            {
                bool answer = await CoreMethods.DisplayAlert("Attention", "Are you sure wish to delete this memory?", "Yes", "No");
                if (answer)
                {
                    await _motherService.AddEventToTimeLine($"You deleted the memory {(await _memoryService.GetMemories()).Where(m => m.Id == id).FirstOrDefault().Title}!", TimeLineCategories.DeletedMemoryMessage);
                    await _memoryService.DeleteMemory(id.ToString());
                }
                await _motherService.RefreshCurrentMother();
                RefreshMemories();
            });

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
