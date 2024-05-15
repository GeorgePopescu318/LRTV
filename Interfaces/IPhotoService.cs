using CloudinaryDotNet.Actions;

namespace LRTV.Interfaces
{
    public interface IPhotoService
    {
        Task<ImageUploadResult> AddPhotoAsyncPlayers(IFormFile file);

        Task<ImageUploadResult> AddPhotoAsyncNews(IFormFile file);
        Task<DeletionResult> DeletPhotoAsync(string publicId);
    }
}
