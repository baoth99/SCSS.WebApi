using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Http;
using SCSS.AWSService.Interfaces;
using SCSS.AWSService.Models;
using SCSS.Utilities.BaseResponse;
using SCSS.Utilities.Configurations;
using SCSS.Utilities.Constants;
using SCSS.Utilities.Extensions;
using SCSS.Utilities.ResponseModel;
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

            return $"{path}/{fileName}";
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

        public async Task<BaseApiResponseModel> GetImage(string filepath)
        {
            var path = filepath.Split("/")[0];

            if (!CollectionConstants.FileS3PathCollection.Contains(path))
            {
                return BaseApiResponse.Error(SystemMessageCode.DataInvalid);
            }
            var file = filepath.Split("/")[1];

            if (!CollectionConstants.ImageExtensions.Contains(Path.GetExtension(file.ToLower())))
            {
                return BaseApiResponse.Error(SystemMessageCode.DataInvalid);
            }

            var request = new GetObjectRequest()
            {
                BucketName = AppSettingValues.AWSS3BucketName,
                Key = filepath
            };

            var response = await _amazonS3.GetObjectAsync(request);

            var stream = response.ResponseStream;

            var fileResponse = new FileViewModel()
            {
                Extension = Path.GetExtension(filepath).ToLower(),
                Base64 = stream.ToBase64()
            };

            return BaseApiResponse.OK(fileResponse);
        }
    }
}
