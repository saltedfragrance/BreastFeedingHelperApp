using Mde.Project.Mobile.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace Mde.Project.Mobile.Domain.Services.Interfaces
{
    public interface IMemoryService
    {
        Task<List<Memory>> GetMemories();
        Task CreateMemory(string title, string description, string date, FileResult media, string motherId, string babyId);
        Task DeleteMemory(string id);
    }
}
