using Mde.Project.Mobile.Domain.Enums;
using Mde.Project.Mobile.Domain.Models;
using Mde.Project.Mobile.Domain.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mde.Project.Mobile.Domain.Services.Mocking
{
    public class MockBabyService : IBabyService
    {
        private readonly List<Baby> babies = new List<Baby>()
        {
             new Baby{ FirstName = "Stijn jr", Height = 40, DateOfBirth = new DateTime(2019, 03, 20), Id = Guid.NewGuid(), MotherId = Guid.Parse("572a8007-46c7-44c0-ab7f-7c20d1530a2b"), Weight = 20 }
        };
        public async Task CreateBaby(string id, string firstName, double height, double weight, string motherId, string dateOfBirth)
        {
            babies.Add(new Baby { Id = new Guid(id), FirstName = firstName, Height = height, Weight = weight, MotherId = new Guid(motherId), DateOfBirth = Convert.ToDateTime(dateOfBirth) });
        }
        public async Task DeleteBaby(string id)
        {
            babies.RemoveAll(b => b.Id == new Guid(id));
        }
        public async Task<List<Baby>> GetBabies()
        {
            return await Task.FromResult(babies);
        }
        public async Task<Baby> GetBaby(string id)
        {
            return babies.Where(b => b.Id == new Guid(id)).FirstOrDefault();
        }
        public async Task UpdateBaby(string id, string firstName, string birthDate, double weight, double height)
        {
            Baby baby = babies.Where(b => b.Id == (new Guid(id))).FirstOrDefault();
            await CreateBaby(baby.Id.ToString() ,firstName, height, weight, baby.MotherId.ToString(), birthDate.ToString());
            babies.Remove(baby);
        }
    }
}
