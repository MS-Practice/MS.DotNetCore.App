using Logging.Demo.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Logging.Demo.Core.Services
{
    public class OperationService
    {
        public IOperationTransient OperationTransient { get; }
        public IOperationScoped OperationScoped { get; }
        public IOperationSingleton OperationSingleton { get; }
        public IOperationSingletonInstance SingletonInstanceOperation { get; set; }

        public OperationService(IOperationTransient transientOperation,
            IOperationScoped scopedOperation,
            IOperationSingleton singletonOperation,
            IOperationSingletonInstance singletonInstanceOperation)
        {
            OperationScoped = scopedOperation;
            OperationSingleton = singletonOperation;
            OperationTransient = transientOperation;
            SingletonInstanceOperation = singletonInstanceOperation;
        }
    }
}
