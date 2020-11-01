using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RequestsService
{
    public class AuthOptions
    {
        public const string ISSUER = "RequestsService"; // издатель токена
        public const string AUDIENCE = "User"; // потребитель токена
        const string KEY = "Gudkov_key_secret";   // ключ для шифрации
        public const int LIFETIME = 1; // время жизни токена - 1 минута

        /// <summary>
        /// Get symmetric security key
        /// </summary>
        /// <returns>symmetric security key</returns>
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }
    }
}
