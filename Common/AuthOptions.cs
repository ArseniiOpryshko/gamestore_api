using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace GameStore.Common
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
} // Bearer eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTUxMiIsInR5cCI6IkpXVCJ9.eyJJZCI6IjQiLCJOaWNrbmFtZSI6ImxvZ2VyIiwiRW1haWwiOiJ0aWt0b2s1cXdlcXdlQGdtYWlsLmNvbSIsIkNhcnRJZCI6IjQiLCJleHAiOjE3Mjc2OTI0NTIsImlzcyI6Imh0dHBzOi8vbG9jYWxob3N0OjcyMTciLCJhdWQiOiJodHRwczovL2xvY2FsaG9zdDo3MjE3In0.GJ4-z3cFpBdv5gisNezOpyG_XinEy1c2FkhbiQg15VR7JmwFuwh3Y2dV5oLl0XTjpJ2LOVHmQpg63GtBKHQ-Mg
