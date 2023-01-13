using FreshMvvm;
using Mde.Project.Mobile.Domain;
using Mde.Project.Mobile.Domain.Enums;
using Mde.Project.Mobile.Domain.Models;
using Mde.Project.Mobile.Domain.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;

namespace Mde.Project.Mobile.ViewModels
{
    public class BreastFeedingViewModel : FreshBasePageModel
    {
        private IMotherService _motherService;
        private INotificationManager _notificationManager;
        private readonly IReminderService _reminderService;
        private const string FeedingReminderType = "FeedingReminderType";
        private const string PumpingReminderType = "PumpingReminderType";
        private bool stopWatchEnabled = false;
        private Stopwatch stopWatch = new Stopwatch();

        public BreastFeedingViewModel(IMotherService motherService, IReminderService reminderService)
        {
            _motherService = motherService;
            _reminderService = reminderService;
            _notificationManager = DependencyService.Get<INotificationManager>();
        }

        private string feedingReminder;
        public string FeedingReminder
        {
            get { return feedingReminder; }
            set
            {
                feedingReminder = value;
                RaisePropertyChanged(nameof(FeedingReminder));
            }
        }

        private bool activeFeedingReminder;
        public bool ActiveFeedingReminder
        {
            get { return activeFeedingReminder; }
            set
            {
                activeFeedingReminder = value;
                RaisePropertyChanged(nameof(ActiveFeedingReminder));
            }
        }

        private string pumpingReminder;
        public string PumpingReminder
        {
            get { return pumpingReminder; }
            set
            {
                pumpingReminder = value;
                RaisePropertyChanged(nameof(PumpingReminder));
            }
        }

        private bool activePumpingReminder;
        public bool ActivePumpingReminder
        {
            get { return activePumpingReminder; }
            set
            {
                activePumpingReminder = value;
                RaisePropertyChanged(nameof(ActivePumpingReminder));
            }
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
            await GetReminders();
            base.ViewIsAppearing(sender, e);
        }

        public override void ReverseInit(object returnedData)
        {
            base.ReverseInit(returnedData);
        }

        public async Task GetReminders()
        {
            var reminders = await _reminderService.GetAll(_motherService.CurrentMother.Id.ToString());
            if (reminders != null)
            {
                reminders.ForEach(r =>
                {
                    if (r.Type == FeedingReminderType)
                    {
                        ActiveFeedingReminder = true;
                        FeedingReminder = r.IntervalTime;
                    }
                    else
                    {
                        ActivePumpingReminder = true;
                        PumpingReminder = r.IntervalTime;
                    }
                });
            }
        }

        public ICommand SendNotification => new Command<string>(
           async (string type) =>
           {
               string title;
               string message = $"Baby is huuuungry!";
               var intervalTime = await CurrentPage.DisplayPromptAsync("Alarm interval selection", "Please select an alarm interval in minutes", "Ok", "Cancel", null, -1, Keyboard.Numeric);

               if (intervalTime != null)
               {
                   if (type == "FeedingReminder")
                   {
                       title = $"Time to breastfeed";
                       FeedingReminder = intervalTime;
                       ActiveFeedingReminder = true;
                       await _reminderService.AddReminder(Guid.NewGuid().ToString(), _motherService.CurrentMother.Id.ToString(), intervalTime, title, message, FeedingReminderType);
                   }
                   else
                   {
                       title = $"Time to pump";
                       PumpingReminder = intervalTime;
                       ActivePumpingReminder = true;
                       await _reminderService.AddReminder(Guid.NewGuid().ToString(), _motherService.CurrentMother.Id.ToString(), intervalTime, title, message, PumpingReminderType);
                   }

                   _notificationManager.ScheduleNotification(Guid.NewGuid().ToString(), title, message, DateTime.Now.Millisecond.ToString(), intervalTime);
               }
               else return;
           });

        public ICommand CancelReminder => new Command<string>(
           async (string type) =>
           {
               Reminder reminder = new Reminder();
               bool answer = await CoreMethods.DisplayAlert("Attention", "Are you sure wish to cancel this alert?", "Yes", "No");


               if (answer)
               {
                   if (type == "FeedingReminder")
                   {
                       reminder = await _reminderService.GetFeedingReminder(_motherService.CurrentMother.Id.ToString());
                       ActiveFeedingReminder = false;
                   }
                   else
                   {
                       reminder = await _reminderService.GetPumpingReminder(_motherService.CurrentMother.Id.ToString());
                       ActivePumpingReminder = false;
                   }

                   await _reminderService.RemoveReminder(reminder.Id.ToString());
                   _notificationManager.CancelNotification(reminder.Id.ToString(), reminder.Title, reminder.Message);
               }
               else return;
           });

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
        public ICommand HelpPage => new Command(
            async () =>
            {
                await CoreMethods.PushPageModel<HelpViewModel>(null, true);
            });
    }
}
