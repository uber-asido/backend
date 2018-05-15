using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OData;
using System.Linq;

namespace Uber.Core.OData
{
    public class UberODataController : ODataController
    {
        protected IActionResult ODataBadRequest()
        {
            var error = new ODataError
            {
                ErrorCode = "InvalidModel",
                Message = "The request is invalid",
                Details = ModelState.SelectMany(state =>
                    state.Value.Errors.Select(modelError =>
                    new ODataErrorDetail
                    {
                        ErrorCode = "Invalid",
                        Target = state.Key,
                        Message = string.IsNullOrWhiteSpace(modelError.ErrorMessage)
                            ? modelError.Exception?.Message
                            : modelError.ErrorMessage
                    }
                    )).ToArray()
            };

            return StatusCode(StatusCodes.Status400BadRequest, error);
        }
    }
}
