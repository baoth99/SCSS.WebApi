using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Http;
using SCSS.AWSService.Interfaces;
using SCSS.AWSService.Models;
using SCSS.Utilities.Configurations;
using SCSS.Utilities.Constants;
using SCSS.Utilities.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCSS.AWSService.Implementations
{
    public class StorageBlobS3Service : IStorageBlobS3Service
    {
        private readonly IAmazonS3 _amazonS3;

        public StorageBlobS3Service()
        {
            _amazonS3 = new AmazonS3Client(AppSettingValues.AWSS3AccessKey, AppSettingValues.AWSS3SecretKey, RegionEndpoint.APSoutheast1);
        }

        public async Task<string> UploadFile(IFormFile file,string fileName , FileS3Path path)
        {
            var putRequest = new PutObjectRequest()
            {
                BucketName = AppSettingValues.AWSS3BucketName,
                Key = $"{path}/{fileName}",
                InputStream = file.OpenReadStream(),
                ContentType = file.ContentType,
            };

            var result = await _amazonS3.PutObjectAsync(putRequest);

            return fileName;
        }

        public async Task<FileViewModel> GetFile(string fileName, FileS3Path path)
        {
            var request = new GetObjectRequest()
            {
                BucketName = AppSettingValues.AWSS3BucketName,
                Key = $"{path}/{fileName}"
            };

            var response = await _amazonS3.GetObjectAsync(request);

            var stream = response.ResponseStream;

            return new FileViewModel()
            {
                Extension = Path.GetExtension(fileName),
                Base64 = stream.ToBase64()
            };
        }
    }
}
