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
                PassWord = "t"
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
    }
}
