using System.IO;
using SmartParkingSystem.Models;
using SmartParkingSystem.Repositories;
using SmartParkingSystem.Services;
using Xunit;

namespace SmartParkingSystem.Tests
{
    public class ParkingTests
    {
        [Fact]
        public void Search_Existing_ReturnsResults()
        {
            var repo = CreateRepository();
            var service = new ParkingService(repo);

            service.Add(new ParkingSpot { Name = "TestParking", PricePerHour = 2 });

            var result = service.SearchByName("TestParking");

            Assert.NotNull(result);
            Assert.True(result.Count > 0);
        }

        [Fact]
        public void Search_NotExisting_ReturnsEmpty()
        {
            var repo = CreateRepository();
            var service = new ParkingService(repo);

            var result = service.SearchByName("XYZ123");

            Assert.Empty(result);
        }

        [Fact]
        public void Add_InvalidPrice_ThrowsValidationError()
        {
            var repo = CreateRepository();
            var service = new ParkingService(repo);

            var spot = new ParkingSpot
            {
                Name = "Test",
                PricePerHour = -5
            };

            var exception = Assert.Throws<System.Exception>(() => service.Add(spot));

            Assert.Contains("Cmimi", exception.Message);
        }

        [Fact]
        public void Add_NameWithComma_CanBeReadBack()
        {
            var repo = CreateRepository();
            var service = new ParkingService(repo);

            service.Add(new ParkingSpot { Name = "Parking, Qendra", PricePerHour = 3 });

            var result = service.SearchByName("Qendra");

            Assert.Single(result);
            Assert.Equal("Parking, Qendra", result[0].Name);
        }

        [Fact]
        public void Add_DuplicateOccupiedName_ThrowsValidationError()
        {
            var repo = CreateRepository();
            var service = new ParkingService(repo);

            service.Add(new ParkingSpot { Name = "Parking C", PricePerHour = 1.5, IsAvailable = false });

            var exception = Assert.Throws<System.Exception>(() =>
                service.Add(new ParkingSpot { Name = "parking c", PricePerHour = 2, IsAvailable = true }));

            Assert.Contains("eshte i zene", exception.Message);
        }

        private static FileRepository CreateRepository()
        {
            var filePath = Path.Combine(Path.GetTempPath(), $"parking-tests-{Path.GetRandomFileName()}.csv");
            return new FileRepository(filePath);
        }
    }

    public class AuthTests
    {
        [Fact]
        public void Register_StoresHashedPassword()
        {
            var filePath = Path.Combine(Path.GetTempPath(), $"users-tests-{Path.GetRandomFileName()}.csv");
            var service = new AuthService(filePath);

            service.Register(new User
            {
                FullName = "Test User",
                Email = "test@example.com",
                Password = "secret123"
            });

            var storedLine = File.ReadAllText(filePath);

            Assert.Contains("PBKDF2$", storedLine);
            Assert.DoesNotContain(",secret123,", storedLine);
        }

        [Fact]
        public void Login_CreatesValidToken()
        {
            var filePath = Path.Combine(Path.GetTempPath(), $"users-tests-{Path.GetRandomFileName()}.csv");
            var service = new AuthService(filePath);
            var user = service.Register(new User
            {
                FullName = "Test User",
                Email = "test@example.com",
                Password = "secret123"
            });

            var loggedInUser = service.Login("test@example.com", "secret123");
            var token = service.CreateToken(loggedInUser);

            Assert.Equal(user.Id, loggedInUser.Id);
            Assert.True(service.IsTokenValid($"Bearer {token}"));
        }

        [Fact]
        public void Register_AssignsAdminOnlyForConfiguredEmail()
        {
            var filePath = Path.Combine(Path.GetTempPath(), $"users-tests-{Path.GetRandomFileName()}.csv");
            var service = new AuthService(filePath, "owner@example.com");

            var admin = service.Register(new User
            {
                FullName = "Owner",
                Email = "owner@example.com",
                Password = "secret123"
            });

            var client = service.Register(new User
            {
                FullName = "Client User",
                Email = "client@example.com",
                Password = "secret123"
            });

            Assert.Equal("Admin", admin.Role);
            Assert.Equal("Client", client.Role);
        }

        [Fact]
        public void GetAll_NormalizesStoredRolesFromConfiguredAdminEmail()
        {
            var filePath = Path.Combine(Path.GetTempPath(), $"users-tests-{Path.GetRandomFileName()}.csv");
            File.WriteAllLines(filePath, new[]
            {
                "1,Owner,owner@example.com,password,Client",
                "2,Other,other@example.com,password,Admin"
            });

            var service = new AuthService(filePath, "owner@example.com");
            var users = service.GetAll();

            Assert.Equal("Admin", users.Single(user => user.Email == "owner@example.com").Role);
            Assert.Equal("Client", users.Single(user => user.Email == "other@example.com").Role);
        }
    }
}
