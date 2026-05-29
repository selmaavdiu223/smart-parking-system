using System.Globalization;
using SmartParkingSystem.Models;
using SmartParkingSystem.Utils;

namespace SmartParkingSystem.Repositories
{
    public class FileRepository
    {
        private readonly string _filePath;
        private readonly bool _seedDefaultParkings;

        public FileRepository(string filePath = "data.csv")
        {
            _filePath = filePath;
            _seedDefaultParkings = false;
        }

        public List<ParkingSpot> GetAll()
        {
            var list = new List<ParkingSpot>();

            try
            {
                EnsureFile();

                var lines = File.ReadAllLines(_filePath);

                foreach (var line in lines)
                {
                    try
                    {
                        var parts = CsvUtils.ParseLine(line);

                        if (parts.Count < 4)
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
                        Console.WriteLine("Rresht i pavlefshem ne file u injorua.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Gabim gjate leximit te file: " + ex.Message);
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

                spot.Id = list.Any() ? list.Max(x => x.Id) + 1 : 1;

                var line = CsvUtils.ToLine(
                    spot.Id,
                    spot.Name,
                    spot.PricePerHour.ToString(CultureInfo.InvariantCulture),
                    spot.IsAvailable);

                EnsureDirectory();
                File.AppendAllText(_filePath, line + Environment.NewLine);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Gabim gjate shtimit: " + ex.Message);
            }
        }

        public void Save(List<ParkingSpot> list)
        {
            try
            {
                var lines = list.Select(x =>
                    CsvUtils.ToLine(
                        x.Id,
                        x.Name,
                        x.PricePerHour.ToString(CultureInfo.InvariantCulture),
                        x.IsAvailable)
                );

                EnsureDirectory();
                File.WriteAllLines(_filePath, lines);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Gabim gjate ruajtjes: " + ex.Message);
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
                    Console.WriteLine("Parking nuk u gjet per fshirje.");
                    return;
                }

                Save(newList);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Gabim gjate fshirjes: " + ex.Message);
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
                    Console.WriteLine("Parking nuk u gjet per update.");
                    return;
                }

                existing.Name = updated.Name;
                existing.PricePerHour = updated.PricePerHour;
                existing.IsAvailable = updated.IsAvailable;

                Save(list);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Gabim gjate update: " + ex.Message);
            }
        }

        private void EnsureDirectory()
        {
            var directory = Path.GetDirectoryName(_filePath);

            if (!string.IsNullOrWhiteSpace(directory))
                Directory.CreateDirectory(directory);
        }

        private void EnsureFile()
        {
            EnsureDirectory();

            if (File.Exists(_filePath) && new FileInfo(_filePath).Length > 0)
                return;

            if (_seedDefaultParkings && File.Exists("data.csv"))
            {
                File.Copy("data.csv", _filePath, overwrite: true);
                return;
            }

            if (!File.Exists(_filePath))
                File.Create(_filePath).Close();
        }
    }
}
