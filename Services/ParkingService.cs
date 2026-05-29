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
            var data = _repo.GetAll();

            if (!string.IsNullOrWhiteSpace(nameFilter))
            {
                data = data
                    .Where(x => x.Name.ToLower().Contains(nameFilter.ToLower()))
                    .ToList();
            }

            return data;
        }

        public List<ParkingSpot> SearchByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new Exception("Emri per kerkim nuk mund te jete bosh");

            return _repo.GetAll()
                .Where(x => x.Name.ToLower().Contains(name.ToLower()))
                .ToList();
        }

        public void Add(ParkingSpot spot)
        {
            if (string.IsNullOrWhiteSpace(spot.Name))
                throw new Exception("Emri nuk mund te jete bosh");

            if (spot.PricePerHour <= 0)
                throw new Exception("Cmimi duhet te jete me i madh se 0");

            var existing = _repo.GetAll()
                .FirstOrDefault(x => x.Name.Equals(spot.Name.Trim(), StringComparison.OrdinalIgnoreCase));

            if (existing != null)
            {
                var status = existing.IsAvailable ? "i lire" : "i zene";
                throw new Exception($"Parkingu {existing.Name} ekziston dhe eshte {status}. Nuk mund te shtohet perseri.");
            }

            spot.Name = spot.Name.Trim();
            _repo.Add(spot);
        }

        public ParkingSpot GetById(int id)
        {
            if (id <= 0)
                throw new Exception("ID duhet te jete me i madh se 0");

            var spot = _repo.GetById(id);

            if (spot == null)
                throw new Exception("Parking nuk u gjet");

            return spot;
        }

        public void Delete(int id)
        {
            if (id <= 0)
                throw new Exception("ID duhet te jete me i madh se 0");

            var existing = _repo.GetById(id);

            if (existing == null)
                throw new Exception("Parking nuk ekziston");

            _repo.Delete(id);
        }

        public void Update(ParkingSpot spot)
        {
            if (spot.Id <= 0)
                throw new Exception("ID duhet te jete me i madh se 0");

            if (string.IsNullOrWhiteSpace(spot.Name))
                throw new Exception("Emri nuk mund te jete bosh");

            if (spot.PricePerHour <= 0)
                throw new Exception("Cmimi duhet te jete me i madh se 0");

            var existing = _repo.GetById(spot.Id);

            if (existing == null)
                throw new Exception("Parking nuk ekziston");

            var duplicate = _repo.GetAll()
                .FirstOrDefault(x =>
                    x.Id != spot.Id &&
                    x.Name.Equals(spot.Name.Trim(), StringComparison.OrdinalIgnoreCase));

            if (duplicate != null)
                throw new Exception($"Parkingu {duplicate.Name} ekziston tashme");

            spot.Name = spot.Name.Trim();
            _repo.Update(spot);
        }
    }
}
