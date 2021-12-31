namespace Roarshin.AuthTools {

    public sealed class TokenDetails : ITokenDetails {

        /// <inheritdoc cref="ITokenDetails.Token" />
        public string Token { get; set; }

        /// <inheritdoc cref="ITokenDetails.AccessToken" />
        public string AccessToken { get; set; }

        /// <inheritdoc cref="ITokenDetails.RefreshToken" />
        public string RefreshToken { get; set; }
    }
}
