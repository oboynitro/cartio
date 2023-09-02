using System;
using System.Collections.Generic;

namespace Cartio.DTOs.Responses
{
    public class CartItemResponse
    {
        public Guid Id { get; set; }
        public Guid ItemId { get; set; }
        public string ItemName { get; set; } = null!;
        public int Quantity { get; set; }
        public double UnitPrice { get; set; }
        public string PhoneNumber { get; set; } = null!;
    }

    public class CartResponse
    {
        public IEnumerable<CartItemResponse>? CartItems { get; set; }
    }
}
