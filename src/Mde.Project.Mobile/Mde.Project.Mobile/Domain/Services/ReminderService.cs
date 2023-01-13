using Mde.Project.Mobile.Domain.Models;
using Mde.Project.Mobile.Domain.Services.Interfaces;
using Newtonsoft.Json;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Firebase.Database.Query;

namespace Mde.Project.Mobile.Domain.Services
{
    public class ReminderService : IReminderService
    {
        private readonly IFireBaseService _fireBaseService;

        public ReminderService(IFireBaseService fireBaseService)
        {
            _fireBaseService = fireBaseService;
        }

        public async Task AddReminder(string id, string motherId, string intervalTime, string title, string message, string type)
        {
            var toAdd = new Reminder
            {
                Id = new Guid(id),
                MotherId = motherId,
                IntervalTime = intervalTime,
                Title = title,
                Message = message,
                Type = type
            };

            await _fireBaseService.Client.Child(nameof(Reminder)).PostAsync(JsonConvert.SerializeObject(toAdd));
        }

        public async Task RemoveReminder(string id)
        {
            var toDelete = (await _fireBaseService.Client.Child(nameof(Reminder))
                                                                             .OnceAsync<Reminder>())
                                                                             .Where(b => b.Object.Id.ToString() == id)
                                                                             .FirstOrDefault();

            await _fireBaseService.Client.Child(nameof(Reminder)).Child(toDelete.Key).DeleteAsync();
        }

        public async Task<List<Reminder>> GetAll(string motherId)
        {
            return (await _fireBaseService.Client.Child(nameof(Reminder))
                                                                           .OnceAsync<Reminder>())
                                                                           .Where(b => b.Object.MotherId == motherId).Select(r => new Reminder
                                                                           {
                                                                               Id = r.Object.Id,
                                                                               Type = r.Object.Type,
                                                                               IntervalTime = r.Object.IntervalTime,
                                                                               Message = r.Object.Message,
                                                                               MotherId = r.Object.MotherId,
                                                                               Title = r.Object.Title
                                                                           }).ToList();
        }

        public async Task<Reminder> GetFeedingReminder(string motherId)
        {
            return (await _fireBaseService.Client.Child(nameof(Reminder))
                                                                             .OnceAsync<Reminder>())
                                                                             .Where(b => b.Object.MotherId == motherId
                                                                             && b.Object.Type == "FeedingReminderType").Select(r => new Reminder
                                                                             {
                                                                                 Id = r.Object.Id,
                                                                                 Type = r.Object.Type,
                                                                                 IntervalTime = r.Object.IntervalTime,
                                                                                 Message = r.Object.Message,
                                                                                 MotherId = r.Object.MotherId,
                                                                                 Title = r.Object.Title
                                                                             }).FirstOrDefault();
        }
        public async Task<Reminder> GetPumpingReminder(string motherId)
        {
            return (await _fireBaseService.Client.Child(nameof(Reminder))
                                                                             .OnceAsync<Reminder>())
                                                                             .Where(b => b.Object.MotherId == motherId
                                                                             && b.Object.Type == "PumpingReminderType").Select(r => new Reminder
                                                                             {
                                                                                 Id = r.Object.Id,
                                                                                 Type = r.Object.Type,
                                                                                 IntervalTime = r.Object.IntervalTime,
                                                                                 Message = r.Object.Message,
                                                                                 MotherId = r.Object.MotherId,
                                                                                 Title = r.Object.Title
                                                                             }).FirstOrDefault();
        }
    }
}
