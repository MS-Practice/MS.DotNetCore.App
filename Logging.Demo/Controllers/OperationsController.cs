using Microsoft.AspNetCore.Mvc;
using Logging.Demo.Core.Services;
using Logging.Demo.Core.Interfaces;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Logging.Demo.Controllers
{
    public class OperationsController : Controller
    {
        private readonly OperationService _operationService;
        private readonly IOperationTransient _operationTransient;
        private readonly IOperationScoped _operationScoped;
        private readonly IOperationSingleton _operationSingleton;
        private readonly IOperationSingletonInstance _operationSingletonInstance;

        public OperationsController(OperationService operationService,
            IOperationTransient transientOperation,
            IOperationScoped scopedOperation,
            IOperationSingleton singletonOperation,
            IOperationSingletonInstance singletonInstanceOperation)
        {
            _operationScoped = scopedOperation;
            _operationTransient = transientOperation;
            _operationService = operationService;
            _operationSingleton = singletonOperation;
            _operationSingletonInstance = singletonInstanceOperation;
        }

        public IActionResult Index()
        {
            ViewBag.Transient = _operationTransient;
            ViewBag.Scoped = _operationScoped;
            ViewBag.Singleton = _operationSingleton;
            ViewBag.SingletonInstance = _operationSingletonInstance;
            ViewBag.Service = _operationService;
            return View();
        }
    }
}
