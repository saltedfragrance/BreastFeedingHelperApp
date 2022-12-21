using Acr.UserDialogs;
using FreshMvvm;
using Mde.Project.Mobile.Domain.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public Location CurrentLocation { get; set; }
        private string currentCountry;
        public string CurrentCountry
        {
            get { return currentCountry; }
            set
            {
                currentCountry = value;
                RaisePropertyChanged(nameof(CurrentCountry));
            }
        }
        private string currentCity;
        public string CurrentCity
        {
            get { return currentCity; }
            set
            {
                currentCity = value;
                RaisePropertyChanged(nameof(CurrentCity));
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
        protected async override void ViewIsAppearing(object sender, EventArgs e)
        {
            UserDialogs.Instance.ShowLoading("Getting location...");
            PageTitle = "Help";
            await GetLocation();
            base.ViewIsAppearing(sender, e);
            UserDialogs.Instance.HideLoading();
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

        public ICommand SearchDiaperStores => new Command(
            async () =>
            {
                await Launcher.OpenAsync($"geo:{CurrentLocation.Latitude},{CurrentLocation.Longitude}?q=Kruidvat");
            });

        private async Task GetLocation()
        {
            CurrentLocation = (await Geolocation.GetLocationAsync(new GeolocationRequest(GeolocationAccuracy.Best)));
            IEnumerable<Placemark> location = await Geocoding.GetPlacemarksAsync(CurrentLocation);
            CurrentCountry = location.FirstOrDefault().CountryName;
            CurrentCity = location.FirstOrDefault().Locality;
        }
    }
}
