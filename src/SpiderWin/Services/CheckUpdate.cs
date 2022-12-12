using Utility.Http;

namespace SpiderWin.Services
{
    public class CheckUpdate
    {
        const string NewlyVersionFile = "https://github.com/sigeer/HttpJob/raw/master/Version.txt";

        public static async Task<string> GetNewlyVersion()
        {
            var data = await HttpRequest.GetAsync(NewlyVersionFile);
            return data;
        }
    }
}
