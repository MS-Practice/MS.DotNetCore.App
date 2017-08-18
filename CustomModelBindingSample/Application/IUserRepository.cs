using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomModelBindingSample.Application
{
    public interface IUserRepository
    {
        bool VerifyEmail(string email);
    }
}
