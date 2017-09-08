using AuthoringTagHelpers.Models;
using System.Collections.Generic;

namespace AuthoringTagHelpers.Interfaces
{
    public interface IToDoItemRepository
    {
        IEnumerable<ToDoItem> List();
    }
}
