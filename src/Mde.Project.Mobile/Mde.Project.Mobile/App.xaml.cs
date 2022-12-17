using FreshMvvm;
using Mde.Project.Mobile.Domain.Models;
using Mde.Project.Mobile.Domain.Services;
using Mde.Project.Mobile.Domain.Services.Interfaces;
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

            FreshIOC.Container.Register<IFireBaseService>(new FireBaseService());
            FreshIOC.Container.Register<IBabyService>(new BabyService(FreshIOC.Container.Resolve<IFireBaseService>()));
            FreshIOC.Container.Register<IMotherService>(new MotherService(FreshIOC.Container.Resolve<IFireBaseService>(), FreshIOC.Container.Resolve<IBabyService>()));
            FreshIOC.Container.Register<IUserService>(new UserService(FreshIOC.Container.Resolve<IMotherService>(), FreshIOC.Container.Resolve<IFireBaseService>()));
            FreshIOC.Container.Register<IMemoryService>(new MemoryService(FreshIOC.Container.Resolve<IFireBaseService>()));

            var loginContainer = new FreshNavigationContainer(FreshPageModelResolver.ResolvePageModel<LoginViewModel>(), Constants.LoginContainer);
            var mainContainer = new FreshTabbedNavigationContainer(Constants.MainContainer);
            mainContainer.BarBackgroundColor = Color.Pink;
            mainContainer.AddTab<TimeLineViewModel>("Timeline", "timeline.png", null);
            mainContainer.AddTab<StatisticsViewModel>("Statistics", "statistics.png", null);
            mainContainer.AddTab<BabyViewModel>("Babies", "baby.png", null);
            mainContainer.AddTab<BreastFeedingViewModel>("Breastfeeding", "breastfeeding.png", null);
            mainContainer.AddTab<MemoriesViewModel>("Memories", "memories.png", null);
            mainContainer.On<Xamarin.Forms.PlatformConfiguration.Android>().SetToolbarPlacement(ToolbarPlacement.Bottom);

            MainPage = loginContainer;
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
