namespace Assets.GameSystem.BattleSystem.Scripts
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using GlobalData;
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
        public int maxHp;
        public int maxArmor;
        public int atk;
        public int skillId;
    }
}