﻿using Firebase.Database.Query;
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
        public MemoryService(IFireBaseService fireBaseService)
        {
            _fireBaseService = fireBaseService;
        }

        public async Task CreateMemory(string title, string description, string date, FileResult media, string motherId, string babyId)
        {
            var memoryToAdd = new Memory
            {
                Id = Guid.NewGuid(),
                Date = Convert.ToDateTime(date),
                Description = description,
                MotherId = new Guid(motherId),
                Title = title,
                BabyId = new Guid(babyId)
            };

            //create media file name
            string fileName = $"{motherId}-{babyId}-{Guid.NewGuid()}{Path.GetExtension(media.FullPath)}";
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
                BabyId = m.Object.BabyId,
                IsMovie = m.Object.IsMovie,
                IsPicture = m.Object.IsPicture,
                MemoryUrl = m.Object.MemoryUrl
            }).ToList();

            foreach (Memory memory in memories)
            {
                var mediaSource = LoadMedia(memory.IsPicture, memory.IsMovie, memory.MemoryUrl, memories);
                if (mediaSource is ImageSource) memory.MemoryImage = (ImageSource)mediaSource;
                else memory.MemoryVideo = (MediaSource)mediaSource;
            }
            return memories;
        }

        private object LoadMedia(bool isImage, bool isVideo, string url, List<Memory> memories)
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
