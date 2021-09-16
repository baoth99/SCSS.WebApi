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
            StatusCode = HttpStatusCodes.BadRequest;
            MessageCode = SystemMessageCode.DataInvalid;
            MessageDetail = "Validation Failed";
            Data = modelState.Keys
                    .SelectMany(key => modelState[key].Errors.Select(x => new ValidationError(key, x.ErrorMessage)))
                    .ToList();
            Total = modelState.Count;
        }
    }

    public class ValidationError
    {
        public string Field { get; }

        public string Code { get; set; }

        public ValidationError(string field, string code)
        {
            Field = field != string.Empty ? field : null;
            Code = code;
        }
    }
}
