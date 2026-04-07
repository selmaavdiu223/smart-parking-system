using SmartParkingSystem.Models;
using SmartParkingSystem.Repositories;

namespace SmartParkingSystem.Services
{
    public class ParkingService
    {
        private readonly FileRepository _repo;

        public ParkingService(FileRepository repo)
        {
            _repo = repo;
        }

        public List<ParkingSpot> List(string? nameFilter = null)
        {
            try
            {
                var data = _repo.GetAll();

                if (!string.IsNullOrEmpty(nameFilter))
                {
                    data = data.Where(x => x.Name.ToLower().Contains(nameFilter.ToLower())).ToList();
                }

                return data;
            }
            catch
            {
                Console.WriteLine("Gabim gjatë listimit.");
                return new List<ParkingSpot>();
            }
        }

        // ✅ FEATURE E RE (Search)
        public List<ParkingSpot> SearchByName(string name)
        {
            try
            {
                var data = _repo.GetAll();

                if (string.IsNullOrWhiteSpace(name))
                    return new List<ParkingSpot>();

                return data
                    .Where(x => x.Name.ToLower().Contains(name.ToLower()))
                    .ToList();
            }
            catch
            {
                Console.WriteLine("Gabim gjatë kërkimit.");
                return new List<ParkingSpot>();
            }
        }

        public void Add(ParkingSpot spot)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(spot.Name))
                    throw new Exception("Name nuk mund te jete bosh");

                if (spot.PricePerHour <= 0)
                    throw new Exception("Price duhet > 0");

                _repo.Add(spot);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public ParkingSpot? GetById(int id)
        {
            try
            {
                return _repo.GetById(id);
            }
            catch
            {
                Console.WriteLine("Gabim gjatë kërkimit me ID.");
                return null;
            }
        }

        public void Delete(int id)
        {
            try
            {
                _repo.Delete(id);
            }
            catch
            {
                Console.WriteLine("Gabim gjatë fshirjes.");
            }
        }

        public void Update(ParkingSpot spot)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(spot.Name))
                    throw new Exception("Name nuk mund te jete bosh");

                if (spot.PricePerHour <= 0)
                    throw new Exception("Price duhet > 0");

                _repo.Update(spot);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
