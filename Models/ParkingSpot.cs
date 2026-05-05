namespace SmartParkingSystem.Models
{
    public class ParkingSpot
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public double PricePerHour { get; set; }

        public bool IsAvailable { get; set; }
    }
}