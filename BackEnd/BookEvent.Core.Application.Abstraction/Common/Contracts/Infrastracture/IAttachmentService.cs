using Microsoft.AspNetCore.Http;

namespace BookEvent.Core.Application.Abstraction.Common.Contracts.Infrastracture
{
    public interface IAttachmentService
    {
        Task<string?> UploadAsynce(IFormFile file, string folderName);



        bool Delete(string filePath);
    }
}
