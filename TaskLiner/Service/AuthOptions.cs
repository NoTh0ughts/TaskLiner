using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace TaskLiner.Service
{
    public class AuthOptions
    {
        public const string IDUSER  = "Taskliner_server";
        public const string AUDIENCE = "Taskliner_client";
        public const string KEY = "TaskLiner_shawerma644";
        public const int LIFETIME = 1;
        public static SymmetricSecurityKey GetSymmetricSecurityKey() =>
            new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
    }
}