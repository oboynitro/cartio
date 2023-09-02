using Cartio.Application.Errors;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Cartio.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorsController : ControllerBase
    {
        [Route("/error")]
        public IActionResult Error()
        {
            Exception exception = HttpContext.Features.Get<IExceptionHandlerFeature>()?.Error;

            int statusCode;
            string title;

            switch (exception)
            {
                case IServiceException serviceException:
                    statusCode = (int)serviceException.StatusCode;
                    title = serviceException.ErrorMessage;
                    break;
                default:
                    statusCode = StatusCodes.Status500InternalServerError;
                    title = "An unexpected error occured";
                    break;
            }

            return Problem(statusCode: statusCode, title: title);
        }
    }
}
