using FreshMvvm;
using Mde.Project.Mobile.Domain.Enums;
using Mde.Project.Mobile.Domain.Models;
using Mde.Project.Mobile.Domain.Services;
using Mde.Project.Mobile.Domain.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Threading.Tasks;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.Core;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Essentials;
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

        private bool isPicture;
        public bool IsPicture
        {
            get { return isPicture; }
            set
            {
                isPicture = value;
                RaisePropertyChanged(nameof(IsPicture));
            }
        }

        private bool isMovie;
        public bool IsMovie
        {
            get { return isMovie; }
            set
            {
                isMovie = value;
                RaisePropertyChanged(nameof(IsMovie));
            }
        }

        private ImageSource pictureSource;
        public ImageSource PictureSource
        {
            get { return pictureSource; }
            set
            {
                pictureSource = value;
                RaisePropertyChanged(nameof(PictureSource));
            }
        }

        private MediaSource videoSource;
        public MediaSource VideoSource
        {
            get { return videoSource; }
            set
            {
                videoSource = value;
                RaisePropertyChanged(nameof(VideoSource));
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
        public ICommand AddPicture => new Command<object>(
        async (object control) =>
        {
            Stream stream = null;
            IsPicture = true;
            IsMovie = false;
            VideoSource = null;

            if (((Button)control).Text == "Add a picture")
            {
                var result = await MediaPicker.PickPhotoAsync();
                if (result != null) stream = await result.OpenReadAsync();
                else return;
            }
            else
            {
                var result = await MediaPicker.CapturePhotoAsync();
                if (result != null) stream = await result.OpenReadAsync();
                else return;
            }
            PictureSource = ImageSource.FromStream(() => stream);
        });

        public ICommand AddMovie => new Command<object>(
            async (object control) =>
            {
                //https://stackoverflow.com/questions/71583611/xamarin-forms-mediaelement-playing-video-selected-from-gallery-with-crossmedia

                IsPicture = false;
                IsMovie = true;
                PictureSource = null;

                string path = string.Empty;
                var fileName = "tempvideo";
                var newFile = Path.Combine(FileSystem.AppDataDirectory, fileName + ".mp4");
                Stream stream = null;

                if (((Button)control).Text == "Add a movie")
                {
                    var result = await MediaPicker.PickVideoAsync();
                    if (result != null) stream = await result.OpenReadAsync();
                    else return;
                }
                else
                {
                    var result = await MediaPicker.CaptureVideoAsync();
                    if (result != null) stream = await result.OpenReadAsync();
                    else return;
                }
                using (var newStream = File.OpenWrite(newFile))
                    await stream.CopyToAsync(newStream);

                VideoSource = MediaSource.FromFile(newFile);
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
