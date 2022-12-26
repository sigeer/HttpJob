namespace SpiderTool
{
    public class Configs
    {
        public static string BaseDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "store");
        private static readonly string PropsFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "x.cache");
        public static void InitBaseDir()
        {
            if (!File.Exists(PropsFile))
            {
                File.WriteAllText(PropsFile, Configs.BaseDir);
            }
            else
            {
                Configs.BaseDir = File.ReadAllText(PropsFile);
            }
        }

        public static void UpdateBaseDir(string newBaseDir)
        {
            BaseDir = newBaseDir;
            File.WriteAllText(PropsFile, Configs.BaseDir);
        }

    }
}
