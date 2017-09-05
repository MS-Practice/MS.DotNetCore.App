using Logging.Demo.Core.Models;
using System.Collections.Generic;

namespace Logging.Demo.Core.Interfaces
{
    public interface ICharacterRepository
    {
        IEnumerable<Character> ListAll();
        void Add(Character character);
    }
}
