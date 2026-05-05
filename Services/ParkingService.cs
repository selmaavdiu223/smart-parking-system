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

        // Search
        public List<ParkingSpot> SearchByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new Exception("Emri për kërkim nuk mund të jetë bosh");

            var data = _repo.GetAll();

            return data
                .Where(x => x.Name.ToLower().Contains(name.ToLower()))
                .ToList();
        }

        // ✅ VALIDIM I PËRMIRËSUAR
        public void Add(ParkingSpot spot)
        {
            if (spot.Id <= 0)
                throw new Exception("ID duhet të jetë më i madh se 0");

            if (string.IsNullOrWhiteSpace(spot.Name))
                throw new Exception("Emri nuk mund të jetë bosh");

            if (spot.PricePerHour <= 0)
                throw new Exception("Çmimi duhet të jetë më i madh se 0");

            // kontroll ID unike
            var existing = _repo.GetAll().FirstOrDefault(x => x.Id == spot.Id);
            if (existing != null)
                throw new Exception("Ekziston parking me këtë ID");

            _repo.Add(spot);
        }

        public ParkingSpot? GetById(int id)
        {
            if (id <= 0)
                throw new Exception("ID duhet të jetë më i madh se 0");

            var spot = _repo.GetById(id);

            if (spot == null)
                throw new Exception("Parking nuk u gjet");

            return spot;
        }

        public void Delete(int id)
        {
            if (id <= 0)
                throw new Exception("ID duhet të jetë më i madh se 0");

            var existing = _repo.GetById(id);
            if (existing == null)
                throw new Exception("Parking nuk ekziston");

            _repo.Delete(id);
        }

        public void Update(ParkingSpot spot)
        {
            if (spot.Id <= 0)
                throw new Exception("ID duhet të jetë më i madh se 0");

            if (string.IsNullOrWhiteSpace(spot.Name))
                throw new Exception("Emri nuk mund të jetë bosh");

            if (spot.PricePerHour <= 0)
                throw new Exception("Çmimi duhet të jetë më i madh se 0");

            var existing = _repo.GetById(spot.Id);
            if (existing == null)
                throw new Exception("Parking nuk ekziston");

            _repo.Update(spot);
        }
    }
}