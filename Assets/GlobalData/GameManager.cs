using Tool.ResourceMgr;

namespace GlobalData
{
    public static class GameManager
    {
        public const Languages languages = Languages.English;
        public const float SwitchTurnTime = 1.5f;
        public const float ActInternalTime = 1f;
        public const float BulletScreenTime = 1f;

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
        ENEMY_1,
        ENEMY_2
    }
}