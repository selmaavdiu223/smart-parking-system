using SmartParkingSystem.Models;
using SmartParkingSystem.Utils;
using System.Security.Cryptography;

namespace SmartParkingSystem.Services
{
    public class AuthService
    {
        private const int HashIterations = 100_000;
        private const int SaltSize = 16;
        private const int HashSize = 32;

        private readonly string _filePath;
        private readonly string _adminEmail;
        private readonly Dictionary<string, int> _activeTokens = new();

        public AuthService(IConfiguration configuration)
            : this("App_Data/users.csv", configuration["AdminEmail"])
        {
        }

        public AuthService(string filePath = "App_Data/users.csv", string? adminEmail = null)
        {
            _filePath = filePath;
            _adminEmail = (adminEmail ?? "admin@example.com").Trim();
        }

        public List<User> GetAll()
        {
            EnsureFile();

            return File.ReadAllLines(_filePath)
                .Where(line => !string.IsNullOrWhiteSpace(line))
                .Select(ParseUser)
                .Where(user => user != null)
                .Cast<User>()
                .Select(NormalizeRole)
                .ToList();
        }

        public User Register(User user)
        {
            if (string.IsNullOrWhiteSpace(user.FullName))
                throw new Exception("Emri nuk mund te jete bosh");

            if (string.IsNullOrWhiteSpace(user.Email))
                throw new Exception("Email nuk mund te jete bosh");

            if (string.IsNullOrWhiteSpace(user.Password) || user.Password.Length < 4)
                throw new Exception("Password duhet te kete se paku 4 karaktere");

            var users = GetAll();

            if (users.Any(x => x.Email.Equals(user.Email, StringComparison.OrdinalIgnoreCase)))
                throw new Exception("Ky email ekziston");

            user.Id = users.Any() ? users.Max(x => x.Id) + 1 : 1;
            user.Email = user.Email.Trim();
            user.FullName = user.FullName.Trim();
            user.Password = HashPassword(user.Password);
            user.Role = IsAdminEmail(user.Email) ? "Admin" : "Client";

            EnsureDirectory();
            File.AppendAllText(_filePath, ToCsv(user) + Environment.NewLine);

            return user;
        }

        public User Login(string email, string password)
        {
            var users = GetAll();
            var user = users.FirstOrDefault(x => x.Email.Equals(email.Trim(), StringComparison.OrdinalIgnoreCase));

            if (user == null || !VerifyPassword(password, user.Password))
                throw new Exception("Email ose password gabim");

            if (!IsHashedPassword(user.Password))
            {
                user.Password = HashPassword(password);
                Save(users);
            }

            return user;
        }

        public string CreateToken(User user)
        {
            var token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
            _activeTokens[token] = user.Id;
            return token;
        }

        public bool IsTokenValid(string? authorizationHeader)
        {
            return TryGetToken(authorizationHeader, out var token) && _activeTokens.ContainsKey(token);
        }

        public User? GetUserByToken(string? authorizationHeader)
        {
            if (!TryGetToken(authorizationHeader, out var token) || !_activeTokens.TryGetValue(token, out var userId))
                return null;

            return GetAll().FirstOrDefault(user => user.Id == userId);
        }

        public bool IsAdmin(string? authorizationHeader)
        {
            return GetUserByToken(authorizationHeader)?.Role.Equals("Admin", StringComparison.OrdinalIgnoreCase) == true;
        }

        public void RevokeToken(string? authorizationHeader)
        {
            if (string.IsNullOrWhiteSpace(authorizationHeader))
                return;

            if (TryGetToken(authorizationHeader, out var token))
                _activeTokens.Remove(token);
        }

        private static bool TryGetToken(string? authorizationHeader, out string token)
        {
            token = string.Empty;

            if (string.IsNullOrWhiteSpace(authorizationHeader))
                return false;

            const string prefix = "Bearer ";
            token = authorizationHeader.StartsWith(prefix, StringComparison.OrdinalIgnoreCase)
                ? authorizationHeader[prefix.Length..].Trim()
                : authorizationHeader.Trim();

            return !string.IsNullOrWhiteSpace(token);
        }

        private bool IsAdminEmail(string email)
        {
            return !string.IsNullOrWhiteSpace(_adminEmail) &&
                email.Equals(_adminEmail, StringComparison.OrdinalIgnoreCase);
        }

        private User NormalizeRole(User user)
        {
            user.Role = IsAdminEmail(user.Email) ? "Admin" : "Client";
            return user;
        }

        private void EnsureFile()
        {
            if (!File.Exists(_filePath))
            {
                EnsureDirectory();
                File.Create(_filePath).Close();
            }
        }

        private void Save(List<User> users)
        {
            EnsureDirectory();
            File.WriteAllLines(_filePath, users.Select(ToCsv));
        }

        private void EnsureDirectory()
        {
            var directory = Path.GetDirectoryName(_filePath);

            if (!string.IsNullOrWhiteSpace(directory))
                Directory.CreateDirectory(directory);
        }

        private static User? ParseUser(string line)
        {
            var parts = CsvUtils.ParseLine(line);

            if (parts.Count < 5)
                return null;

            return new User
            {
                Id = int.Parse(parts[0]),
                FullName = parts[1],
                Email = parts[2],
                Password = parts[3],
                Role = parts[4]
            };
        }

        private static string ToCsv(User user)
        {
            return CsvUtils.ToLine(user.Id, user.FullName, user.Email, user.Password, user.Role);
        }

        private static string HashPassword(string password)
        {
            var salt = RandomNumberGenerator.GetBytes(SaltSize);
            var hash = Rfc2898DeriveBytes.Pbkdf2(
                password,
                salt,
                HashIterations,
                HashAlgorithmName.SHA256,
                HashSize);

            return string.Join("$",
                "PBKDF2",
                HashIterations,
                Convert.ToBase64String(salt),
                Convert.ToBase64String(hash));
        }

        private static bool VerifyPassword(string password, string storedPassword)
        {
            if (!IsHashedPassword(storedPassword))
                return password == storedPassword;

            var parts = storedPassword.Split('$');

            if (parts.Length != 4 || !int.TryParse(parts[1], out var iterations))
                return false;

            var salt = Convert.FromBase64String(parts[2]);
            var expectedHash = Convert.FromBase64String(parts[3]);
            var actualHash = Rfc2898DeriveBytes.Pbkdf2(
                password,
                salt,
                iterations,
                HashAlgorithmName.SHA256,
                expectedHash.Length);

            return CryptographicOperations.FixedTimeEquals(actualHash, expectedHash);
        }

        private static bool IsHashedPassword(string password)
        {
            return password.StartsWith("PBKDF2$", StringComparison.Ordinal);
        }
    }
}
