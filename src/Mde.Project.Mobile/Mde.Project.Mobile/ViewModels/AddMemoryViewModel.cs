using FreshMvvm;
using Mde.Project.Mobile.Domain.Enums;
using Mde.Project.Mobile.Domain.Models;
using Mde.Project.Mobile.Domain.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Input;
using Xamarin.CommunityToolkit.Core;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Mde.Project.Mobile.ViewModels
{
    public class AddMemoryViewModel : FreshBasePageModel
    {
        private readonly IMemoryService _memoryService;
        private readonly IMotherService _motherService;

        public AddMemoryViewModel(IMemoryService memoryService, IMotherService motherService)
        {
            _memoryService = memoryService;
            _motherService = motherService;
        }

        public FileResult MediaFile { get; set; }

        private IList<Baby> babies;
        public IList<Baby> Babies
        {
            get { return babies; }
            set
            {
                babies = value;
                RaisePropertyChanged(nameof(Babies));
            }
        }

        private Baby selectedBaby;
        public Baby SelectedBaby
        {
            get { return selectedBaby; }
            set
            {
                selectedBaby = value;
                RaisePropertyChanged(nameof(SelectedBaby));
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

        private int rotation;
        public int Rotation
        {
            get { return rotation; }
            set
            {
                rotation = value;
                RaisePropertyChanged(nameof(Rotation));
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
            Babies = _motherService.CurrentMother.Babies.ToList();
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
            MediaFile = null;

            if (((Button)control).Text == "Add a picture")
            {
                MediaFile = await MediaPicker.PickPhotoAsync();
            }
            else
            {
                MediaFile = await MediaPicker.CapturePhotoAsync();
            }

            if (MediaFile != null) stream = await MediaFile.OpenReadAsync();
            else return;

            PictureSource = ImageSource.FromStream(() => stream);
        });

        public ICommand AddMovie => new Command<object>(
            async (object control) =>
            {
                //https://stackoverflow.com/questions/71583611/xamarin-forms-mediaelement-playing-video-selected-from-gallery-with-crossmedia

                IsPicture = false;
                IsMovie = true;
                PictureSource = null;
                MediaFile = null;

                string path = string.Empty;
                var fileName = "tempvideo";
                var newFile = Path.Combine(FileSystem.AppDataDirectory, fileName + ".mp4");
                Stream stream = null;

                if (((Button)control).Text == "Add a movie")
                {
                    MediaFile = await MediaPicker.PickVideoAsync();
                }
                else
                {
                    MediaFile = await MediaPicker.CaptureVideoAsync();

                }

                if (MediaFile != null) stream = await MediaFile.OpenReadAsync();
                else return;

                using (var newStream = File.OpenWrite(newFile))
                    await stream.CopyToAsync(newStream);

                VideoSource = MediaSource.FromFile(newFile);
            });

        public ICommand AddMemory => new Command(
            async () =>
            {
                await _memoryService.CreateMemory(Title, Description, DateTime.Now.ToString(), MediaFile, _motherService.CurrentMother.Id.ToString(), SelectedBaby.Id.ToString());
                await _motherService.AddEventToTimeLine($"A new memory was added! {(await _memoryService.GetMemories()).Last().Title}!", TimeLineCategories.MemoryAddedMessage);
                await _motherService.RefreshCurrentMother();
                PreviousPageModel.ReverseInit(new Memory());
                await CoreMethods.PopPageModel(true, true);
            });

        //public ICommand DeleteMemory => new Command<Guid>(
        //    async (Guid id) =>
        //    {
        //        await _memoryService.DeleteMemory(id.ToString());
        //    });

        public ICommand RotateImage => new Command(
             () =>
            {
                if (Rotation == 0) Rotation = 90;
                else if (Rotation == 90) Rotation = 180;
                else Rotation = 0;
            });
    }
}
