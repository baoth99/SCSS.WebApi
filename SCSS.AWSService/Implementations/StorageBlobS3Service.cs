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
using System.IO;
using System.Threading.Tasks;

namespace SCSS.AWSService.Implementations
{
    public class StorageBlobS3Service : AWSBaseService ,IStorageBlobS3Service
    {
        #region Services

        /// <summary>
        /// The amazon s3
        /// </summary>
        private readonly IAmazonS3 AmazonS3;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="StorageBlobS3Service"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public StorageBlobS3Service(ILoggerService logger) : base(logger)
        {
            AmazonS3 = new AmazonS3Client(AppSettingValues.AWSS3AccessKey, AppSettingValues.AWSS3SecretKey, RegionEndpoint.APSoutheast1);
        }

        #endregion

        #region Upload File

        /// <summary>
        /// Uploads the file.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public async Task<string> UploadFile(IFormFile file, string fileName, FileS3Path path)
        {
            try
            {
                var putRequest = new PutObjectRequest()
                {
                    BucketName = AppSettingValues.AWSS3BucketName,
                    Key = $"{path}/{fileName}",
                    InputStream = file.OpenReadStream(),
                    ContentType = file.ContentType,
                };

                await AmazonS3.PutObjectAsync(putRequest);
                Logger.LogInfo(AWSLoggerMessage.UploadFileSuccess(fileName, path));

                return $"{path}/{fileName}";
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, AWSLoggerMessage.UploadFileFail(fileName, path));
                return string.Empty;
            } 
            finally
            {
                AmazonS3.Dispose();
            }
        }

        #endregion

        #region GetFile

        /// <summary>
        /// Gets the file.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public async Task<FileViewModel> GetFile(string fileName, FileS3Path path)
        {
            try
            {
                var request = new GetObjectRequest()
                {
                    BucketName = AppSettingValues.AWSS3BucketName,
                    Key = $"{path}/{fileName}"
                };

                var response = await AmazonS3.GetObjectAsync(request);

                var stream = response.ResponseStream;
                var base64 = stream.ToBase64();

                Logger.LogInfo(AWSLoggerMessage.GetFileSuccess(fileName, path));
                return new FileViewModel()
                {
                    Extension = Path.GetExtension(fileName),
                    Base64 = base64
                };
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, AWSLoggerMessage.GetFileFail(fileName, path));
                return null;
            }
            finally
            {
                AmazonS3.Dispose();
            }         
        }

        #endregion

        #region Get File By URL

        /// <summary>
        /// Gets the file.
        /// </summary>
        /// <param name="filepath">The filepath.</param>
        /// <returns></returns>
        public async Task<FileViewModel> GetFile(string filepath)
        {
            try
            {
                var path = filepath.Split("/")[0];

                if (!CollectionConstants.FileS3PathCollection.Contains(path))
                {
                    return null;
                }
                var file = filepath.Split("/")[1];

                if (!CollectionConstants.ImageExtensions.Contains(Path.GetExtension(file.ToLower())))
                {
                    return null;
                }

                var request = new GetObjectRequest()
                {
                    BucketName = AppSettingValues.AWSS3BucketName,
                    Key = filepath
                };

                var response = await AmazonS3.GetObjectAsync(request);

                var stream = response.ResponseStream;
                var base64 = stream.ToBase64();

                var fileResponse = new FileViewModel()
                {
                    Extension = Path.GetExtension(filepath).ToLower().Substring(1),
                    Base64 = base64
                };

                Logger.LogInfo(AWSLoggerMessage.GetFileSuccess(filepath));

                return fileResponse;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, AWSLoggerMessage.GetFileFail(filepath));
                return null;
            }
            finally
            {
                AmazonS3.Dispose();
            }
        }

        #endregion

        #region Get Image

        /// <summary>
        /// Gets the image.
        /// </summary>
        /// <param name="filepath">The filepath.</param>
        /// <returns></returns>
        public async Task<BaseApiResponseModel> GetImage(string filepath)
        {
            try
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

                var response = await AmazonS3.GetObjectAsync(request);

                var stream = response.ResponseStream;
                var base64 = stream.ToBase64();
                var fileResponse = new FileViewModel()
                {
                    Extension = Path.GetExtension(filepath).ToLower().Substring(1),
                    Base64 = base64
                };

                Logger.LogInfo(AWSLoggerMessage.GetFileSuccess(filepath));

                return BaseApiResponse.OK(fileResponse);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, AWSLoggerMessage.GetFileFail(filepath));
                return BaseApiResponse.Error();
            }
        }

        #endregion

    }
}
