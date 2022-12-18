using FreshMvvm;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mde.Project.Mobile.ViewModels
{
    public class HelpViewModel : FreshBasePageModel
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

        protected async override void ViewIsAppearing(object sender, EventArgs e)
        {
            PageTitle = "Help";
            base.ViewIsAppearing(sender, e);
        }
    }
}
