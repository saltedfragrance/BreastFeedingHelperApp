using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Mde.Project.Mobile.Domain.Services.Interfaces
{
    public interface IMediaPicker
    {
        Task<Stream> PickMedia(bool pickPicture, bool pickMovie);
    }
}
