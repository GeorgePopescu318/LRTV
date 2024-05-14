using CloudinaryDotNet.Actions;

namespace LRTV.Interfaces
{
    public interface IPhotoService
    {
        Task<ImageUploadResult> AddPhotoAsync(IFormFile file);
        Task<DeletionResult> DeletPhotoAsync(string publicId);
    }
}
