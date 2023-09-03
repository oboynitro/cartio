using Cartio.Entities.Common;
using System;

namespace Cartio.Entities
{
    public sealed class Cart : BaseEntity
    {
        public Cart()
        {}

        public Cart(
            Guid itemId,
            string itemName,
            int quantity,
            double unitPrice,
            string phoneNumber)
        {
            ItemId = itemId;
            ItemName = itemName;
            Quantity = quantity;
            UnitPrice = unitPrice;
            PhoneNumber = phoneNumber;
        }

        public Guid ItemId { get; set; }
        public string ItemName { get; set; } = null!;
        public int Quantity { get; set; }
        public double UnitPrice { get; set; }
        public string PhoneNumber { get; set; } = null!;
    }
}
