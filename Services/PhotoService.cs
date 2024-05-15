using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using LRTV.Helpers;
using LRTV.Interfaces;
using Microsoft.Extensions.Options;

namespace LRTV.Services
{
    public class PhotoService : IPhotoService
    {
        private readonly Cloudinary _cloudinary;
        public PhotoService(IOptions<CloudinarySettings> config)
        {
            var acc = new Account(
                config.Value.CloudName,
                config.Value.ApiKey,
                config.Value.ApiSecret
                );
            _cloudinary = new Cloudinary( acc );
            
        }
        public async Task<ImageUploadResult> AddPhotoAsyncPlayers(IFormFile file)
        {
            //using var newStream = new MemoryStream(File.ReadAllBytes().ToArray());
            //var formFile = new FormFile(newStream, 0, newStream.Length, "streamFile", "");
            var uploadResult = new ImageUploadResult();
         
            if (file.Length > 0)
            {
                using var stream = file.OpenReadStream();
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    Transformation = new Transformation().Height(200).Width(200).Crop("crop").Gravity("face")
                };
                uploadResult = await _cloudinary.UploadAsync(uploadParams);

            }
           return uploadResult;     
        }

        public async Task<ImageUploadResult> AddPhotoAsyncNews(IFormFile file)
        {
            //using var newStream = new MemoryStream(File.ReadAllBytes().ToArray());
            //var formFile = new FormFile(newStream, 0, newStream.Length, "streamFile", "");
            var uploadResult = new ImageUploadResult();

            if (file.Length > 0)
            {
                using var stream = file.OpenReadStream();
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    Transformation = new Transformation().Height(600).Width(800).Crop("crop").Gravity("face")
                };
                uploadResult = await _cloudinary.UploadAsync(uploadParams);

            }
            return uploadResult;
        }

        public async Task<ImageUploadResult> AddPhotoAsyncTeams(IFormFile file)
        {
            //using var newStream = new MemoryStream(File.ReadAllBytes().ToArray());
            //var formFile = new FormFile(newStream, 0, newStream.Length, "streamFile", "");
            var uploadResult = new ImageUploadResult();

            if (file.Length > 0)
            {
                using var stream = file.OpenReadStream();
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    Transformation = new Transformation().Height(200).Width(200).Crop("crop").Gravity("face")
                };
                uploadResult = await _cloudinary.UploadAsync(uploadParams);

            }
            return uploadResult;
        }

        public async Task<DeletionResult> DeletPhotoAsync(string publicId)
        {
            var deleteParams = new DeletionParams(publicId);
            var result = await _cloudinary.DestroyAsync(deleteParams);

            return result;
        }
    }
}
