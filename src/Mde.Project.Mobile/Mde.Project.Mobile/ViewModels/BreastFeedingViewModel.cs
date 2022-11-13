using FreshMvvm;
using Mde.Project.Mobile.Domain.Enums;
using Mde.Project.Mobile.Domain.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Mde.Project.Mobile.ViewModels
{
    public class BreastFeedingViewModel : FreshBasePageModel
    {
        private IMotherService _motherService;

        public BreastFeedingViewModel(IMotherService motherService)
        {
            _motherService = motherService;
        }

        private bool stopWatchEnabled = false;
        private Stopwatch stopWatch = new Stopwatch();

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
        public bool StopWatchEnabled
        {
            get { return stopWatchEnabled; }
            set
            {
                stopWatchEnabled = value;
                RaisePropertyChanged(nameof(StopWatchEnabled));
            }
        }

        private bool leftNippleIsChecked;

        public bool LeftNippleIsChecked
        {
            get { return leftNippleIsChecked; }
            set
            {
                leftNippleIsChecked = value;
                if (leftNippleIsChecked)
                {
                    RightNippleIsChecked = false;
                    BothNipplesAreChecked = false;
                }
                RaisePropertyChanged(nameof(LeftNippleIsChecked));
            }
        }

        private bool rightNippleIsChecked;

        public bool RightNippleIsChecked
        {
            get { return rightNippleIsChecked; }
            set
            {
                rightNippleIsChecked = value;
                if (rightNippleIsChecked)
                {
                    LeftNippleIsChecked = false;
                    BothNipplesAreChecked = false;
                }
                RaisePropertyChanged(nameof(RightNippleIsChecked));
            }
        }

        private bool bothNipplesAreChecked;

        public bool BothNipplesAreChecked
        {
            get { return bothNipplesAreChecked; }
            set
            {
                bothNipplesAreChecked = value;
                if (bothNipplesAreChecked)
                {
                    RightNippleIsChecked = false;
                    LeftNippleIsChecked = false;
                }
                RaisePropertyChanged(nameof(BothNipplesAreChecked));
            }
        }

        private bool pumpingStarted = false;

        public bool PumpingStarted
        {
            get { return pumpingStarted; }
            set
            {
                pumpingStarted = value;
                RaisePropertyChanged(nameof(PumpingStarted));
            }
        }

        private bool pumpingStopped = true;

        public bool PumpingStopped
        {
            get { return pumpingStopped; }
            set
            {
                pumpingStopped = value;
                RaisePropertyChanged(nameof(PumpingStopped));
            }
        }

        private string stopWatchSeconds;
        public string StopWatchSeconds
        {
            get { return stopWatchSeconds; }
            set
            {
                stopWatchSeconds = value;
                RaisePropertyChanged(nameof(StopWatchSeconds));
            }
        }

        private string stopWatchMinutes;
        public string StopWatchMinutes
        {
            get { return stopWatchMinutes; }
            set
            {
                stopWatchMinutes = value;
                RaisePropertyChanged(nameof(StopWatchMinutes));
            }
        }

        private string stopWatchHours;
        public string StopWatchHours
        {
            get { return stopWatchHours; }
            set
            {
                stopWatchHours = value;
                RaisePropertyChanged(nameof(StopWatchHours));
            }
        }

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
            PageTitle = "Breastfeeding";
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
        public ICommand StartPumping => new Command(
            () =>
            {
                if (!RightNippleIsChecked && !LeftNippleIsChecked && !BothNipplesAreChecked)
                {
                    CoreMethods.DisplayAlert("Attention", "Please select which nipples you wish to pump", "Continue");
                    return;
                }
                PumpingStopped = false;
                PumpingStarted = true;
                StopWatchEnabled = true;
                stopWatch.Restart();
                StopWatchHours = stopWatch.Elapsed.Hours.ToString();
                StopWatchMinutes = stopWatch.Elapsed.Minutes.ToString();
                StopWatchSeconds = stopWatch.Elapsed.Seconds.ToString();

                Device.StartTimer(TimeSpan.FromSeconds(1), () =>
                {
                    StopWatchHours = stopWatch.Elapsed.Hours.ToString();
                    StopWatchMinutes = stopWatch.Elapsed.Minutes.ToString();
                    StopWatchSeconds = stopWatch.Elapsed.Seconds.ToString();
                    return true;
                });
            });

        public ICommand StopPumping => new Command(
                async () =>
                {
                    string amountPumped = await CurrentPage.DisplayPromptAsync("End pumping session?", "Please enter the amount of milk pumped in milliliters", "Finish session", "Continue session");
                    if (amountPumped != null)
                    {
                        string nipples = null;
                        string message = null;
                        if (RightNippleIsChecked) nipples = "the right nipple";
                        else if (LeftNippleIsChecked) nipples = "the left nipple";
                        else nipples = "both nipples";
                        PumpingStopped = true;
                        PumpingStarted = false;
                        StopWatchEnabled = false;
                        stopWatch.Stop();
                        TimeSpan ts = stopWatch.Elapsed;
                        if (ts.Minutes == 0) message = $"Pumped {amountPumped}ml of breast milk on {nipples} during {string.Format("{0:0} seconds", ts.Seconds)}";
                        else message = $"Pumped {amountPumped}ml of breast milk on {nipples} during {string.Format("{0:0} minutes and {1:0} seconds", ts.Minutes, ts.Seconds)}";
                        await _motherService.AddEventToTimeLine(message, TimeLineCategories.PumpingMessage);
                    }
                });

        public ICommand AccountPage => new Command(
            async () =>
            {
                await CoreMethods.PushPageModel<RegistrationViewModel>(null, true);
            });
    }
}
