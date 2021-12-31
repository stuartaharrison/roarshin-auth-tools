namespace Roarshin.AuthTools {
    
    /// <summary>
    /// Allows your to cross-check that a token being supplied is valid and was written by your application
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ITokenVerifier<T> {

        /// <summary>
        /// Checks that the string token is a valid signature and returns your user entity model object
        /// </summary>
        /// <param name="token">token value</param>
        /// <returns>User entity model object</returns>
        T VerifyToken(string token);
        
        /// <summary>
        /// CHecks that the string token is a valid signature and returns TRUE or FALSE
        /// </summary>
        /// <param name="token">token value</param>
        /// <param name="signature">User entity model object</param>
        /// <returns>TRUE if the token is a valid signature</returns>
        bool TryVerifyToken(string token, out T signature);
    }
}
