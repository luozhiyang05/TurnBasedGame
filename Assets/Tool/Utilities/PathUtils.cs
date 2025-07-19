namespace Tool.Utilities
{
    public static class PathUtils
    {
        private static readonly string START_NAME = "gamesystem/";
        private static readonly string END_NAME = "_ui_prefab";
        public static string GetSystemAssetBundlePath(string systemName)
        {
            var abName = START_NAME + systemName[..systemName.LastIndexOf("Module")] + END_NAME;
            return abName.ToLower();
        }
        public static string GetViewNameFromAbPath(string abPath)
        {
            return abPath.Split('/')[2];
        }
    }
}