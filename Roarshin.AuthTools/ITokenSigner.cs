namespace Roarshin.AuthTools {
    
    /// <summary>
    /// Allows you to convert your entity/model into a token that contains claims & roles that represent that model
    /// </summary>
    /// <typeparam name="T">The user entity/model</typeparam>
    public interface ITokenSigner<T> {

        /// <summary>
        /// Takes your concrete entity/model object and converts it into token details. 
        /// </summary>
        /// <param name="signature">The user model object</param>
        /// <returns></returns>
        public ITokenDetails SignToken(T signature);
    }
}
