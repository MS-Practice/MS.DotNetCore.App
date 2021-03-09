using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetCore.Fundamentals.Controllers
{
    public class LControllerBase : Controller
    {
        public IStringLocalizer L { get; }
        public LControllerBase(IStringLocalizer l)
        {
            L = l;
        }
    }
}
