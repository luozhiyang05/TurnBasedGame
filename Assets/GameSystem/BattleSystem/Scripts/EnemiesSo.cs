namespace Assets.GameSystem.BattleSystem.Scripts
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using GlobalData;
    using Tool.ResourceMgr;
    using Tool.Utilities.CSV;
    using UnityEngine;

    [CreateAssetMenu(fileName = "敌人库", menuName = "EnemiesSo", order = 0)]
    public class EnemiesSo : ScriptableObject
    {
        public TextAsset textAsset;
        public List<EnemyData> enemies;
        private void OnValidate()
        {
            if (textAsset != null)
            {
                enemies.Clear();
                CsvKit.Read<EnemyData>(textAsset, BindingFlags.Public | BindingFlags.Instance, value =>
                {
                    enemies.Add(value);
                });
            }
        }
        public int GetEnemyCnt()
        {
            return enemies.Count;
        }
        public Sprite GetEnemyIcon(int id)
        {
            return ResMgr.GetInstance().SyncLoad<Sprite>(GameManager.GetIconPath(enemies.Find(value=>value.id==id).iconName));
        }
        public string GetEnemyName(int id)
        {
            return enemies.Find(value=>value.id==id).enemyType.ToString();
        }
        public string GetEnemyDesc(int id)
        {
            return enemies.Find(value=>value.id==id).desc;
        }
        public EnemyData GetEnemyDataById(int id)
        {
            return enemies.Find(value=>value.id==id);
        }
    }

    [Serializable]
    public class EnemyData
    {
        public int id;
        public EnemyType enemyType;
        public string desc;
        public int maxHp;
        public int maxArmor;
        public int atk;
        public int skillId;
        public string iconName;
    }
}