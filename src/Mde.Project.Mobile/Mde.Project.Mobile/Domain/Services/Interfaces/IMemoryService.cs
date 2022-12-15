using Mde.Project.Mobile.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Mde.Project.Mobile.Domain.Services.Interfaces
{
    public interface IMemoryService
    {
        Task CreateMemory(string title, string description, string date, string mediaUri, int motherId);
        Task DeleteMemory(string id);
    }
}
