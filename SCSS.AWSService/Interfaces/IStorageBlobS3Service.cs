using Microsoft.AspNetCore.Http;
using SCSS.AWSService.Models;
using SCSS.Utilities.Constants;
using System.Threading.Tasks;

namespace SCSS.AWSService.Interfaces
{
    public interface IStorageBlobS3Service
    {
        /// <summary>
        /// Uploads the file.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        Task<string> UploadFile(IFormFile file, string fileName, FileS3Path path);

        /// <summary>
        /// Gets the file.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        Task<FileResultModel> GetFile(string fileName, FileS3Path path);

        /// <summary>
        /// Gets the image.
        /// </summary>
        /// <param name="filepath">The filepath.</param>
        /// <returns></returns>
        Task<FileResponseModel> GetImage(string filepath);

        /// <summary>
        /// Gets the file.
        /// </summary>
        /// <param name="filepath">The filepath.</param>
        /// <returns></returns>
        Task<FileResultModel> GetFile(string filepath);
    }
}
