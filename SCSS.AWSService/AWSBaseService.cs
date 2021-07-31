using Amazon;
using Amazon.S3;
using SCSS.Utilities.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCSS.AWSService
{
    public class AWSBaseService
    {
        protected readonly IAmazonS3 _amazonS3;

        public AWSBaseService()
        {
            _amazonS3 = new AmazonS3Client(AppSettingValues.AWSS3AccessKey, AppSettingValues.AWSS3SecretKey, RegionEndpoint.APSoutheast1);
        }
    }
}
