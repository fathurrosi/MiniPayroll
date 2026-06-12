using App.Application.Interfaces.Services;
 
namespace App.Infrastructure.Services
{

    public sealed class HasherService : IHasherService
    {
        // WorkFactor 12 is solid. 
        // In 2026 hardware, 13 is often preferred, but 12 is safe.
        private const int WorkFactor = 12;

        public string Hash(string password)
        {
            // Use the fully qualified class name: BCrypt.Net.BCrypt
            return BCrypt.Net.BCrypt.HashPassword(password, WorkFactor);
        }

        public bool Verify(string password, string passwordHash)
        {
            if (string.IsNullOrEmpty(passwordHash) || string.IsNullOrEmpty(password))
                return false;

            try
            {
                // Explicit call here as well
                return BCrypt.Net.BCrypt.Verify(password, passwordHash);
            }
            catch
            {
                // In case the hash in DB is malformed (e.g. old MD5 data)
                return false;
            }
        }
    }
}
