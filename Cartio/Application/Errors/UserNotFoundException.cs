using Cartio.Application.Errors;
using System;
using System.Net;

namespace Cartio.Api.Application.Errors
{
    public class UserNotFoundException : Exception, IServiceException
    {
        public HttpStatusCode StatusCode => HttpStatusCode.NotFound;

        public string ErrorMessage => "Request user does not exist";
    }
}
