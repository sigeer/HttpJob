using System.Xml;

namespace SpiderWin.Services
{
    public class SystemUpdate
    {
        const string NewlyVersionFile = "https://raw.githubusercontent.com/sigeer/HttpJob/master/src/SpiderWin/SpiderWin.csproj";
        const string InstallationPackage = "";

        public static async Task<UpdateInfo> GetNewlyVersion()
        {
            using var httpClient = new HttpClient();
            var data = await httpClient.GetStringAsync(NewlyVersionFile);
            var doc = new XmlDocument();
            doc.LoadXml(data);

            var rootNode = doc.SelectSingleNode("Project");
            if (rootNode != null)
            {
                var coreNode = rootNode.SelectSingleNode("PropertyGroup");
                if (coreNode == null)
                    return new UpdateInfo();

                var info = new UpdateInfo();
                info.Version = coreNode.SelectSingleNode("Version")?.InnerText ?? "---";
                info.Description = coreNode.SelectSingleNode("Description")?.InnerText ?? "---";
                return info;
            }
            return new UpdateInfo();
        }

        public static async Task<string> DownloadPackage()
        {
            using var httpClient = new HttpClient();
            var fileReponse = await httpClient.GetAsync(InstallationPackage);
            using var stream = fileReponse.Content.ReadAsStream();
            var bytes = new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);
            stream.Seek(0, SeekOrigin.Begin);
            File.WriteAllBytes(InstallationPackage, bytes);
            return InstallationPackage;
        }

        public static async Task Install()
        {
            //1.下载最新文件
            //2.调起更新脚本
            //3.脚本更新
            var installationPackage = await DownloadPackage();

        }
    }

    public class UpdateInfo
    {
        public string Version { get; set; } = null!;
        public string Description { get; set; } = null!;
    }
}
