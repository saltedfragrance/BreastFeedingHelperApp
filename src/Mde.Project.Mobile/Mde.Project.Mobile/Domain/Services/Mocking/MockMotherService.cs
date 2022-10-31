﻿using Mde.Project.Mobile.Domain.Models;
using Mde.Project.Mobile.Domain.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Mde.Project.Mobile.Domain.Services.Mocking
{
    public class MockMotherService : IMotherService
    {
        private static List<Mother> _motherList = new List<Mother>
        {
            new Mother
            {
                Id = Guid.Parse("111111111-1111-11111-11111111111111"),
                Email = "testmother@test.com",
                FirstName = "Angelina",
                LastName = "Jolie",
                MidWifePhoneNumber = 0497554433,
                PassWord = "test",
                UserName = "TestMother"
            }
        };
        public Task<Mother> CreateMother(Mother mother)
        {
            throw new NotImplementedException();
        }

        public Task<Mother> GetMother(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Login(string email, string password)
        {
            if (email == "testmother@test.com") return await Task.FromResult(true);
            else return await Task.FromResult(false);
        }

        public Task<bool> Logout()
        {
            throw new NotImplementedException();
        }

        public Task<bool> Register(string firstName, string lastName, string userName, string email, string passWord, int midWifePhoneNumber, string location)
        {
            throw new NotImplementedException();
        }

        public Task<Mother> UpdateMother(Mother mother)
        {
            throw new NotImplementedException();
        }
    }
}
