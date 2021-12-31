using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roarshin.AuthTools.UnitTests {
    
    public class VerifyingTokenTests {

        private ITokenSigner<UserDetails> _tokenSigner;
        private ITokenVerifier<UserDetails> _tokenVerifier;

        [SetUp]
        public void Setup() {
            _tokenSigner = new TokenHandler<UserDetails>("UNITTESTS", 120);
            _tokenVerifier = new TokenHandler<UserDetails>("UNITTESTS", 120);
        }

        [Test]
        public void VerifyBasicToken() {
            // create and sign a token
            var userToSign = new UserDetails {
                UserId = Guid.Parse("c27b8008-73e4-44d6-958d-9c927de9a862"),
                Username = "Roarshin",
                Age = 30,
                CreditNumber = 123456678922334,
                CreditLimit = 3500.00m,
                Location = 1.0f,
                IsActive = true,
                Roles = new List<string> {
                    "Admin",
                    "Security",
                    "Tester"
                }
            };
            var tokenDetails = _tokenSigner.SignToken(userToSign);

            // Now verify that the signed token is okay!
            var tokenSignature = _tokenVerifier.VerifyToken($"{tokenDetails.Token}.{tokenDetails.AccessToken}");

            Assert.AreEqual(tokenSignature.Username, userToSign.Username);
        }

        private class UserDetails {

            [TokenClaim("uid")]
            public Guid UserId { get; set; }

            [TokenClaim("username")]
            public string Username { get; set; }

            [TokenClaim("age")]
            public int Age { get; set; }

            [TokenClaim("creditno")]
            public long CreditNumber { get; set; }

            [TokenClaim("limit")]
            public decimal CreditLimit { get; set; }

            [TokenClaim("location")]
            public float Location { get; set; }

            [TokenClaim("active")]
            public bool IsActive { get; set; }

            [TokenClaim("role")]
            public IEnumerable<string> Roles { get; set; }
        }
    }
}
