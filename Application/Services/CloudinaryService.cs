using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Application.Interfaces;
using Application.Settings;
using SkiaSharp;

namespace Application.Services
{
    public class CloudinaryService : ICloudinaryService
    {
        private readonly Cloudinary _cloudinary;

        public CloudinaryService(IOptions<CloudinarySettings> config)
        {
            var account = new Account(
                config.Value.CloudName,
                config.Value.ApiKey,
                config.Value.ApiSecret);

            _cloudinary = new Cloudinary(account);
        }

        public async Task<ImageUploadResult> UploadImageAsync(IFormFile file)
        {
            var uploadResult = new ImageUploadResult();

            if (file.Length > 0)
            {
                using var stream = file.OpenReadStream();
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream)
                };
                uploadResult = await _cloudinary.UploadAsync(uploadParams);
            }

            return uploadResult;
        }

        public static bool IsAvatarSquareResolutionValid(IFormFile file, int minWidth, int minHeight)
        {
            using var stream = file.OpenReadStream();
            using var skBitmap = SKBitmap.Decode(stream);

            if (skBitmap.Width >= minWidth && skBitmap.Height >= minHeight)
            {
                return true;
            }

            return false;
        }
    }
}