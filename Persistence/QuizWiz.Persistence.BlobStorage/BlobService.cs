using Azure;
using Azure.Storage.Blobs;
using QuizWiz.Persistence.BlobStorage.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizWiz.Persistence.BlobStorage
{
    public interface IBlobService
    {
        Task<Uri> CreateBlobAsync(byte[] file, string name);
        Task<Stream> DownloadBlobAsync(string blobName);
    }
    public class BlobService : IBlobService
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly BlobStorageSettings _settings;

        public BlobService(BlobServiceClient blobServiceClient, BlobStorageSettings settings)
        {
            _blobServiceClient = blobServiceClient;
            _settings = settings;
        }

        public async Task<Uri> CreateBlobAsync(byte[] file, string name)
        {
            try
            {
                var blobClient = await GetBlobClientAsync(name);

                using (var stream = new MemoryStream(file))
                {
                    await blobClient.UploadAsync(stream, true);
                }

                return blobClient.Uri;
            }
            catch (Exception ex)
            {
                throw new RequestFailedException($"Error uploading blob: {ex.Message}");
            }
        }

        public async Task<Stream> DownloadBlobAsync(string blobName)
        {
            try
            {
                var blobClient = await GetBlobClientAsync(blobName);

                var response = await blobClient.DownloadStreamingAsync();

                var blobInfo = response.Value.Content;

                return blobInfo;
            }
            catch (Exception ex)
            {
                throw new FileNotFoundException($"Could not find {blobName}: {ex.Message}");
            }
        }

        private async Task<BlobClient>GetBlobClientAsync(string blobname)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(_settings.BlobContainerName);

            await containerClient.CreateIfNotExistsAsync();

            var blobClient = containerClient.GetBlobClient(blobname);

            return blobClient;
        }
    }
}
