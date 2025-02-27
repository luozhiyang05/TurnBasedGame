using Assets.GameSystem.BattleSystem.Scripts;
using Tool.ResourceMgr;
using Tool.Utilities.Save;
using UnityEngine;

namespace GlobalData
{
    public static class GameManager
    {
        public static Languages languages = Languages.中文;
        public const float SwitchTurnTime = 1.5f;
        public const float ActInternalTime = 1f;
        public const float BulletScreenTime = 1f;

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

        public static string GetText(string key)
        {
            return ResMgr.GetInstance().SyncLoad<MultipleLanguagesSo>("多语言配置").GetLanguageText(key);
        }
    }

    public enum Languages
    {
        中文,
        English
    }

    public enum CharacterType
    {
        猫猫,
        狗狗,
        鸭子
    }

    public enum EnemyType
    {
        enemy_slime,
        enemy_store
    }
}