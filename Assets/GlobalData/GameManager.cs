using Assets.GameSystem.BattleSystem.Scripts;
using Tool.ResourceMgr;
using Tool.Utilities.Save;
using UnityEngine;

namespace GlobalData
{
    public static class GameManager
    {
        public static Languages languages = Languages.中文;
        public const float nextWaveWaitTime = 1f;
        public const float actTntervalTime = 1f;

        public static void InitGlobalData()
        {
            // 读取保存的多语言设置
            languages = PlayerPrefs.GetInt("languages") switch
            {
                0 => Languages.中文,
                1 => Languages.English,
                _ => throw new System.Exception("多语言选择错误"),
            };
        }

        public static string GetText(string key,params string[] args)
        {
            string ReplaceFirst(string text, string search, string replace)
            {
                int pos = text.IndexOf(search);
                if (pos < 0)
                {
                    return text;
                }
                return text.Substring(0, pos) + replace + text.Substring(pos + search.Length);
            }

            var str = ResMgr.GetInstance().SyncLoad<MultipleLanguagesSo>("多语言配置").GetLanguageText(key);
            for (int i = 0; i < args.Length; i++)
            {
                str = ReplaceFirst(str, "%s", "{" + i + "}");
            }
            return string.Format(str, args);    // 替换字符串
        }
    }

    public enum Languages
    {
        中文,
        English
    }

    public enum CharacterType
    {
        character_cat,
        character_dog,
        character_duck
    }

    public enum EnemyType
    {
        enemy_slime,
        enemy_store
    }
}