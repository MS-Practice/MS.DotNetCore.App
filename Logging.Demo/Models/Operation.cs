using Logging.Demo.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Logging.Demo.Models
{
    public class Operation : IOperationTransient,IOperationScoped,IOperationSingleton,IOperationSingletonInstance
    {
        public Operation() : this(Guid.NewGuid())
        {

        }

        public Operation(Guid guid)
        {
            OperationId = guid;
        }

        public Guid OperationId { get; private set; }
    }
}
