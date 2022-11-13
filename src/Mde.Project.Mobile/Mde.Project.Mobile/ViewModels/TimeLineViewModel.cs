using FreshMvvm;
using Mde.Project.Mobile.Domain.Models;
using Mde.Project.Mobile.Domain.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Mde.Project.Mobile.ViewModels
{
    public class TimeLineViewModel : FreshBasePageModel
    {
        private IMotherService _motherService;

        private bool hasEvents;
        public bool HasEvents
        {
            get { return hasEvents; }
            set
            {
                hasEvents = value;
                RaisePropertyChanged(nameof(HasEvents));
            }
        }

        private bool hasNoEvents;
        public bool HasNoEvents
        {
            get { return hasNoEvents; }
            set
            {
                hasNoEvents = value;
                RaisePropertyChanged(nameof(HasNoEvents));
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

        public TimeLineViewModel(IMotherService motherService)
        {
            _motherService = motherService;
        }

        private ObservableCollection<Event> timeLineEvents;

        public ObservableCollection<Event> TimeLineEvents
        {
            get
            {
                return timeLineEvents;
            }
            set
            {
                timeLineEvents = value;
                if (timeLineEvents != null) timeLineEvents = new ObservableCollection<Event>(timeLineEvents.OrderBy(t => t.Date).Reverse());
                RaisePropertyChanged(nameof(TimeLineEvents));
            }
        }

        protected override void ViewIsAppearing(object sender, EventArgs e)
        {
            PageTitle = "Timeline";
            base.ViewIsAppearing(sender, e);
            RefreshTimeLine();
        }

        private void RefreshTimeLine()
        {
            if (_motherService.CurrentMother.TimeLine.Events.Count != 0)
            {
                HasEvents = true;
                HasNoEvents = false;
                TimeLineEvents = new ObservableCollection<Event>(_motherService.CurrentMother.TimeLine.Events);
            }
            else
            {
                HasEvents = false;
                HasNoEvents = true;
            }
        }

        public ICommand AccountPage => new Command(
            async () =>
            {
                await CoreMethods.PushPageModel<RegistrationViewModel>(null, true);
            });
    }
}
