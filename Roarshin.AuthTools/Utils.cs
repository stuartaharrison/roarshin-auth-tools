using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Claims;

namespace Roarshin.AuthTools {
    
    internal static class Utils {
    
        /// <summary>
        /// Converts a Array of Claims into an Array<T>.
        /// </summary>
        /// <typeparam name="T">The type to convert the Claim value into.</typeparam>
        /// <param name="claims">List of Security.Claim objects from the decrytped token.</param>
        /// <returns>Returns an Array<T> of converted Claim objects.</returns>
        public static T[] ParseToArray<T>(Claim[] claims) {
            return claims.Select(x => (T)Convert.ChangeType(x.Value, typeof(T))).ToArray();
        }

        /// <summary>
        /// Checks to see if a specific object property is a type of Array and not a single property.
        /// </summary>
        /// <param name="property">The property to check.</param>
        /// <returns>TRUE if the property is an Array or inherits from IEnumerable<T></returns>
        public static bool IsPropertyACollection(PropertyInfo property) {
            if (!typeof(String).Equals(property.PropertyType) && (
                property.PropertyType.IsArray 
                || typeof(IEnumerable).IsAssignableFrom(property.PropertyType)
                || property.PropertyType.GetInterface(typeof(IEnumerable<>).FullName) != null)) {
                return true;
            }
            else {
                return false;
            }
        }
    }
}
