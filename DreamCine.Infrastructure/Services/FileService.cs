using DreamCine.Application.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace DreamCine.Infrastructure.Services
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _env;

        public FileService(IWebHostEnvironment env)
        {
            _env = env;
        }

        public async Task<string> UploadFileAsync(IFormFile file, string folderName)
        {
            if (file == null || file.Length == 0)
            {
                throw new ArgumentException("Invalid or empty file was uploaded.");
            }

            if (file.Length > 5 * 1024 * 1024)
            {
                throw new ArgumentException("File size cannot exceed 5 MB.");
            }

            var allowedExtension = new[]{ ".jpg", ".jpeg", ".png", ".webp" };
            string fileExtension = Path.GetExtension(file.FileName).ToLower();
            if (!allowedExtension.Contains(fileExtension))
            {
                throw new ArgumentException("Only .jpg, .jpeg, .png, and .webp files are allowed.");
            }

            string uniqueFileName = Guid.NewGuid().ToString() + fileExtension;

            string uploadsFolder = Path.Combine(_env.WebRootPath, folderName);
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            return uniqueFileName;
        }
    }
}
