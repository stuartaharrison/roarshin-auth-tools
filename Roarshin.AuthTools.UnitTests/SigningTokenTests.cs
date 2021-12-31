using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Roarshin.AuthTools.UnitTests {
    
    public class Tests {

        private ITokenSigner<UserDetails> _tokenSigner;

        [SetUp]
        public void Setup() {
            _tokenSigner = new TokenHandler<UserDetails>("UNITTESTS", 120);
        }

        [Test]
        public void SignBasicToken() {
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
            Assert.That(() => !string.IsNullOrWhiteSpace(tokenDetails.Token));
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