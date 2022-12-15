using Firebase.Database.Query;
using Mde.Project.Mobile.Domain.Models;
using Mde.Project.Mobile.Domain.Services.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace Mde.Project.Mobile.Domain.Services
{
    public class MemoryService : IMemoryService
    {
        private readonly IFireBaseService _fireBaseService;
        public MemoryService(IFireBaseService fireBaseService)
        {
            _fireBaseService = fireBaseService;
        }

        public async Task CreateMemory(string title, string description, string date, FileResult media, string motherId, string babyId)
        {
            //create media file name
            media.FileName = $"{motherId}-{Guid.NewGuid()}";
            //upload media to firestore
            if (media.ContentType == "image/png")
            {
                await _fireBaseService.FireBaseStorage.Child("Images").Child(media.FileName).PutAsync(await media.OpenReadAsync());
            }
            else
            {
                await _fireBaseService.FireBaseStorage.Child("Videos").Child(media.FileName).PutAsync(await media.OpenReadAsync());
            }

            //add memory
            var memoryToAdd = new Memory
            {
                Id = Guid.NewGuid(),
                Date = Convert.ToDateTime(date),
                Description = description,
                MotherId = new Guid(motherId),
                Title = title,
                BabyId = new Guid(babyId)
            };

            await _fireBaseService.Client.Child(nameof(Memory)).PostAsync(JsonConvert.SerializeObject(memoryToAdd));
        }

        public async Task DeleteMemory(string id)
        {
            var memoryToDelete = (await _fireBaseService.Client.Child(nameof(Memory))
                                                                             .OnceAsync<Memory>())
                                                                             .Where(b => b.Object.Id.ToString() == id)
                                                                             .FirstOrDefault();

            await _fireBaseService.Client.Child(nameof(Memory)).Child(memoryToDelete.Key).DeleteAsync();
        }

        public async Task<List<Memory>> GetMemories()
        {
            var memories = (await _fireBaseService.Client.Child(nameof(Memory)).OnceAsync<Memory>()).Select(m => new Memory
            {
                Id = m.Object.Id,
                Date = m.Object.Date,
                Description = m.Object.Description,
                MotherId = m.Object.MotherId,
                Title = m.Object.Title,
                BabyId = m.Object.BabyId
            }).ToList();

            return memories;
        }
    }
}
