using SmartParkingSystem.Models;
using System.Globalization;

namespace SmartParkingSystem.Repositories
{
    public class FileRepository
    {
        private readonly string _filePath = "data.csv";

        public List<ParkingSpot> GetAll()
        {
            if (!File.Exists(_filePath))
                return new List<ParkingSpot>();

            var lines = File.ReadAllLines(_filePath);

            return lines.Select(line =>
            {
                var parts = line.Split(',');

                return new ParkingSpot
                {
                    Id = int.Parse(parts[0]),
                    Name = parts[1],
                    PricePerHour = double.Parse(parts[2], CultureInfo.InvariantCulture),
                    IsAvailable = bool.Parse(parts[3])
                };
            }).ToList();
        }

        public ParkingSpot GetById(int id)
        {
            return GetAll().FirstOrDefault(x => x.Id == id);
        }

        public void Add(ParkingSpot spot)
        {
            var list = GetAll();

            spot.Id = list.Any() ? list.Max(x => x.Id) + 1 : 1;

            var line = $"{spot.Id},{spot.Name},{spot.PricePerHour},{spot.IsAvailable}";
            File.AppendAllText(_filePath, line + Environment.NewLine);
        }

        public void Save(List<ParkingSpot> list)
        {
            var lines = list.Select(x => $"{x.Id},{x.Name},{x.PricePerHour},{x.IsAvailable}");
            File.WriteAllLines(_filePath, lines);
        }

        public void Delete(int id)
        {
            var list = GetAll();
            list = list.Where(x => x.Id != id).ToList();
            Save(list);
        }

        public void Update(ParkingSpot updated)
        {
            var list = GetAll();

            var existing = list.FirstOrDefault(x => x.Id == updated.Id);

            if (existing != null)
            {
                existing.Name = updated.Name;
                existing.PricePerHour = updated.PricePerHour;
                existing.IsAvailable = updated.IsAvailable;

                Save(list);
            }
        }
    }
}