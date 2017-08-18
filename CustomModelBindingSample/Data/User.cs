using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomModelBindingSample.Data
{
    public class User
    {
        [Remote(action:"VerifyEmail",controller:"Users")]
        public string Email { get; set; }
    }
}
