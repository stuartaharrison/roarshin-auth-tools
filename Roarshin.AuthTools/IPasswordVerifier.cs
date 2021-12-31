namespace Roarshin.AuthTools {
    
    /// <summary>
    /// Allows you to verify that a specific plain text value matches that of the hashed version.
    /// </summary>
    public interface IPasswordVerifier {
        
        /// <summary>
        /// Compares the hashed value with that of the plain text version.
        /// </summary>
        /// <param name="hashedPassword">The hashed password value to compare against.</param>
        /// <param name="plainTextPassword">The plain text password.</param>
        /// <returns>TRUE if the plain text hash matches that of hashed version.</returns>
        bool IsPasswordValid(string hashedPassword, string plainTextPassword);
    }
}
