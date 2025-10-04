using System.ComponentModel.DataAnnotations;

namespace Talabat.Core.Dtos
{
    public class BasketItemDto
    {
        [Required]
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string PictureUrl { get; set; }

        [Required]
        [Range(0.1,double.MaxValue,ErrorMessage ="Price must be greater")]
        public int Price { get; set; }
        [Required]
        [Range(1,int.MaxValue,ErrorMessage ="Quantity must be one item at least")] 
        public int Quantity { get; set; }
        public string brand { get; set; }
        public string type { get; set; }
    }
}