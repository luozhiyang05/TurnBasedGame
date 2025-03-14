using System;
using System.Collections.Generic;
using System.Reflection;
using Assets.GameSystem.BattleSystem.Scripts;
using GlobalData;
using Tool.ResourceMgr;
using Tool.Utilities.CSV;
using UnityEngine;
namespace Assets.GameSystem.MenuSystem.LevelChose.Scripts
{

    [CreateAssetMenu(fileName = "LevelsSo", menuName = "LevelsSo", order = 0)]
    public class LevelsSo : ScriptableObject
    {
        public TextAsset textAsset;
        public List<LevelData> levels;
        private void OnValidate() {
            if (textAsset!=null)
            {
                levels.Clear();
                CsvKit.Read<LevelData>(textAsset, BindingFlags.Public | BindingFlags.Instance,ValueTuple=>{
                    levels.Add(ValueTuple);
                });
            }
        }

        /// <summary>
        /// 根据关卡id获取关卡数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public LevelData GetLevelDataById(int id)
        {
            return levels.Find(value=>value.id==id);
        }

        /// <summary>
        /// 根据关卡id获取关卡名字
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetLevelNameById(int id)
        {
            string key = levels.Find(value=>value.id==id).levelName;
            return GameManager.GetText(key);
        }
    }

    [Serializable]
    public class LevelData
    {
        public int id;
        public string levelName;
        public int waveCnt;
        public string enemyIds; //1-2/1-2
        public string bgName;

        /// <summary>
        /// 获取当前关卡的波次数量
        /// </summary>
        /// <returns></returns>
        public int GetWaveCnt()
        {
            return waveCnt;
        }

        /// <summary>
        /// 获取当前波次的敌人数据
        /// </summary>
        /// <param name="wava"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public WavasData GetWavaData(int wava)
        {
            if (wava <= 0 || wava > waveCnt)
            {
                throw new Exception("波次读取下标错误");
            }

            var enemiesSo = ResMgr.GetInstance().SyncLoad<EnemiesSo>("敌人库");
            var wavaData = new WavasData
            {
                enemies = new List<EnemyData>()
            };

            string[] enemyIdsSplit = enemyIds.Split('/')[wava-1].Split('-');
            for (int i = 0; i < enemyIdsSplit.Length; i++)
            {
                wavaData.enemies.Add(enemiesSo.GetEnemyDataById(int.Parse(enemyIdsSplit[i])));
            }
            return wavaData;
        }
    }

    [Serializable]
    public class WavasData
    {
        public List<EnemyData> enemies;
    }

}