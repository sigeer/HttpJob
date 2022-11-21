using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace SpiderTool.EntityFrameworkCore
{
    public static class DbMigration
    {
        public static void CreateDataBase(this DbContext dbContext, string dbType)
        {
            var assembly = Assembly.GetExecutingAssembly();
            using var sqlStream = assembly.GetManifestResourceStream($"{assembly.GetName().Name}.{dbType.ToLower()}.sql");

            if (sqlStream != null)
            {
                using var reader = new StreamReader(sqlStream);
                var sqlStr = reader.ReadToEnd();
                dbContext.Database.ExecuteSqlRaw(sqlStr);
            }
        }
    }
}
