using System;
using System.ComponentModel.DataAnnotations;

namespace Cartio.DTOs.Requests
{
    public class RemoveCartItemRequest
    {
        [Required(AllowEmptyStrings = false)]
        public Guid ItemId { get; set; }

        public bool ClearItem { get; set; } = false;
    }
}
