using FreshMvvm;
using Mde.Project.Mobile.Domain.Models;
using Mde.Project.Mobile.Domain.Services;
using Mde.Project.Mobile.Domain.Services.Interfaces;
using Mde.Project.Mobile.Domain.Services.Mocking;
using Mde.Project.Mobile.ViewModels;
using System;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Xamarin.Forms.Xaml;

namespace Mde.Project.Mobile
{
    public partial class App : Xamarin.Forms.Application
    {
        public App()
        {
            InitializeComponent();

            FreshIOC.Container.Register<IMotherService>(new MockMotherService());
            FreshIOC.Container.Register<IBabyService>(new MockBabyService());
            FreshIOC.Container.Register<IUserService>(new UserService(new MockMotherService()));

            var mainContainer = new FreshTabbedNavigationContainer(Constants.MainContainer);
            mainContainer.BarBackgroundColor = Color.Pink;
            mainContainer.AddTab<TimeLineViewModel>("Timeline", "timeline.png", null);
            mainContainer.AddTab<StatisticsViewModel>("Statistics", "statistics.png", null);
            mainContainer.AddTab<BabyViewModel>("Babies", "baby.png", null);
            mainContainer.AddTab<BreastFeedingViewModel>("Breastfeeding", "breastfeeding.png", null);
            mainContainer.AddTab<TimeLineViewModel>("Memories", "memories.png", null);
            mainContainer.On<Xamarin.Forms.PlatformConfiguration.Android>().SetToolbarPlacement(ToolbarPlacement.Bottom);

            MainPage = new FreshNavigationContainer(FreshPageModelResolver.ResolvePageModel<LoginViewModel>());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
