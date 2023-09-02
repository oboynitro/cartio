using System;
using System.ComponentModel.DataAnnotations;

namespace Cartio.DTOs.Requests
{
    public class AddCartItemRequest
    {
        [Required(AllowEmptyStrings = false)]
        public Guid ItemId { get; set; }

        public string ItemName { get; set; } = null!;
        public int Quantity { get; set; }
        public double UnitPrice { get; set; }
    }
}
