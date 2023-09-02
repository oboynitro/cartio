using System;
using System.Net;

namespace Cartio.Application.Errors
{
    public class AuthenticationException : Exception, IServiceException
    {
        public HttpStatusCode StatusCode => HttpStatusCode.NotFound;

        public string ErrorMessage => "Invalid phone number or password";
    }
}
