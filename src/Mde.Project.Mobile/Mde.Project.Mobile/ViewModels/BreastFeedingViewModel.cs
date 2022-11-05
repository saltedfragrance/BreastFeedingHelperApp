using FreshMvvm;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mde.Project.Mobile.ViewModels
{
    public class BreastFeedingViewModel : FreshBasePageModel
    {
        public bool IsBreastFeedingPage { get; set; } = true;
        protected async override void ViewIsAppearing(object sender, EventArgs e)
        {
            base.ViewIsAppearing(sender, e);
        }

        public override void ReverseInit(object returnedData)
        {
            base.ReverseInit(returnedData);
        }
    }
}
