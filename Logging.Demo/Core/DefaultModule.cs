using Autofac;
using Logging.Demo.Core.Interfaces;
using Logging.Demo.Infrastructure;

namespace Logging.Demo.Core
{
    public class DefaultModule:Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<CharacterRepository>().As<ICharacterRepository>();
        }
    }
}
