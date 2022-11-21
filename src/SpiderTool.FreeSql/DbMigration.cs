using System.Reflection;

namespace SpiderTool.FreeSql
{
    public static class DbMigration
    {
        public static void CreateDatabase(this IFreeSql sqlClient, string dbType)
        {
            var assembly = Assembly.GetExecutingAssembly();
            using var sqlStream = assembly.GetManifestResourceStream($"{assembly.GetName().Name}.{dbType.ToLower()}.sql");

            if (sqlStream != null)
            {
                using var reader = new StreamReader(sqlStream);
                var sqlStr = reader.ReadToEnd();
                sqlClient.Ado.ExecuteNonQuery(sqlStr);
            }
        }
    }
}
