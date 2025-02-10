using System;
using System.Collections.Generic;
using GlobalData;
using UnityEngine;
namespace Assets.GameSystem.MenuSystem.LevelChose.Scripts
{

    [CreateAssetMenu(fileName = "LevelsSo", menuName = "LevelsSo", order = 0)]
    public class LevelsSo : ScriptableObject
    {
        public List<Level> levels;
        public Level GetLevelById(int id)
        {
            if (id <= 0 || id > levels.Count)
            {
                throw new Exception("关卡读取下标错误");
            }
            return levels[id - 1];
        }
    }

    [Serializable]
    public class Level
    {
        public string levelName;
        public List<WavasData> wavasDatas;
        public WavasData GetWavaData(int wava)
        {
            if (wava <= 0 || wava > wavasDatas.Count)
            {
                throw new Exception("波次读取下标错误");
            }
            return wavasDatas[wava - 1];
        }
        public int GetWavasCount()
        {
            return wavasDatas.Count;
        }
    }

    [Serializable]
    public class WavasData
    {
        public string wavaName;
        public List<EnemyData> enemies;
    }

    [Serializable]
    public class EnemyData
    {
        public EnemyType enemyType;
        public string name=>enemyType.ToString();
        public int maxHp;
        public int maxArmor;
        public int atk;
    }

}