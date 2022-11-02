using FreshMvvm;
using Mde.Project.Mobile.Domain.Models;
using Mde.Project.Mobile.Domain.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Text;
using System.Threading.Tasks;

namespace Mde.Project.Mobile.ViewModels
{
    public class BabyViewModel : FreshBasePageModel
    {
        private readonly IBabyService _babyService;
        private readonly IMotherService _motherService;

        public BabyViewModel(IBabyService babyService, IMotherService motherService)
        {
            _babyService = babyService;
            _motherService = motherService;
        }

        private ObservableCollection<Baby> babies;

        public ObservableCollection<Baby> Babies
        {
            get { return babies; }
            set
            {
                babies = value;
                RaisePropertyChanged(nameof(Babies));
            }
        }
        protected async override void ViewIsAppearing(object sender, EventArgs e)
        {
            base.ViewIsAppearing(sender, e);

            await RefreshBabies();
        }

        private async Task RefreshBabies()
        {
            var babies = await _babyService.GetBabies();
            Babies = new ObservableCollection<Baby>();
            babies.ToList().Where(b => b.MotherId == _motherService.CurrentMother.Id).ToList().ForEach(baby => Babies.Add(baby));
        }
    }
}
