using System;
using System.Net;

namespace Cartio.Application.Errors
{
    public class DuplicatePhoneNumberException : Exception, IServiceException
    {
        public HttpStatusCode StatusCode => HttpStatusCode.Conflict;

        public string ErrorMessage => "Phone number already exist";
    }
}
