using System.ComponentModel.DataAnnotations.Schema;

namespace SpiderTool.Constants
{
    public static class CommonExtension
    {
        public static string GetTableName(this Type type)
        {
            var attrs = (TableAttribute[])type.GetCustomAttributes(typeof(TableAttribute), false);
            if (attrs.Length > 0 && attrs[0] != null)
            {
                return attrs[0].Name;
            }
            return type.Name;
        }
    }

    public class Constants
    {
        public const string DBName = "spider";
    }
}
