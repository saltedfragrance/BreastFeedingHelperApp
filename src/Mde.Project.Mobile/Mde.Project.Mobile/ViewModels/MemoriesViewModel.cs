using FreshMvvm;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Mde.Project.Mobile.ViewModels
{
    public class MemoriesViewModel : FreshBasePageModel
    {
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


        protected override void ViewIsAppearing(object sender, EventArgs e)
        {
            PageTitle = "Memories";
            base.ViewIsAppearing(sender, e);
        }

        public ICommand AccountPage => new Command(
            async () =>
            {
                await CoreMethods.PushPageModel<RegistrationViewModel>(null, true);
            });
    }
}
