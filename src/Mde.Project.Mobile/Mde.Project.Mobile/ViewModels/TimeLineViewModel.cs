using FreshMvvm;
using Mde.Project.Mobile.Domain.Models;
using Mde.Project.Mobile.Domain.Services.Interfaces;
using System;
using System.Collections.Generic;
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

        private TimeLine timeLine;

        public TimeLine TimeLine
        {
            get { return timeLine; }
            set
            {
                timeLine = value;
                RaisePropertyChanged(nameof(TimeLine));
            }
        }

        protected override void ViewIsAppearing(object sender, EventArgs e)
        {
            base.ViewIsAppearing(sender, e);
            RefreshTimeLine();
        }

        private void RefreshTimeLine()
        {
            TimeLine = _motherService.CurrentMother.TimeLine;
        }

    }
}
