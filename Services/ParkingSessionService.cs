using System.Globalization;
using SmartParkingSystem.Models;
using SmartParkingSystem.Repositories;
using SmartParkingSystem.Utils;

namespace SmartParkingSystem.Services
{
    public class ParkingSessionService
    {
        private readonly FileRepository _parkingRepository;
        private readonly string _filePath;

        public ParkingSessionService(FileRepository parkingRepository, string filePath = "App_Data/sessions.csv")
        {
            _parkingRepository = parkingRepository;
            _filePath = filePath;
        }

        public List<ParkingSession> GetAll()
        {
            EnsureFile();

            return File.ReadAllLines(_filePath)
                .Where(line => !string.IsNullOrWhiteSpace(line))
                .Select(ParseSession)
                .Where(session => session != null)
                .Cast<ParkingSession>()
                .ToList();
        }

        public ParkingSession StartSession(int parkingSpotId, string vehiclePlate)
        {
            if (parkingSpotId <= 0)
                throw new Exception("Parking spot nuk eshte valid");

            if (string.IsNullOrWhiteSpace(vehiclePlate))
                throw new Exception("Targa nuk mund te jete bosh");

            var spot = _parkingRepository.GetById(parkingSpotId);

            if (spot == null)
                throw new Exception("Parking spot nuk ekziston");

            if (!spot.IsAvailable)
                throw new Exception("Ky parking spot eshte i zene");

            var sessions = GetAll();
            var session = new ParkingSession
            {
                Id = sessions.Any() ? sessions.Max(x => x.Id) + 1 : 1,
                ParkingSpotId = parkingSpotId,
                VehiclePlate = vehiclePlate.Trim().ToUpperInvariant(),
                EntryTime = DateTime.Now,
                IsActive = true
            };

            sessions.Add(session);
            Save(sessions);

            spot.IsAvailable = false;
            _parkingRepository.Update(spot);

            return session;
        }

        public ParkingSession EndSession(int sessionId)
        {
            var sessions = GetAll();
            var session = sessions.FirstOrDefault(x => x.Id == sessionId);

            if (session == null)
                throw new Exception("Sesioni nuk u gjet");

            if (!session.IsActive)
                throw new Exception("Sesioni eshte mbyllur me pare");

            var spot = _parkingRepository.GetById(session.ParkingSpotId);

            if (spot == null)
                throw new Exception("Parking spot nuk u gjet");

            session.ExitTime = DateTime.Now;
            session.IsActive = false;

            var hours = Math.Max(1, Math.Ceiling((session.ExitTime.Value - session.EntryTime).TotalHours));
            session.TotalPrice = hours * spot.PricePerHour;

            Save(sessions);

            spot.IsAvailable = true;
            _parkingRepository.Update(spot);

            return session;
        }

        private void Save(List<ParkingSession> sessions)
        {
            var lines = sessions.Select(ToCsv);
            EnsureDirectory();
            File.WriteAllLines(_filePath, lines);
        }

        private void EnsureFile()
        {
            if (!File.Exists(_filePath))
            {
                EnsureDirectory();
                File.Create(_filePath).Close();
            }
        }

        private void EnsureDirectory()
        {
            var directory = Path.GetDirectoryName(_filePath);

            if (!string.IsNullOrWhiteSpace(directory))
                Directory.CreateDirectory(directory);
        }

        private static ParkingSession? ParseSession(string line)
        {
            var parts = CsvUtils.ParseLine(line);

            if (parts.Count < 7)
                return null;

            return new ParkingSession
            {
                Id = int.Parse(parts[0]),
                ParkingSpotId = int.Parse(parts[1]),
                VehiclePlate = parts[2],
                EntryTime = DateTime.Parse(parts[3], CultureInfo.InvariantCulture),
                ExitTime = string.IsNullOrWhiteSpace(parts[4])
                    ? null
                    : DateTime.Parse(parts[4], CultureInfo.InvariantCulture),
                TotalPrice = double.Parse(parts[5], CultureInfo.InvariantCulture),
                IsActive = bool.Parse(parts[6])
            };
        }

        private static string ToCsv(ParkingSession session)
        {
            var exitTime = session.ExitTime?.ToString("O", CultureInfo.InvariantCulture) ?? string.Empty;

            return CsvUtils.ToLine(
                session.Id,
                session.ParkingSpotId,
                session.VehiclePlate,
                session.EntryTime.ToString("O", CultureInfo.InvariantCulture),
                exitTime,
                session.TotalPrice.ToString(CultureInfo.InvariantCulture),
                session.IsActive);
        }
    }
}
