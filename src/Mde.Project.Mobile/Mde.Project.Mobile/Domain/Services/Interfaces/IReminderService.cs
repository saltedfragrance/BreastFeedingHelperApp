using Mde.Project.Mobile.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Mde.Project.Mobile.Domain.Services.Interfaces
{
    public interface IReminderService
    {
        Task AddReminder(string id, string motherId,string interval, string title, string message, string type);
        Task RemoveReminder(string id);
        Task<Reminder> GetFeedingReminder(string motherId);
        Task<Reminder> GetPumpingReminder(string motherId);
    }
}
