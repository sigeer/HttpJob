using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpiderTool.Constants
{
    public class StatusMessage
    {
        public const string Success = "Success";
        public const string Error = "Error";
        public const string FormInvalid = "FormInvalid";
    }

    public class RequestMethod
    {
        public const string POST = "POST";
        public const string GET = "GET";
    }

    public enum ContentType
    {
        Html = 1,
        Text = 2,
        DownloadLink = 3,
        JumpLink = 4
    }
}
