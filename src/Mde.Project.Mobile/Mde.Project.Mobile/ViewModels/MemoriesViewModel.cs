using FreshMvvm;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mde.Project.Mobile.ViewModels
{
    public class MemoriesViewModel: FreshBasePageModel
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

        protected override void ViewIsAppearing(object sender, EventArgs e)
        {
            PageTitle = "Memories";
            base.ViewIsAppearing(sender, e);
        }
    }
}
