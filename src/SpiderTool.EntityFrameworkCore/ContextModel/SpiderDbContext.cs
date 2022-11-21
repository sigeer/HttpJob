using Microsoft.EntityFrameworkCore;
using SpiderTool.DataBase;

namespace SpiderTool.EntityFrameworkCore.ContextModel
{
    public class SpiderDbContext : DbContext
    {
#pragma warning disable CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。

        public DbSet<DB_Spider> Spiders { get; set; }

        public DbSet<DB_Template> Templates { get; set; }
        public DbSet<DB_SpiderTemplate> SpiderTemplates { get; set; }
        public DbSet<DB_ReplacementRule> ReplacementRules { get; set; }
        public DbSet<DB_Task> Tasks { get; set; }

        public SpiderDbContext(DbContextOptions<SpiderDbContext> options) : base(options)
        {
        }
#pragma warning restore CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
    }
}
