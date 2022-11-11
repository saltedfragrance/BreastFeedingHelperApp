using FreshMvvm;
using Mde.Project.Mobile.Domain.Models;
using Mde.Project.Mobile.Domain.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using System.Text;

namespace Mde.Project.Mobile.ViewModels
{
    public class TimeLineViewModel : FreshBasePageModel
    {
        private IMotherService _motherService;

        public TimeLineViewModel(IMotherService motherService)
        {
            _motherService = motherService;
        }

        private ObservableCollection<Event> timeLineEvents;

        public ObservableCollection<Event> TimeLineEvents
        {
            get { return timeLineEvents; }
            set
            {
                timeLineEvents = value;
                RaisePropertyChanged(nameof(TimeLineEvents));
            }
        }

        protected override void ViewIsAppearing(object sender, EventArgs e)
        {
            base.ViewIsAppearing(sender, e);
            RefreshTimeLine();
        }

        private void RefreshTimeLine()
        {
            TimeLineEvents = new ObservableCollection<Event>(_motherService.CurrentMother.TimeLine.Events);
        }

    }
}
