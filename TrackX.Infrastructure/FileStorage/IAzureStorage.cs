using Microsoft.AspNetCore.Http;

namespace TrackX.Infrastructure.FileStorage
{
    public interface IAzureStorage
    {
        Task<string> SaveFile(string container, IFormFile file);
        Task<string> EditFile(string container, IFormFile file, string route);
        Task RemoveFile(string route, string container);
    }
}