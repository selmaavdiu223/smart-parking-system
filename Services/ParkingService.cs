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

            if (!string.IsNullOrEmpty(nameFilter))
            {
                data = data.Where(x => x.Name.Contains(nameFilter)).ToList();
            }

            return data;
        }

        public void Add(ParkingSpot spot)
        {
            if (string.IsNullOrWhiteSpace(spot.Name))
                throw new Exception("Name nuk mund te jete bosh");

            if (spot.PricePerHour <= 0)
                throw new Exception("Price duhet > 0");

            _repo.Add(spot);
        }

        public ParkingSpot GetById(int id)
        {
            return _repo.GetById(id);
        }

        public void Delete(int id)
        {
            _repo.Delete(id);
        }

        public void Update(ParkingSpot spot)
        {
            if (string.IsNullOrWhiteSpace(spot.Name))
                throw new Exception("Name nuk mund te jete bosh");

            if (spot.PricePerHour <= 0)
                throw new Exception("Price duhet > 0");

            _repo.Update(spot);
        }
    }
}
