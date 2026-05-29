namespace SmartParkingSystem.Models
{
    public class ParkingSession
    {
        public int Id { get; set; }

        public int ParkingSpotId { get; set; }

        public string VehiclePlate { get; set; } = string.Empty;

        public DateTime EntryTime { get; set; }

        public DateTime? ExitTime { get; set; }

        public double TotalPrice { get; set; }

        public bool IsActive { get; set; }
    }
}
