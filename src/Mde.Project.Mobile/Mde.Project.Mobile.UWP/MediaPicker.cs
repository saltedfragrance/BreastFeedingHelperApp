using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mde.Project.Mobile.Domain.Services.Interfaces;
using Mde.Project.Mobile.UWP;
using Windows.Storage;
using Windows.Storage.Pickers;
using Xamarin.Forms;

[assembly: Dependency(typeof(MediaPicker))]
namespace Mde.Project.Mobile.UWP
{

    public class MediaPicker : IMediaPicker
    {
        public async Task<Stream> PickMedia(bool pickPicture, bool pickMovie)
        {
            FileOpenPicker openPicker = new FileOpenPicker();
            if (pickPicture)
            {
                openPicker.FileTypeFilter.Add(".jpg");
                openPicker.FileTypeFilter.Add(".jpeg");
                openPicker.FileTypeFilter.Add(".png");
                openPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            }
            else
            {
                openPicker.FileTypeFilter.Add(".mp4");
                openPicker.SuggestedStartLocation = PickerLocationId.VideosLibrary;
            }
            openPicker.ViewMode = PickerViewMode.Thumbnail;


            var file = await openPicker.PickSingleFileAsync();
            return await file.OpenStreamForReadAsync();
        }
    }
}
