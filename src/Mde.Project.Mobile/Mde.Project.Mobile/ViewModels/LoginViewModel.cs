using FreshMvvm;
using Mde.Project.Mobile.Domain.Models;
using Mde.Project.Mobile.Domain.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Mde.Project.Mobile.ViewModels
{
    public class LoginViewModel : FreshBasePageModel
    {
        private readonly IMotherService _motherService;
        public LoginViewModel(IMotherService motherService)
        {
            _motherService = motherService;
        }

        private List<Mother> Mothers { get; set; }

        private Mother currentMother;

        public Mother CurrentMother
        {
            get { return currentMother; }
            set
            {
                currentMother = value;
                RaisePropertyChanged(nameof(Mother));
            }
        }



        protected async override void ViewIsAppearing(object sender, EventArgs e)
        {
            base.ViewIsAppearing(sender, e);

            await GetMothers();
        }

        private async Task GetMothers()
        {
            Mothers = await _motherService.GetMothers();
        }

        public ICommand Login => new Command(
            async () =>
            {

            });
    }
}
