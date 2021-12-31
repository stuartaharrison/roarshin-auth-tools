namespace Roarshin.AuthTools.DependencyInjection {
    
    public sealed class RoarshinAuthToolOptions {

        public int KeySize { get; private set; } = 32;

        public int SaltSize { get; private set; } = 16;

        public int Iterations { get; private set; } = 3;

        public int TokenExpiresInMinutes { get; private set; } = 30;

        public string TokenKey { get; private set; } = "ROARSHIN"; 

        public string ValidCharacters { get; private set; } = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";

        /// <summary>
        /// Configure the key that will be used when generating/signing a JWT Token.
        /// </summary>
        /// <param name="tokenKey">Signing Key.</param>
        /// <param name="expiryInMinutes">How long (in minutes) until the Token will become expired.</param>
        public void ConfigureAuthToken(string tokenKey, int expiryInMinutes = 30) {
            TokenKey = tokenKey;
            TokenExpiresInMinutes = expiryInMinutes;
        }

        /// <summary>
        /// Configure the cryptography options.
        /// </summary>
        /// <param name="keySize">Size of the desired key. The bigger the Key Size, the bigger the derived key.</param>
        /// <param name="saltSize">The size of the random salt that you want the class to generate.</param>
        /// <param name="iterations">The number of iterations for the operation.</param>
        public void ConfigureCryptography(int keySize, int saltSize, int iterations) {
            KeySize = keySize;
            SaltSize = saltSize;
            Iterations = iterations;
        }

        /// <summary>
        /// Configure the list of valid characters that can be used when generating a random password.
        /// </summary>
        /// <param name="validCharacters">The valid character list as a single string. (Example: abcdefghiABC)</param>
        public void UseValidCharacterDictionary(string validCharacters) {
            ValidCharacters = validCharacters;
        }

        /// <summary>
        /// Configure the list of valid characters that can be used when generating a random password.
        /// </summary>
        /// <param name="validCharacters">Array of valid characters that will be converted into a single string list.</param>
        public void UseValidCharacterDictionary(char[] validCharacters) {
            UseValidCharacterDictionary(new string(validCharacters));
        }
    }
}
