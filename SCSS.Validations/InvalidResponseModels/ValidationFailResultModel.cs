using Microsoft.AspNetCore.Mvc.ModelBinding;
using SCSS.Utilities.Constants;
using SCSS.Utilities.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCSS.Validations.InvalidResponseModels
{
    public class ValidationFailResultModel : ValidationFailResonseModel
    {
        public ValidationFailResultModel(ModelStateDictionary modelState)
        {
            IsSuccess = BooleanConstants.FALSE;
            StatusCode = HttpStatusCodes.Ok;
            MessageCode = SystemMessageCode.DataInvalid;
            MessageDetail = "Validation Failed";
            Data = modelState.Keys
                    .SelectMany(key => modelState[key].Errors.Select(x => new ValidationError(key, 0, x.ErrorMessage)))
                    .ToList();
            Total = 0;
        }
    }

    public class ValidationError
    {
        public string Field { get; }

        public int Code { get; set; }

        public string Message { get; }

        public ValidationError(string field, int code, string message)
        {
            Field = field != string.Empty ? field : null;
            Code = code != 0 ? code : 55; //Custom
            Message = message;
        }
    }
}
