using Xunit;
using System.Linq;
using SmartParkingSystem.Services;
using SmartParkingSystem.Repositories;
using SmartParkingSystem.Models;

namespace SmartParkingSystem.Tests
{
    public class ParkingTests
    {
        [Fact]
        public void Search_Existing_ReturnsResults()
        {
            var repo = new FileRepository();
            var service = new ParkingService(repo);

            service.Add(new ParkingSpot { Name = "TestParking", PricePerHour = 2 });

            var result = service.SearchByName("TestParking");

            Assert.NotNull(result);
            Assert.True(result.Count > 0);
        }

        [Fact]
        public void Search_NotExisting_ReturnsEmpty()
        {
            var repo = new FileRepository();
            var service = new ParkingService(repo);

            var result = service.SearchByName("XYZ123");

            Assert.Empty(result);
        }

        [Fact]
        public void Add_InvalidPrice_DoesNotCrash()
        {
            var repo = new FileRepository();
            var service = new ParkingService(repo);

            var spot = new ParkingSpot
            {
                Name = "Test",
                PricePerHour = -5
            };

            service.Add(spot);

            Assert.True(true);
        }
    }
}