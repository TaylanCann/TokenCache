using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TokenCache.Application.Exceptions
{
    public class UserNotFoundException : Exception
    {
        public UserNotFoundException(string username)
        : base($"User with username '{username}' not found.")
        {
        }

        public UserNotFoundException(string message, bool forLogin)
        : base(message)
        {
        }

        public static UserNotFoundException UserNotFoundForLogin(string username)
        {
            return new UserNotFoundException($"Login failed. User with username '{username}' does not exist.", true);
        }

    }
}
