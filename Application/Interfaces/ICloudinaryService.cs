using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;

namespace Application.Interfaces;

public interface ICloudinaryService
{
    Task<ImageUploadResult> UploadImageAsync(IFormFile file);
}