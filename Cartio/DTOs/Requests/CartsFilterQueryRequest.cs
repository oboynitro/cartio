using Cartio.DTOs.Responses;
using System;

namespace Cartio.DTOs.Requests
{
    public class CartsFilterQueryRequest
    {
        public string PhoneNumber { get; set; } = string.Empty;
        public string Time { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public string ItemName { get; set; } = string.Empty;
        public int Page { get; set; } = 1;
        public int ItemsPerPage { get; set; } = 10;
    }
}
