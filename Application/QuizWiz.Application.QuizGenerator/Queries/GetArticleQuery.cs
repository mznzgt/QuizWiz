using MediatR;
using QuizWiz.Persistence.BlobStorage;

namespace QuizWiz.Application.QuizGenerator.Queries
{
    public class GetArticleQuery : IRequest<Stream>
    {
        public string BlobName { get; set; }

        public GetArticleQuery(string blobName)
        {
            BlobName = blobName;
        }
    }

    public class GetArticleQueryHandler : IRequestHandler<GetArticleQuery, Stream>
    {
        private readonly IBlobService _blobService;

        public GetArticleQueryHandler(IBlobService blobService)
        {
            _blobService = blobService;
        }

        public async Task<Stream> Handle(GetArticleQuery request, CancellationToken cancellationToken)
        {
            var stream = await _blobService.DownloadBlobAsync(request.BlobName);

            return stream;
        }
    }
}
