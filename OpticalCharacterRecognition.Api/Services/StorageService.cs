using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using System;

namespace OpticalCharacterRecognition.Api.Services
{
    public class StorageService
    {
        public StorageService()
        {
            var path = System.IO.Path.Combine(Environment.CurrentDirectory, "key.json");
            var gc = GoogleCredential.FromFile(path);
            Instance = StorageClient.Create(gc);
        }

        private StorageClient Instance { get; }

        public void Upload(Microsoft.AspNetCore.Http.IFormFile file)
        {
            try
            {
                Instance.UploadObject("go-logs-304513.appspot.com", file.FileName, file.ContentType, file.OpenReadStream());
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
            }
        }
    }
}
