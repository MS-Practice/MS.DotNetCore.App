using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Logging.Demo.Core.Interfaces;
using Logging.Demo.Core.Models;
using Logging.Demo.Core;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Logging.Demo.Controllers
{
    public class TodoController : Controller
    {
        private readonly ITodoRepository _todoRepository;
        private readonly ILogger _logger;

        public TodoController(ITodoRepository todoRepository,ILoggerFactory loggerFactory)
        {
            _todoRepository = todoRepository;
            _logger = loggerFactory.CreateLogger("TodoApi.Controllers.TodoController");
        }

        public TodoController(ITodoRepository todoRepository,ILogger logger)
        {
            _todoRepository = todoRepository;
            _logger = logger;
        }

        public IActionResult GetById(string id)
        {
            _logger.LogInformation(LoggingEvents.GetItem, "Getting item {ID}", id);
            var item = _todoRepository.Find(id);
            if(item == null)
            {
                _logger.LogWarning(LoggingEvents.GetItemNotFound, "GetById({ID}) NOT FOUND", id);
                return NotFound();
            }
            return new ObjectResult(item);
        }

        public IEnumerable<TodoItem> GetAll()
        {
            using (_logger.BeginScope("Message {HoleValue}",DateTime.Now))
            {
                _logger.LogInformation(LoggingEvents.ListItems, "Listing all items");
                EnsureItems();
            }
            return _todoRepository.GetAll();
        }

        private void EnsureItems()
        {
            if (!_todoRepository.GetAll().Any())
            {
                _logger.LogInformation(LoggingEvents.GenerateItems, "Generating sample items.");
            }
        }
    }
}
