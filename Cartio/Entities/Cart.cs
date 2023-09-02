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
            User user)
        {
            ItemId = itemId;
            ItemName = itemName;
            Quantity = quantity;
            UnitPrice = unitPrice;
            User = user;
        }

        public Guid ItemId { get; set; }
        public string ItemName { get; set; }
        public int Quantity { get; set; }
        public double UnitPrice { get; set; }
        public User User { get; set; }
    }
}
