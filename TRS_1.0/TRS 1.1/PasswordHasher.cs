using System;
using System.Globalization;
using System.Security.Cryptography;

namespace TRS_1._1
{
    public static class PasswordHasher
    {
        private const int Iterations = 600000;
        private const int SaltSize = 16;
        private const int HashSize = 32;
        private const int MinimumAcceptedIterations = 100000;
        private const int MaximumAcceptedIterations = 1000000;
        private const string Algorithm = "PBKDF2-SHA256";

        public static string HashPassword(string password)
        {
            if (password == null)
                throw new ArgumentNullException(nameof(password));
            if (password.Length < 12)
                throw new ArgumentException("Parola en az 12 karakter olmalıdır.", nameof(password));

            byte[] salt = new byte[SaltSize];
            using (RandomNumberGenerator random = RandomNumberGenerator.Create())
            {
                random.GetBytes(salt);
            }

            byte[] hash;
            using (var derivation = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA256))
            {
                hash = derivation.GetBytes(HashSize);
            }

            return string.Join("$", Algorithm,
                Iterations.ToString(CultureInfo.InvariantCulture),
                Convert.ToBase64String(salt),
                Convert.ToBase64String(hash));
        }

        public static bool VerifyPassword(string password, string storedHash)
        {
            if (string.IsNullOrEmpty(password) || string.IsNullOrWhiteSpace(storedHash))
                return false;

            string[] parts = storedHash.Split('$');
            if (parts.Length != 4 || !string.Equals(parts[0], Algorithm, StringComparison.Ordinal))
                return false;

            int iterations;
            if (!int.TryParse(parts[1], NumberStyles.None, CultureInfo.InvariantCulture, out iterations)
                || iterations < MinimumAcceptedIterations
                || iterations > MaximumAcceptedIterations)
                return false;

            byte[] salt;
            byte[] expectedHash;
            try
            {
                salt = Convert.FromBase64String(parts[2]);
                expectedHash = Convert.FromBase64String(parts[3]);
            }
            catch (FormatException)
            {
                return false;
            }

            if (salt.Length != SaltSize || expectedHash.Length != HashSize)
                return false;

            byte[] actualHash;
            using (var derivation = new Rfc2898DeriveBytes(password, salt, iterations, HashAlgorithmName.SHA256))
            {
                actualHash = derivation.GetBytes(HashSize);
            }

            return FixedTimeEquals(actualHash, expectedHash);
        }

        private static bool FixedTimeEquals(byte[] left, byte[] right)
        {
            if (left == null || right == null || left.Length != right.Length)
                return false;

            int difference = 0;
            for (int i = 0; i < left.Length; i++)
                difference |= left[i] ^ right[i];

            return difference == 0;
        }
    }
}
