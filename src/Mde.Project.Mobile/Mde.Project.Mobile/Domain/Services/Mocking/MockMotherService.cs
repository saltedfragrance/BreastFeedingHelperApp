using Mde.Project.Mobile.Domain.Enums;
using Mde.Project.Mobile.Domain.Models;
using Mde.Project.Mobile.Domain.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace Mde.Project.Mobile.Domain.Services.Mocking
{
    public class MockMotherService : IMotherService
    {
        public Mother CurrentMother { get; set; }
        private List<Mother> mothers = new List<Mother>
        {
            new Mother
            {
                Id = new Guid("572a8007-46c7-44c0-ab7f-7c20d1530a2b"),
                Email = "t@t.com",
                FirstName = "Angelina",
                LastName = "Jolie",
                MidWifePhoneNumber = 0497554433,
                PassWord = "t",
                TimeLine = new TimeLine{ Events = new List<Event>
                {
                    new Event { Id = Guid.NewGuid(), Date = new DateTime(2022, 11, 2, 2, 20, 30), Description = "Pumped 30ml of breast milk", Category = TimeLineCategories.PumpingMessage, Image = "timelinepumping.png"},
                    new Event { Id = Guid.NewGuid(), Date = new DateTime(2022, 11, 2, 7, 40, 10), Description = "Pumped 50ml of breast milk", Category = TimeLineCategories.PumpingMessage, Image = "timelinepumping.png"},
                    new Event { Id = Guid.NewGuid(), Date = new DateTime(2022, 12, 4, 5, 20, 30), Description = "Stijn jr grew by 1cm!", Category = TimeLineCategories.BabyHeightGainMessage, Image = "timelinebabygrowing.png"},
                },
                Id = new Guid("8468bb0b-607b-4fd7-81f1-aa60f132ffb5"), MotherId = new Guid("6286c349-107d-4e04-a118-a78aa37a5c52")},
                 Babies = new List<Baby>
                 {
                     new Baby{ FirstName = "Stijn jr", Height = 40, DateOfBirth = new DateTime(2019, 03, 20), Id = Guid.NewGuid(), MotherId = Guid.Parse("572a8007-46c7-44c0-ab7f-7c20d1530a2b"), Weight = 20 }
                 }
            }
        };

        public async Task CreateMother(string firstName, string lastName, string email, string passWord, int midWifePhoneNumber)
        {
            mothers.Add(new Mother { FirstName = firstName, LastName = lastName, Email = email, PassWord = passWord, MidWifePhoneNumber = midWifePhoneNumber });
        }

        public async Task<List<Mother>> GetMothers()
        {
            return await Task.FromResult(mothers);
        }

        public Task<Mother> UpdateMother(string id)
        {
            throw new NotImplementedException();
        }
        public Task<string> AddEventToTimeLine(string eventMessage, TimeLineCategories messageCategory)
        {
            CurrentMother.TimeLine.Events.Add(new Event { Id = Guid.NewGuid(), Description = eventMessage, Date = DateTime.Now, Category = messageCategory, Image = DetermineImage(messageCategory) });
            return Task.FromResult(eventMessage);
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
    }
}
