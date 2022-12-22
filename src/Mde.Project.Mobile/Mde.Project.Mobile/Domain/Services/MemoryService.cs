using Firebase.Database;
using Firebase.Database.Query;
using Firebase.Storage;
using Mde.Project.Mobile.Domain.Models;
using Mde.Project.Mobile.Domain.Services.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.Core;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Mde.Project.Mobile.Domain.Services
{
    public class MemoryService : IMemoryService
    {
        private readonly IFireBaseService _fireBaseService;
        private readonly IBabyService _babyService;
        public MemoryService(IFireBaseService fireBaseService, IBabyService babyService)
        {
            _fireBaseService = fireBaseService;
            _babyService = babyService;
        }

        public async Task CreateMemory(string title, string description, string date, FileResult media, string motherId, string babyId, int imageRotation)
        {
            //create media file name
            var fileName = $"{motherId}-{babyId}-{Guid.NewGuid()}{Path.GetExtension(media.FullPath)}";

            var memoryToAdd = new Memory
            {
                Id = Guid.NewGuid(),
                Date = Convert.ToDateTime(date),
                Description = description,
                MotherId = new Guid(motherId),
                Title = title,
                BabyId = new Guid(babyId),
                ImageRotation = imageRotation,
                FileName = fileName
            };

            //upload media to firestore
            if (media.ContentType.Contains("image"))
            {
                await _fireBaseService.FireBaseStorage.Child("Images").Child(fileName).PutAsync(await media.OpenReadAsync());
                memoryToAdd.MemoryUrl = await _fireBaseService.FireBaseStorage.Child("Images").Child(fileName).GetDownloadUrlAsync() + Path.GetExtension(media.FullPath);
                memoryToAdd.IsPicture = true;
                memoryToAdd.IsMovie = false;
            }
            else
            {
                memoryToAdd.MemoryUrl = await _fireBaseService.FireBaseStorage.Child("Videos").Child(fileName).PutAsync(await media.OpenReadAsync());
                memoryToAdd.IsPicture = false;
                memoryToAdd.IsMovie = true;
            }
            //add memory
            await _fireBaseService.Client.Child(nameof(Memory)).PostAsync(JsonConvert.SerializeObject(memoryToAdd));
        }
        public async Task DeleteMemory(string id)
        {
            var memoryToDelete = (await _fireBaseService.Client.Child(nameof(Memory))
                                                                             .OnceAsync<Memory>())
                                                                             .Where(b => b.Object.Id.ToString() == id)
                                                                             .FirstOrDefault();

            await _fireBaseService.FireBaseStorage.Child("Images")
                                                  .Child(memoryToDelete.Object.FileName)
                                                  .DeleteAsync();
            await _fireBaseService.Client.Child(nameof(Memory)).Child(memoryToDelete.Key).DeleteAsync();
        }

        public async Task DeleteMemories(List<string> ids)
        {
            List<FirebaseObject<Memory>> memoriesToDelete = new List<FirebaseObject<Memory>>();

            foreach (string id in ids)
            {
                var memoryToDelete = (await _fireBaseService.Client.Child(nameof(Memory))
                                                                             .OnceAsync<Memory>())
                                                                             .Where(b => b.Object.Id.ToString() == id)
                                                                             .FirstOrDefault();
                memoriesToDelete.Add(memoryToDelete);
            }

            memoriesToDelete.ForEach(async (m) =>
            {
                await _fireBaseService.FireBaseStorage.Child("Images")
                                      .Child(m.Object.FileName)
                                      .DeleteAsync();
            });

            memoriesToDelete.ForEach(async (m) =>
            {
                await _fireBaseService.Client.Child(nameof(Memory)).Child(m.Key).DeleteAsync();
            });
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
                BabyId = m.Object.BabyId,
                IsMovie = m.Object.IsMovie,
                IsPicture = m.Object.IsPicture,
                MemoryUrl = m.Object.MemoryUrl,
                ImageRotation = m.Object.ImageRotation
            }).ToList();

            foreach (Memory memory in memories)
            {
                var mediaSource = LoadMedia(memory.IsPicture, memory.MemoryUrl);
                if (mediaSource is ImageSource) memory.MemoryImage = (ImageSource)mediaSource;
                else memory.MemoryVideo = (MediaSource)mediaSource;
                memory.Baby = (await _babyService.GetBabies()).Where(b => b.Id == memory.BabyId).FirstOrDefault();
            }

            return memories;
        }

        private object LoadMedia(bool isImage, string url)
        {
            ImageSource img = null;
            MediaSource video = null;
            var webClient = new WebClient();
            byte[] imgBytes = webClient.DownloadData(url);
            if (isImage)
            {
                img = ImageSource.FromStream(() => new MemoryStream(imgBytes));
            }
            else
            {
                video = MediaSource.FromUri(url);
            }

            if (video != null) { return video; }
            else return img;
        }
    }
}
