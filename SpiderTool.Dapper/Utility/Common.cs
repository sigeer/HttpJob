using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpiderTool.Dapper.Utility
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
}
