namespace Roarshin.AuthTools {
    
    /// <summary>
    /// Allows you to hash a password or generate & hash a new random password from a list of valid characters.
    /// </summary>
    public interface IPasswordGenerator {
        
        /// <summary>
        /// Takes the Plain Text password and hashes it using the options specified.
        /// </summary>
        /// <param name="plainText">The password in plain text format.</param>
        /// <returns>Returns a hash of the plain text value in a sequence like '{iterations}.{salt}.{key}'</returns>
        string GeneratePasswordHash(string plainText);

        /// <summary>
        /// Generates a random plain text string and then hashes it using the options specified.
        /// </summary>
        /// <param name="plainText">The randomly generated string in plain text format.</param>
        /// <returns>Returns a hash of the randomly generated string in a sequence like '{iterations}.{salt}.{key}'</returns>
        string GenerateRandomPasswordHash(int length, out string plainText);
    }
}
