using Firebase.Database;
using Firebase.Database.Query;
using Mde.Project.Mobile.Domain.Enums;
using Mde.Project.Mobile.Domain.Models;
using Mde.Project.Mobile.Domain.Services.Interfaces;
using Mde.Project.Mobile.Pages;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms.Internals;

namespace Mde.Project.Mobile.Domain.Services
{
    public class MotherService : IMotherService
    {
        private readonly IFireBaseService _fireBaseService;
        public Mother CurrentMother { get; set; }
        public MotherService(IFireBaseService fireBaseService)
        {
            _fireBaseService = fireBaseService;
        }
        public async Task CreateMother(string firstName, string lastName, string email, string passWord, int midWifePhoneNumber)
        {
            var motherToAdd = new Mother
            {
                Id = Guid.NewGuid(),
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                PassWord = passWord,
                MidWifePhoneNumber = midWifePhoneNumber,
                TimeLineId = Guid.NewGuid()
            };

            var timeLineToAdd = new TimeLine
            {
                Id = motherToAdd.TimeLineId,
                MotherId = motherToAdd.Id
            };

            await _fireBaseService.Client.Child(nameof(TimeLine)).PostAsync(JsonConvert.SerializeObject(timeLineToAdd));
            await _fireBaseService.Client.Child(nameof(Mother)).PostAsync(JsonConvert.SerializeObject(motherToAdd));
        }

        public async Task<List<Mother>> GetMothers()
        {

            var mothers = (await _fireBaseService.Client.Child(nameof(Mother)).OnceAsync<Mother>()).Select(m => new Mother
            {
                Id = m.Object.Id,
                FirstName = m.Object.FirstName,
                LastName = m.Object.LastName,
                Email = m.Object.Email,
                PassWord = m.Object.PassWord,
                MidWifePhoneNumber = m.Object.MidWifePhoneNumber,
                TimeLineId = m.Object.TimeLineId
            }).ToList();


            foreach (Mother mother in mothers)
            {
                var timeLines = (await _fireBaseService.Client.Child(nameof(TimeLine)).OnceAsync<TimeLine>()).Select(t => new TimeLine
                {
                    Id = t.Object.Id,
                    MotherId = t.Object.MotherId
                }).ToList();

                var timeLineId = timeLines.Where(t => t.Id == mother.Id)
                                              .Select(t => t.Id)
                                              .FirstOrDefault();

                mother.TimeLine = new TimeLine
                {
                    Id = timeLineId,
                    MotherId = mother.Id
                };
            }
            return mothers;
        }

        public async Task UpdateMother(string id, Mother mother)
        {
            var motherToUpdate = (await _fireBaseService.Client.Child(nameof(Mother))
                                                    .OnceAsync<Mother>())
                                                    .Where(t => t.Object.Id == Guid.Parse(id))
                                                    .FirstOrDefault();

            await _fireBaseService.Client.Child(nameof(Mother)).Child(motherToUpdate.Key).PutAsync(mother);
            await RefreshCurrentMother();
        }
        public async Task AddEventToTimeLine(string eventMessage, TimeLineCategories messageCategory)
        {
            var timeLineToUpdate = (await _fireBaseService.Client.Child(nameof(TimeLine))
                                                                .OnceAsync<TimeLine>())
                                                                .Where(t => t.Object.MotherId == CurrentMother.Id)
                                                                .FirstOrDefault();

            Event timeLineEvent = new Event()
            {
                Id = Guid.NewGuid(),
                Description = eventMessage,
                Category = messageCategory,
                Date = DateTime.Now,
                Image = DetermineImage(messageCategory)
            };

            await _fireBaseService.Client.Child(nameof(TimeLine)).Child(timeLineToUpdate.Key).Child("Events").Child(timeLineEvent.Id.ToString()).PutAsync(JsonConvert.SerializeObject(timeLineEvent));
            await RefreshCurrentMother();
        }

        private string DetermineImage(TimeLineCategories messageCategory)
        {
            if (messageCategory == TimeLineCategories.BreastFeedingMessage) return "timelinebreastfeeding.png";
            else if (messageCategory == TimeLineCategories.PumpingMessage) return "timelinepumping.png";
            else if (messageCategory == TimeLineCategories.MemoryAddedMessage) return "timelinememory.png";
            else if (messageCategory == TimeLineCategories.AddedBabyMessage) return "timelinenewborn.png";
            else if (messageCategory == TimeLineCategories.BabyHeightGainMessage) return "timelinebabygrowing.png";
            else return "timelinebabyweightgain.png";
        }

        private async Task RefreshCurrentMother()
        {
            var motherToRefresh = (await _fireBaseService.Client.Child(nameof(Mother)).OnceAsync<Mother>()).Where(m => m.Object.Id == CurrentMother.Id).FirstOrDefault();



            var timeLineOfMother = (await _fireBaseService.Client.Child(nameof(TimeLine)).OnceAsync<JObject>());

            //TimeLine timeLine = JObject.Parse(timeLineOfMother.ToString()).SelectToken("$.Events[?(@.]")


            var bla = timeLineOfMother.Select(p => p.Object.GetValue("Events").Root).ToList();
            List<Event> events = new List<Event>();
            foreach (JToken blak in bla)
            {
                var black = blak.ToObject<Event>();
                events.Add(black);
            }
            
            //.Where(t => t.Object.Id == CurrentMother.Id)
            //.Select(p => new TimeLine
            //{
            //    Events = p.Object.Events.Select(e => new Event
            //    {
            //        Id = e.Id,
            //        Category = (TimeLineCategories)e.Category,
            //        Date = Convert.ToDateTime(e.Date),
            //        Description = e.Description

            //    }).ToList(),
            //    Id = p.Object.Id,
            //    MotherId = p.Object.MotherId,
            //}).FirstOrDefault();



            CurrentMother = new Mother()
            {
                Id = motherToRefresh.Object.Id,
                Email = motherToRefresh.Object.Email,
                FirstName = motherToRefresh.Object.FirstName,
                LastName = motherToRefresh.Object.LastName,
                MidWifePhoneNumber = motherToRefresh.Object.MidWifePhoneNumber,
                TimeLine = new TimeLine
                {
                    Id = motherToRefresh.Object.TimeLineId,
                    MotherId = motherToRefresh.Object.Id,
                },
                TimeLineId = motherToRefresh.Object.TimeLineId,
            };
        }
    }
}
