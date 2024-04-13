using System.ComponentModel.DataAnnotations;

namespace WebAPI.Messages
{
    public class ListingRequest
    {
        [Required(ErrorMessage = "Car Brand is required")]
        public string? CarBrand { get; set; }

        [Required(ErrorMessage = "Car Model is required")]
        public string? CarModel { get; set; }

        [Required(ErrorMessage = "Car Year is required")]
        public int? CarYear { get; set; }

        [Required(ErrorMessage = "Listing Value is required")]
        public int? Value { get; set; }

        public string? Description { get; set; }
    }
}
