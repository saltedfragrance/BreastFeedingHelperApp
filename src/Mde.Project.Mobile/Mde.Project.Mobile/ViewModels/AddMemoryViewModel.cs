using FreshMvvm;
using Mde.Project.Mobile.Domain.Enums;
using Mde.Project.Mobile.Domain.Models;
using Mde.Project.Mobile.Domain.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Threading.Tasks;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Mde.Project.Mobile.ViewModels
{
    public class AddMemoryViewModel : FreshBasePageModel
    {
        private readonly IMotherService _motherService;
        private readonly IBabyService _babyService;

        public AddMemoryViewModel(IMotherService motherService, IBabyService babyService)
        {
            _motherService = motherService;
            _babyService = babyService;
        }

        private string pageTitle;
        public string PageTitle
        {
            get { return pageTitle; }
            set
            {
                pageTitle = value;
                RaisePropertyChanged(nameof(pageTitle));
            }
        }

        private Guid? id;

        public Guid? Id
        {
            get { return id; }
            set { id = value; }
        }

        private DateTime date;
        public DateTime Date
        {
            get
            {
                return date;
            }
            set
            {
                date = value;
                RaisePropertyChanged(nameof(Date));
            }
        }

        private string title;
        public string Title
        {
            get
            {
                return title;
            }
            set
            {
                title = value;
                RaisePropertyChanged(nameof(Title));
            }
        }

        private string description;
        public string Description
        {
            get
            {
                return description;
            }
            set
            {
                description = value;
                RaisePropertyChanged(nameof(Description));
            }
        }

        public async override void Init(object initData)
        {
            base.Init(initData);
            PageTitle = "Add memory";
        }

        public ICommand PreviousPage => new Command(
            async () =>
            {
                await CoreMethods.PopPageModel(true, true);
            });

        //public ICommand AddMemory => new Command(
        //    async () =>
        //    {
        //        await _memoryService.AddMemory();
        //        var memories = await _memoryService.GetMemories();
        //        await _motherService.AddEventToTimeLine($"A new memory was added!  {memories.Last()}!", TimeLineCategories.MemoryAddedMessage);
        //        PreviousPageModel.ReverseInit(new Memory());
        //        await CoreMethods.PopPageModel(true, true);
        //    });

        //public ICommand DeleteMemory => new Command<Guid>(
        //    async (Guid id) =>
        //    {
        //        await _memoryService.DeleteMemory(id.ToString());
        //    });
    }
}
