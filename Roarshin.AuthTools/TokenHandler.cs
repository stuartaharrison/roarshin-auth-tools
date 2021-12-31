using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Roarshin.AuthTools {

    public sealed class TokenHandler<T> : ITokenSigner<T>, ITokenVerifier<T> {

        private readonly string _key;
        private readonly int _expiryMinutes;

        /// <inheritdoc cref="ITokenSigner{T}.SignToken(T)" />
        public ITokenDetails SignToken(T signature) {
            // build the base objects
            var handler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_key));
            var tokenDescription = new SecurityTokenDescriptor {
                Subject = new ClaimsIdentity(),
                Expires = DateTime.UtcNow.AddMinutes(_expiryMinutes),
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature)
            };

            // extract the claim details from the signature object
            var properties = signature.GetType().GetProperties();
            foreach (var property in properties) {
                var propertyValue = property.GetValue(signature);
                if (propertyValue == null) {
                    continue;
                }

                var claimAttribute = (TokenClaimAttribute)Attribute.GetCustomAttribute(property, typeof(TokenClaimAttribute));
                if (claimAttribute == null) {
                    continue;
                }

                if (Utils.IsPropertyACollection(property)) {
                    foreach (object item in propertyValue as IEnumerable) {
                        tokenDescription.Subject.AddClaim(new Claim(claimAttribute.ClaimKey, item.ToString()));
                    }
                }
                else {
                    tokenDescription.Subject.AddClaim(new Claim(claimAttribute.ClaimKey, propertyValue.ToString()));
                }
            }

            // write the token out
            var jwtToken = handler.WriteToken(handler.CreateToken(tokenDescription));
            var splitToken = jwtToken.Split('.');   // NOTE: we split and the last part (security) should be stored into a Http-only cookie

            return new TokenDetails {
                Token = $"{splitToken[0]}.{splitToken[1]}",
                AccessToken = splitToken[2],
                RefreshToken = Guid.NewGuid().ToString("N")
            };
        }

        /// <inheritdoc cref="ITokenVerifier{T}.TryVerifyToken(string, out T)" />
        public bool TryVerifyToken(string token, out T signature) {
            try {
                signature = VerifyToken(token);
                if (signature != null) {
                    return true;
                }
                else {
                    return false;
                }
            }
            catch {
                signature = default;
                return false;
            }
        }

        /// <inheritdoc cref="ITokenVerifier{T}.VerifyToken(string)" />
        public T VerifyToken(string token) {
            var th = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_key));

            th.ValidateToken(token, new TokenValidationParameters {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = key,
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken securityToken);

            if (securityToken is not JwtSecurityToken validatedToken) {
                throw new Exception($"Invalid token ({token}).");
            }

            T obj = (T)Activator.CreateInstance(typeof(T));
            var properties = typeof(T).GetProperties();

            foreach (var property in properties) {
                var claimAttribute = (TokenClaimAttribute)Attribute.GetCustomAttribute(property, typeof(TokenClaimAttribute));
                if (claimAttribute == null) {
                    continue;
                }

                var claims = validatedToken.Claims.Where(x => x.Type == claimAttribute.ClaimKey);
                if (!claims.Any()) {
                    continue;
                }

                if (Utils.IsPropertyACollection(property) && claims.Count() > 1) {
                    // TODO: handle "2D Array" types?
                    var elementType = property.PropertyType.GenericTypeArguments.First();
                    var methodDefinition = typeof(Utils).GetMethod("ParseToArray");
                    var methodInfo = methodDefinition.MakeGenericMethod(elementType);

                    object collection = methodInfo.Invoke(null, new object[] { claims.ToArray() });
                    property.SetValue(obj, collection, null);
                }
                else {
                    if (property.PropertyType.Name == "Guid") {
                        var guidValue = Guid.Parse(claims.First().Value);
                        property.SetValue(obj, guidValue, null);
                    }
                    else {
                        property.SetValue(obj, Convert.ChangeType(claims.First().Value, property.PropertyType), null);
                    }
                }
            }

            return obj;
        }

        public TokenHandler(string key, int tokenExpiryMinutes) {
            _key = key.PadRight(32, 'A');
            _expiryMinutes = tokenExpiryMinutes;
        }
    }
}
