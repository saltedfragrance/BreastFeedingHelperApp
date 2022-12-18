using FreshMvvm;
using Mde.Project.Mobile.Domain.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Mde.Project.Mobile.ViewModels
{
    public class HelpViewModel : FreshBasePageModel
    {
        private readonly IMotherService _motherService;
        public HelpViewModel(IMotherService motherService)
        {
            _motherService = motherService;
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
        protected async override void ViewIsAppearing(object sender, EventArgs e)
        {
            PageTitle = "Help";
            base.ViewIsAppearing(sender, e);
        }

        public ICommand PhoneToMidWife => new Command(
            async () =>
            {
                try
                {
                    PhoneDialer.Open(_motherService.CurrentMother.MidWifePhoneNumber.ToString());
                }
                catch (Exception)
                {

                    var phoneNumber = await CurrentPage.DisplayPromptAsync("Something went wrong", "Please enter a valid number", "Ok,", "Cancel");
                    PhoneDialer.Open(phoneNumber);
                }
            });
    }
}
