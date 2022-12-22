using Mde.Project.Mobile.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.Core;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Mde.Project.Mobile.Domain.Services.Interfaces
{
    public interface IMemoryService
    {
        Task<List<Memory>> GetMemories();
        Task CreateMemory(string title, string description, string date, FileResult movie, string motherId, string babyId, int imageRotation);
        //Task CreateMemory(string title, string description, string date, ImageSource image, string motherId, string babyId);
        Task DeleteMemory(string id);
    }
}
