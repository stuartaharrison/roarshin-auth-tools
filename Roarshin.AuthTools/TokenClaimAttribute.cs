using System;

namespace Roarshin.AuthTools {
    
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class TokenClaimAttribute : Attribute {
    
        public string ClaimKey { get; set; }

        public TokenClaimAttribute(string key) {
            ClaimKey = key;
        }
    }
}
