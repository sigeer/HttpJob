using SqlSugar;
using System.Reflection;

namespace SpiderTool.SqlSugar
{
    public static class DbMigration
    {
        public static void CreateDatabase(this ISqlSugarClient sqlClient, DbType dbType)
        {
            var assembly = Assembly.GetExecutingAssembly();
            using var sqlStream = assembly.GetManifestResourceStream($"{assembly.GetName().Name}.{dbType.ToString().ToLower()}.sql");

            if (sqlStream != null)
            {
                using var reader = new StreamReader(sqlStream);
                var sqlStr = reader.ReadToEnd();
                sqlClient.Ado.ExecuteCommand(sqlStr);
            }
        }
    }
}
