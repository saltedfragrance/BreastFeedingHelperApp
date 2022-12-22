using Acr.UserDialogs;
using FluentValidation;
using FreshMvvm;
using Mde.Project.Mobile.Domain.Enums;
using Mde.Project.Mobile.Domain.Models;
using Mde.Project.Mobile.Domain.Services.Interfaces;
using Mde.Project.Mobile.Domain.Validators;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Input;
using Xamarin.CommunityToolkit.Core;
using Xamarin.Essentials;
using Xamarin.Forms;
using static Xamarin.Essentials.Permissions;

namespace Mde.Project.Mobile.ViewModels
{
    public class AddMemoryViewModel : FreshBasePageModel
    {
        private readonly IMemoryService _memoryService;
        private readonly IMotherService _motherService;
        private readonly IValidator _memoryValidator;

        public AddMemoryViewModel(IMemoryService memoryService, IMotherService motherService)
        {
            _memoryService = memoryService;
            _motherService = motherService;
            _memoryValidator = new MemoryValidator();
        }

        private string babyError;

        public string BabyError
        {
            get { return babyError; }
            set
            {
                babyError = value;
                RaisePropertyChanged(nameof(BabyError));
                RaisePropertyChanged(nameof(BabyErrorVisible));
            }
        }

        public bool BabyErrorVisible
        {
            get { return !string.IsNullOrWhiteSpace(BabyError); }
        }


        private string titleError;

        public string TitleError
        {
            get { return titleError; }
            set
            {
                titleError = value;
                RaisePropertyChanged(nameof(TitleError));
                RaisePropertyChanged(nameof(TitleErrorVisible));
            }
        }

        public bool TitleErrorVisible
        {
            get { return !string.IsNullOrWhiteSpace(TitleError); }
        }

        private string descriptionError;

        public string DescriptionError
        {
            get { return descriptionError; }
            set
            {
                descriptionError = value;
                RaisePropertyChanged(nameof(DescriptionError));
                RaisePropertyChanged(nameof(DescriptionErrorVisible));
            }
        }

        public bool DescriptionErrorVisible
        {
            get { return !string.IsNullOrWhiteSpace(DescriptionError); }
        }

        private string fileError;

        public string FileError
        {
            get { return fileError; }
            set
            {
                fileError = value;
                RaisePropertyChanged(nameof(FileError));
                RaisePropertyChanged(nameof(FileErrorVisible));
            }
        }

        public bool FileErrorVisible
        {
            get { return !string.IsNullOrWhiteSpace(FileError); }
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
                if (selectedBaby != null) BabyError = "";
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

            if (((Button)control).Text == "Add a picture")
            {
                MediaFile = await MediaPicker.PickPhotoAsync();
            }
            else
            {
                MediaFile = await MediaPicker.CapturePhotoAsync();
            }

            if (MediaFile != null)
            {
                stream = await MediaFile.OpenReadAsync();
                FileError = null;
            }
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

                if (MediaFile != null)
                {
                    stream = await MediaFile.OpenReadAsync();
                    FileError = null;
                }
                else return;

                using (var newStream = File.OpenWrite(newFile))
                    await stream.CopyToAsync(newStream);

                VideoSource = MediaSource.FromFile(newFile);
            });

        public ICommand AddMemory => new Command(
            async () =>
            {
                Memory memoryToAdd = new Memory { Title = Title, Description = Description, Baby = SelectedBaby };
                if (Validate(memoryToAdd, MediaFile) && MediaFile != null)
                {
                    UserDialogs.Instance.ShowLoading("Adding memory...");
                    await _memoryService.CreateMemory(Title, Description, DateTime.Now.ToString(), MediaFile, _motherService.CurrentMother.Id.ToString(), SelectedBaby.Id.ToString(), Rotation);
                    await _motherService.AddEventToTimeLine($"A new memory was added! {(await _memoryService.GetMemories()).Last().Title}!", TimeLineCategories.MemoryAddedMessage);
                    await _motherService.RefreshCurrentMother();
                    PreviousPageModel.ReverseInit(new Memory());
                    await CoreMethods.PopPageModel(true, true);
                    UserDialogs.Instance.HideLoading();
                }
            });

        public ICommand RotateImage => new Command(
                () =>
                {
                    if (MediaFile != null)
                    {
                        if (MediaFile.ContentType.Contains("image"))
                        {
                            if (Rotation == 0) Rotation = 90;
                            else if (Rotation == 90) Rotation = 180;
                            else Rotation = 0;
                        }
                        else
                        {
                            CoreMethods.DisplayAlert("Warning", "Can't rotate a video!", "Continue");
                        }
                    }
                    else CoreMethods.DisplayAlert("Warning", "Please enter an image or video first", "Continue");
                });

        private bool Validate(Memory memory, FileResult file = null)
        {
            TitleError = "";
            DescriptionError = "";
            FileError = "";
            BabyError = "";

            var validationContext = new ValidationContext<Memory>(memory);
            var validationResult = _memoryValidator.Validate(validationContext);

            foreach (var error in validationResult.Errors)
            {
                if (error.PropertyName == nameof(memory.Title))
                {
                    TitleError = error.ErrorMessage;
                }
                if (error.PropertyName == nameof(memory.Description))
                {
                    DescriptionError = error.ErrorMessage;
                }
                if (error.PropertyName == nameof(memory.Baby))
                {
                    BabyError = error.ErrorMessage;
                }
            }

            if (file != null)
            {
                FileInfo fs = new FileInfo(MediaFile.FullPath);
                if ((fs.Length / 1048576d) > 10) FileError = "File size can't be larger than 10 mb's";
            }
            if (file == null) FileError = "Please submit an image or video";

            else FileError = "";

            return validationResult.IsValid;
        }
    }
}
