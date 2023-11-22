using MediatR;
using QuizWiz.Persistence.BlobStorage;

namespace QuizWiz.Application.QuizGenerator.Commands
{
    public class AddArticleCommand : IRequest<Uri>
    {
        public Stream FileStream { get; set; }
        public string FileName { get; set; }
        public AddArticleCommand(string fileName, Stream fileStream)
        {
            FileName = fileName;
            FileStream = fileStream;
        }
    }

    public class AddArticleCommandHandler : IRequestHandler<AddArticleCommand, Uri>
    {
        private readonly IBlobService _blobService;

        public AddArticleCommandHandler(IBlobService blobService)
        {
            _blobService = blobService;
        }

        public async Task<Uri> Handle(AddArticleCommand request, CancellationToken cancellationToken)
        {
            var fileBytes = StreamToByteArray(request.FileStream);

            // Generate a unique blob name or use the original file name
            var blobName = $"{Guid.NewGuid().ToString()}-{request.FileName}";

            // Upload the blob and get the URI
            var blobUri = await _blobService.CreateBlobAsync(fileBytes, blobName);

            return blobUri;
        }

        private static byte[] StreamToByteArray(Stream stream)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                stream.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }
    }
}
