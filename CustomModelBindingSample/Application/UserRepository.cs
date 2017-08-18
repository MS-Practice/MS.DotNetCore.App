using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomModelBindingSample.Application
{
    public class UserRepository : IUserRepository
    {
        private readonly HashSet<string> _emailAddresses = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        public bool VerifyEmail(string email)
        {
            return _emailAddresses.Add(email);
        }
    }
}
