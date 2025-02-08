using System;
using System.Collections.Generic;
using GlobalData;
using UnityEngine;
namespace Assets.GameSystem.BattleSystem.Scripts
{

    [CreateAssetMenu(fileName = "LevelsSo", menuName = "LevelsSo", order = 0)]
    public class LevelsSo : ScriptableObject
    {
        public List<Level> levels;
        public Level GetLevelData(int level)
        {
            if (level<=0 || level>levels.Count)
            {
                throw new Exception("关卡读取下标错误");
            }
            return levels[level-1];
        }
    }

    [Serializable]
    public class Level
    {
        public string name;
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
        public List<EnemyType> enemyTypes;
    }

}