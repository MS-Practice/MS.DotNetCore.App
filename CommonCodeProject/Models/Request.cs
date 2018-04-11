using System;
using System.Collections.Generic;
using System.Text;

namespace CommonCodeProject.Models
{
    public class Request
    {
        public int Id { get; set; }
        public DateTime DT { get; set; }
        public string MiddlewareActivation { get; set; }
        public string Value { get; set; }
    }
}
