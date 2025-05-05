using System.ComponentModel.DataAnnotations;

namespace Shared.DataTransferObject.BasketModuleDtoss
{
    public class BasketItemDto
    {
        public int Id { get; set; }
        public string ProductName { get; set; } = default!;
    
        public string PictureUrl { get; set; } = default!;
        [Range(1 ,double.MaxValue)]
        public decimal Price { get; set; }
        [Range(1, 100)]

        public int Quantity { get; set; }
    }
}