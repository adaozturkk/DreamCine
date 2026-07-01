using Microsoft.AspNetCore.Http;

namespace DreamCine.Application.Interfaces
{
    public interface IFileService
    {
        Task<string> UploadFileAsync(IFormFile file, string folderName);
    }
}