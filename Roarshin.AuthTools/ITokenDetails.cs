namespace Roarshin.AuthTools {
    
    public interface ITokenDetails {

        /// <summary>
        /// The value that should be returned to your client application
        /// </summary>
        string Token { get; }
        
        /// <summary>
        /// The value that should be stored in the Http-only cookie as part of the access token. This token should only be valid for a short period of time.
        /// </summary>
        string AccessToken { get; }

        /// <summary>
        /// The value that should be stored in a Http-only cookie and can be used one-time to get a new access token
        /// </summary>
        string RefreshToken { get; }
    }
}
