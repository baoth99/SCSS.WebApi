using Microsoft.AspNetCore.Http;
using SCSS.AWSService.Models;
using SCSS.Utilities.Constants;
using SCSS.Utilities.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCSS.AWSService.Interfaces
{
    public interface IStorageBlobS3Service
    {
        Task<string> UploadFile(IFormFile file, string fileName, FileS3Path path);

        Task<FileViewModel> GetFile(string fileName, FileS3Path path);

        Task<BaseApiResponseModel> GetImage(string filepath);

        Task<FileViewModel> GetFileByUrl(string filepath);
    }
}
