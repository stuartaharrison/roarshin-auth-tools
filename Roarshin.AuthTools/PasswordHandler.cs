using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Roarshin.AuthTools {
    
    public sealed class PasswordHandler : IPasswordGenerator, IPasswordVerifier {

        private readonly int _keySize;
        private readonly int _saltSize;
        private readonly int _iterations;
        private readonly string _validCharacters;

        /// <inheritdoc cref="IPasswordGenerator.GeneratePasswordHash(string)" />
        public string GeneratePasswordHash(string plainText) {
            using var algorithm = new Rfc2898DeriveBytes(plainText, _saltSize, _iterations);

            var key = Convert.ToBase64String(algorithm.GetBytes(_keySize));
            var salt = Convert.ToBase64String(algorithm.Salt);

            return $"{_iterations}.{salt}.{key}";
        }

        /// <inheritdoc cref="IPasswordGenerator.GenerateRandomPasswordHash(int, out string)" />
        public string GenerateRandomPasswordHash(int length, out string plainText) {
            plainText = GeneratePlainTextPassword(length);
            return GeneratePasswordHash(plainText);
        }

        /// <inheritdoc cref="IPasswordVerifier.IsPasswordValid(string, string)" />
        public bool IsPasswordValid(string hashedPassword, string plainTextPassword) {
            var parts = hashedPassword.Split('.');
            if (parts.Length != 3) {
                throw new FormatException("Unexpected hash format. Should be formatted as `{iterations}.{salt}.{hash}`");
            }

            var salt = Convert.FromBase64String(parts[1]);
            var key = Convert.FromBase64String(parts[2]);
            var iterations = Convert.ToInt32(parts[0]);

            using var algorithm = new Rfc2898DeriveBytes(plainTextPassword, salt, iterations);
            var keyToCheck = algorithm.GetBytes(_keySize);

            return keyToCheck.SequenceEqual(key);
        }

        /// <summary>
        /// Creates a random string with the length specified.
        /// </summary>
        /// <param name="length">The length that the string has to be.</param>
        /// <returns>Returns a randomly constructed string to the specified length.</returns>
        private string GeneratePlainTextPassword(int length) {
            var res = new StringBuilder();
            using (RNGCryptoServiceProvider rng = new()) {
                byte[] uintBuffer = new byte[sizeof(uint)];

                while (length-- > 0) {
                    rng.GetBytes(uintBuffer);
                    uint num = BitConverter.ToUInt32(uintBuffer, 0);
                    res.Append(_validCharacters[(int)(num % (uint)_validCharacters.Length)]);
                }
            }

            return res.ToString();
        }

        public PasswordHandler(int keySize, int saltSize, int iterations, string validCharacters) {
            _keySize = keySize;
            _saltSize = saltSize;
            _iterations = iterations;
            _validCharacters = validCharacters;
        }
    }
}
