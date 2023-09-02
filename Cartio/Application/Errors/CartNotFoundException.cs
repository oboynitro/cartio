using System;
using System.Net;

namespace Cartio.Application.Errors
{
    public class CartNotFoundException : Exception, IServiceException
    {
        public HttpStatusCode StatusCode => HttpStatusCode.NotFound;

        public string ErrorMessage => "Cart item does not exist";
    }
}
