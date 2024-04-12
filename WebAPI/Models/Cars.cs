namespace WebAPI.Models
{
    public class Cars
    {
        public int Id { get; set; }

        public string Brand { get; set; } = string.Empty; 

        public string Model { get; set; } = string.Empty;

        public int Year { get; set; }
    }
}
