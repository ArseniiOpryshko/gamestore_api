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
} // Bearer eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTUxMiIsInR5cCI6IkpXVCJ9.eyJJZCI6IjMiLCJOaWNrbmFtZSI6IndvcmtlciIsIkVtYWlsIjoidGlrdG9rNHF3ZXF3ZUBnbWFpbC5jb20iLCJDYXJ0SWQiOiIzIiwiZXhwIjoxNzI3NTIwODgwLCJpc3MiOiJodHRwczovL2xvY2FsaG9zdDo3MjE3IiwiYXVkIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6NzIxNyJ9.mSPoKjS_6qcUPXlFvir--D8UFr5NRWubibq3akzd-zu83JhIp0Ll1qjCN1wLFH9WcQrMq5E-X-rvG5CmobOIFw
