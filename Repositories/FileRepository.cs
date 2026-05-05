using SmartParkingSystem.Models;
using System.Globalization;

namespace SmartParkingSystem.Repositories
{
    public class FileRepository
    {
        private readonly string _filePath = "data.csv";

        public List<ParkingSpot> GetAll()
        {
            var list = new List<ParkingSpot>();

            try
            {
                if (!File.Exists(_filePath))
                {
                    File.Create(_filePath).Close();
                    return list;
                }

                var lines = File.ReadAllLines(_filePath);

                foreach (var line in lines)
                {
                    try
                    {
                        var parts = line.Split(',');

                        if (parts.Length < 4)
                            continue;

                        list.Add(new ParkingSpot
                        {
                            Id = int.Parse(parts[0]),
                            Name = parts[1],
                            PricePerHour = double.Parse(parts[2], CultureInfo.InvariantCulture),
                            IsAvailable = bool.Parse(parts[3])
                        });
                    }
                    catch
                    {
                        Console.WriteLine("Rresht i pavlefshëm në file u injorua.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Gabim gjatë leximit të file: " + ex.Message);
            }

            return list;
        }

        public ParkingSpot? GetById(int id)
        {
            return GetAll().FirstOrDefault(x => x.Id == id);
        }

        public void Add(ParkingSpot spot)
        {
            try
            {
                var list = GetAll();

                // ID auto-increment
                spot.Id = list.Any() ? list.Max(x => x.Id) + 1 : 1;

                var line = $"{spot.Id},{spot.Name},{spot.PricePerHour.ToString(CultureInfo.InvariantCulture)},{spot.IsAvailable}";
                File.AppendAllText(_filePath, line + Environment.NewLine);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Gabim gjatë shtimit: " + ex.Message);
            }
        }

        public void Save(List<ParkingSpot> list)
        {
            try
            {
                var lines = list.Select(x =>
                    $"{x.Id},{x.Name},{x.PricePerHour.ToString(CultureInfo.InvariantCulture)},{x.IsAvailable}"
                );

                File.WriteAllLines(_filePath, lines);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Gabim gjatë ruajtjes: " + ex.Message);
            }
        }

        public void Delete(int id)
        {
            try
            {
                var list = GetAll();
                var newList = list.Where(x => x.Id != id).ToList();

                if (list.Count == newList.Count)
                {
                    Console.WriteLine("Parking nuk u gjet për fshirje.");
                    return;
                }

                Save(newList);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Gabim gjatë fshirjes: " + ex.Message);
            }
        }

        public void Update(ParkingSpot updated)
        {
            try
            {
                var list = GetAll();

                var existing = list.FirstOrDefault(x => x.Id == updated.Id);

                if (existing == null)
                {
                    Console.WriteLine("Parking nuk u gjet për update.");
                    return;
                }

                existing.Name = updated.Name;
                existing.PricePerHour = updated.PricePerHour;
                existing.IsAvailable = updated.IsAvailable;

                Save(list);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Gabim gjatë update: " + ex.Message);
            }
        }
    }
}