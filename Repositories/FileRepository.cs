using SmartParkingSystem.Models;
using System.Globalization;

namespace SmartParkingSystem.Repositories
{
    public class FileRepository
    {
        private readonly string _filePath = "data.csv";

        public List<ParkingSpot> GetAll()
        {
            try
            {
                if (!File.Exists(_filePath))
                {
                    Console.WriteLine("File nuk u gjet, po krijoj file të ri...");
                    File.Create(_filePath).Close();
                    return new List<ParkingSpot>();
                }

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
            catch
            {
                Console.WriteLine("Gabim gjatë leximit të file.");
                return new List<ParkingSpot>();
            }
        }

        public ParkingSpot? GetById(int id)
        {
            try
            {
                return GetAll().FirstOrDefault(x => x.Id == id);
            }
            catch
            {
                Console.WriteLine("Gabim gjatë kërkimit me ID.");
                return null;
            }
        }

        public void Add(ParkingSpot spot)
        {
            try
            {
                var list = GetAll();

                spot.Id = list.Any() ? list.Max(x => x.Id) + 1 : 1;

                var line = $"{spot.Id},{spot.Name},{spot.PricePerHour},{spot.IsAvailable}";
                File.AppendAllText(_filePath, line + Environment.NewLine);
            }
            catch
            {
                Console.WriteLine("Gabim gjatë shtimit.");
            }
        }

        public void Save(List<ParkingSpot> list)
        {
            try
            {
                var lines = list.Select(x => $"{x.Id},{x.Name},{x.PricePerHour},{x.IsAvailable}");
                File.WriteAllLines(_filePath, lines);
            }
            catch
            {
                Console.WriteLine("Gabim gjatë ruajtjes.");
            }
        }

        public void Delete(int id)
        {
            try
            {
                var list = GetAll();
                list = list.Where(x => x.Id != id).ToList();
                Save(list);
            }
            catch
            {
                Console.WriteLine("Gabim gjatë fshirjes.");
            }
        }

        public void Update(ParkingSpot updated)
        {
            try
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
                else
                {
                    Console.WriteLine("Item nuk u gjet për update.");
                }
            }
            catch
            {
                Console.WriteLine("Gabim gjatë update.");
            }
        }
    }
}