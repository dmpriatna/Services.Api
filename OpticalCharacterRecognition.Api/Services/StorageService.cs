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

            Bucket = "go-logs-304513.appspot.com";
            Client = StorageClient.Create(gc);
            Signer = UrlSigner.FromServiceAccountPath(path);
        }

        private string Bucket { get; }

        private StorageClient Client { get; }

        private UrlSigner Signer { get; }

        public Google.Apis.Storage.v1.Data.Object Upload(Microsoft.AspNetCore.Http.IFormFile file)
        {
            try
            {
                return Client.UploadObject(Bucket, file.FileName, file.ContentType, file.OpenReadStream());
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public string Download(string objectName)
        {
            try
            {
                return Signer.Sign(Bucket, objectName, TimeSpan.FromHours(1), System.Net.Http.HttpMethod.Get);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
