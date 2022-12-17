using Firebase.Auth;
using Firebase.Database;
using Firebase.Storage;
using Mde.Project.Mobile.Domain.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mde.Project.Mobile.Domain.Services
{
    public class FireBaseService : IFireBaseService
    {
        public FirebaseClient Client { get; } = new FirebaseClient("https://babytracker-9df68-default-rtdb.europe-west1.firebasedatabase.app/");
        public FirebaseAuthProvider AuthProvider { get; } = new FirebaseAuthProvider(new FirebaseConfig("AIzaSyCwbYQx5eBLQU4ZCC6OTXyuOpwkS0iSlvM"));

        public FirebaseStorage FireBaseStorage { get; } = new FirebaseStorage("babytracker-9df68.appspot.com",
                new FirebaseStorageOptions
                {
                    ThrowOnCancel = true,
                });
    }
}
