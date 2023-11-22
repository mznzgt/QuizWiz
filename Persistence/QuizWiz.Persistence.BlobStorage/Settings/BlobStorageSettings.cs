using Azure.Storage.Blobs.Models;

namespace QuizWiz.Persistence.BlobStorage.Settings
{
    public class BlobStorageSettings
    {
        public string ConnectionString { get; set; }
        public string BlobContainerName { get; set; }
        public PublicAccessType PublicAccessType { get; set; } = PublicAccessType.Blob;
    }
}
