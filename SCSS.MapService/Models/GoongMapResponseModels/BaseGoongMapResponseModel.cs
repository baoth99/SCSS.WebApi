using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCSS.MapService.Models.GoongMapResponseModels
{
    public class GoongMapDistanceMatrixResponseRow
    {
        public List<GoongMapDistanceMatrixResponseElement> Rows { get; set; }

    }

    public class GoongMapDistanceMatrixResponseElement
    {
        public List<DistanceMatrixResponseModel> Elements { get; set; }
    }

    public class ResponseDetail
    {
        public string Text { get; set; }

        public float Value { get; set; }
    }

}
