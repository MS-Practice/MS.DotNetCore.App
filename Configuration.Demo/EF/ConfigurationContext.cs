using Configuration.Demo.Entities;
using Microsoft.EntityFrameworkCore;

namespace Configuration.Demo.EF
{
    /// <summary>
    /// 存储和访问配置类
    /// </summary>
    public class ConfigurationContext:DbContext
    {
        public ConfigurationContext(DbContextOptions options) : base(options) { }

        public DbSet<ConfigurationValue> Values { get; set; }
    }
}
