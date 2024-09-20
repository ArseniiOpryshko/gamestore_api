using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Museum.Data.ObjsForAuth
{
    public class AuthOptions
    {
        const string KEY = "phah1game1store1key this is my custom Secret key for authentication";
        public const int LIFETIME = 24;
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }
    }
}
