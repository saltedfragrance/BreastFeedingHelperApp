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
using Xamarin.Forms.PlatformConfiguration;

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

        private bool onAndroid;
        public bool OnAndroid
        {
            get { return onAndroid; }
            set
            {
                onAndroid = value;
                RaisePropertyChanged(nameof(OnAndroid));
            }
        }

        private bool onUwp;

        public bool OnUwp
        {
            get { return onUwp; }
            set
            {
                onUwp = value;
                RaisePropertyChanged(nameof(OnUwp));
            }
        }
        protected async override void ViewIsAppearing(object sender, EventArgs e)
        {
            if (HelperMethods.CheckOs()) OnAndroid = true;
            else OnUwp = true;

            if (OnAndroid)
            {
                UserDialogs.Instance.ShowLoading("Getting location...");
            }

            PageTitle = "Help";
            await GetLocation();
            base.ViewIsAppearing(sender, e);

            if (OnAndroid)
            {
                UserDialogs.Instance.HideLoading();
            }
        }

        public ICommand PhoneToMidWife => new Command(
            async () =>
            {
                if (OnAndroid)
                {
                    PhoneDialer.Open(_motherService.CurrentMother.MidWifePhoneNumber.ToString());
                }
            });

        public ICommand SearchDiaperStores => new Command(
            async () =>
            {
                if (OnAndroid)
                {
                    await Launcher.OpenAsync($"geo:{CurrentLocation.Latitude},{CurrentLocation.Longitude}?q=Convenience store");
                }
                else if (OnUwp)
                {
                    await Launcher.OpenAsync($"bingmaps:?cp={CurrentLocation.Latitude}~{CurrentLocation.Longitude}&ss=yp.Convenience store~sst.1~pg.2");
                }
            });

        private async Task GetLocation()
        {
            CurrentLocation = (await Geolocation.GetLocationAsync(new GeolocationRequest(GeolocationAccuracy.Best)));
            IEnumerable<Placemark> location = await Geocoding.GetPlacemarksAsync(CurrentLocation);
            CurrentCountry = location.FirstOrDefault().CountryName;
            CurrentCity = location.FirstOrDefault().Locality;
        }
        public ICommand PreviousPage => new Command(
            async () =>
            {
                await CoreMethods.PopPageModel(true, true);
            });
    }
}
