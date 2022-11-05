using FreshMvvm;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Mde.Project.Mobile.ViewModels
{
    public class BreastFeedingViewModel : FreshBasePageModel
    {
        private bool isBreastFeedingPage = true;

        public bool IsBreastFeedingPage
        {
            get { return isBreastFeedingPage; }
            set
            {
                isBreastFeedingPage = value;
                RaisePropertyChanged(nameof(IsBreastFeedingPage));
            }
        }

        private bool isPumpingPage = true;

        public bool IsPumpingPage
        {
            get { return isPumpingPage; }
            set
            {
                isPumpingPage = value;
                RaisePropertyChanged(nameof(IsPumpingPage));
            }
        }

        private bool isFeedingPage;

        public bool IsFeedingPage
        {
            get { return isFeedingPage; }
            set
            {
                isFeedingPage = value;
                RaisePropertyChanged(nameof(IsFeedingPage));
            }
        }

        private bool isRemindersPage;

        public bool IsRemindersPage
        {
            get { return isRemindersPage; }
            set
            {
                isRemindersPage = value;
                RaisePropertyChanged(nameof(IsRemindersPage));
            }
        }
        protected async override void ViewIsAppearing(object sender, EventArgs e)
        {
            base.ViewIsAppearing(sender, e);
        }

        public override void ReverseInit(object returnedData)
        {
            base.ReverseInit(returnedData);
        }

        public ICommand PumpingPage => new Command(
            () =>
           {
               IsPumpingPage = true;
               IsFeedingPage = false;
               IsRemindersPage = false;
           });

        public ICommand FeedingPage => new Command(
            () =>
           {
               IsFeedingPage = true;
               IsPumpingPage = false;
               IsRemindersPage = false;
           });

        public ICommand RemindersPage => new Command(
            () =>
            {
                IsRemindersPage = true;
                IsFeedingPage = false;
                IsPumpingPage = false;
            });
    }
}
