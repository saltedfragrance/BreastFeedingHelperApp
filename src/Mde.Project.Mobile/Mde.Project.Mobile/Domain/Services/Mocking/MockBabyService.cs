using Firebase.Database.Query;
using Mde.Project.Mobile.Domain.Enums;
using Mde.Project.Mobile.Domain.Models;
using Mde.Project.Mobile.Domain.Services.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace Mde.Project.Mobile.Domain.Services.Mocking
{
    public class MockBabyService : IBabyService
    {
        private readonly IFireBaseService _fireBaseService;

        public MockBabyService(IFireBaseService fireBaseService)
        {
            _fireBaseService = fireBaseService;
        }

        private readonly List<Baby> babies = new List<Baby>()
        {
             new Baby{ FirstName = "Stijn jr", Height = 40, DateOfBirth = new DateTime(2019, 03, 20), Id = Guid.NewGuid(), MotherId = Guid.Parse("572a8007-46c7-44c0-ab7f-7c20d1530a2b"), Weight = 20 }
        };
        public async Task CreateBaby(string id, string firstName, double height, double weight, string motherId, string dateOfBirth)
        {
            var babyToAdd = new Baby
            {
                Id = Guid.NewGuid(),
                DateOfBirth = Convert.ToDateTime(dateOfBirth),
                FirstName = firstName,
                Height = height,
                Weight = weight,
                MotherId = new Guid(motherId),
            };

            await _fireBaseService.Client.Child(nameof(Baby)).PostAsync(JsonConvert.SerializeObject(babyToAdd));
        }

        public async Task DeleteBaby(string id)
        {
            var babyToDelete = (await _fireBaseService.Client.Child(nameof(Baby))
                                                                             .OnceAsync<Baby>())
                                                                             .Where(b => b.Object.Id.ToString() == id)
                                                                             .FirstOrDefault();

            await _fireBaseService.Client.Child(nameof(Baby)).Child(babyToDelete.Key).DeleteAsync();
        }
        public async Task<List<Baby>> GetBabies()
        {
            var babies = (await _fireBaseService.Client.Child(nameof(Baby)).OnceAsync<Baby>()).Select(m => new Baby
            {
                Id = Guid.NewGuid(),
                DateOfBirth = m.Object.DateOfBirth,
                FirstName = m.Object.FirstName,
                Height = m.Object.Height,
                Weight = m.Object.Weight,
                MotherId = m.Object.MotherId
            }).ToList();

            return babies;
        }
        public async Task<Baby> GetBaby(string id)
        {
            return (await GetBabies()).Where(b => b.Id.ToString() == id).FirstOrDefault();
        }
        public async Task UpdateBaby(string id, string firstName, string birthDate, double weight, double height)
        {
            Baby baby = new Baby
            {
                Id = new Guid(id),
                FirstName = firstName,
                DateOfBirth = Convert.ToDateTime(birthDate),
                Weight = weight,
                Height = height
            };

            var babyToUpdate = (await _fireBaseService.Client.Child(nameof(Baby))
                                                    .OnceAsync<Baby>())
                                                    .Where(t => t.Object.Id == Guid.Parse(id))
                                                    .FirstOrDefault();

            await _fireBaseService.Client.Child(nameof(Baby)).Child(babyToUpdate.Key).PutAsync(baby);
        }
    }
}
