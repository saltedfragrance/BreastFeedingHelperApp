using FreshMvvm;
using Mde.Project.Mobile.Domain.Models;
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

        public MemoriesViewModel(IMotherService motherService)
        {
            _motherService = motherService;
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
               await CoreMethods.PushPageModel<AddMemoryViewModel>(null, true);
           });

        public ICommand AccountPage => new Command(
            async () =>
            {
                await CoreMethods.PushPageModel<RegistrationViewModel>(null, true);
            });
    }
}
