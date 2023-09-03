using System;
using System.Net;

namespace Cartio.Application.Errors
{
    public class InvalidPhoneNumberException : Exception, IServiceException
    {
        public HttpStatusCode StatusCode => HttpStatusCode.Conflict;

        public string ErrorMessage => "Phone number must be valid, all numeric and 10 character long";
    }
}
