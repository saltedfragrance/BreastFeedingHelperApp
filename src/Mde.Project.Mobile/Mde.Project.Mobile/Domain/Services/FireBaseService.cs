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
        public FirebaseClient Client { get; } = new FirebaseClient("https://baby-a38ca-default-rtdb.europe-west1.firebasedatabase.app/");
        public FirebaseAuthProvider AuthProvider { get; } = new FirebaseAuthProvider(new FirebaseConfig("AIzaSyC6vKhpDe2-g1DfDsf_t9uFJqHnAmPxkQ0 "));

        public FirebaseStorage FireBaseStorage { get; } = new FirebaseStorage("baby-a38ca.appspot.com",
                new FirebaseStorageOptions
                {
                    ThrowOnCancel = true,
                });
    }
}
