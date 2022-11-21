using Dapper;
using System.Data;
using System.Reflection;

namespace SpiderTool.Dapper
{
    public static class DbMigration
    {
        public static void CreateDataBase(this IDbConnection dbConnection, string dbType)
        {
            var assembly = Assembly.GetExecutingAssembly();
            using var sqlStream = assembly.GetManifestResourceStream($"{assembly.GetName().Name}.{dbType.ToLower()}.sql");

            if (sqlStream != null)
            {
                using var reader = new StreamReader(sqlStream);
                var sqlStr = reader.ReadToEnd();
                dbConnection.Execute(sqlStr);
            }
        }
    }
}
