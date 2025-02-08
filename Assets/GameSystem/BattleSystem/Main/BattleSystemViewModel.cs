using System.Collections.Generic;
using Assets.GameSystem.BattleSystem.Scripts;
using Framework;
using GameSystem.BattleSystem.Scripts;
using GameSystem.MVCTemplate;
using Tips;
using Tool.Utilities;
using Tool.Utilities.Events;
using Unity.VisualScripting;
using UnityEngine;

namespace GameSystem.BattleSystem.Main
{
    public class BattleSystemViewModel : BaseModel
    {
        private QArray<AbsUnit> _enemyList = new QArray<AbsUnit>(10);
        private AbsUnit _player;
        private Level _level;
        private int _nowEnemyIndex; //当前敌人下标
        private int _nowWaveIndex;  //当前波次下标
        protected override void OnInit()
        {
            _nowEnemyIndex = 0;
            _nowWaveIndex = 1;
        }

        /// <summary>
        /// 监听某些数据更改事件,可以通知view更新
        /// </summary>
        public override void BindListener()
        {
            // //绑定敌人列表死亡事件
            // _enemyList.AddListenEvent(IListEventType.Remove, absUnit =>
            // {
            //     Debug.Log("死亡单位:" + absUnit.unitName + " id=" + absUnit.id);
            //     if (_enemyList.Count == 0)
            //     {
            //         Debug.Log("敌人全部死亡");
            //         TipsModule.ComfirmTips("提示", "恭喜你，你赢了", () => { });
            //     }
            // });

            // //绑定单位死亡事件
            // EventsHandle.AddListenEvent<AbsUnit>(EventsNameConst.ABSUNIT_DIE, absUnit =>
            // {
            //     UnitDie(absUnit);
            // });
        }

        /// <summary>
        /// 移除事件
        /// </summary>
        public override void RemoveListener()
        {
            // _enemyList.RemoveListenEvent(IListEventType.Remove, absUnit =>
            // {
            //     Debug.Log("死亡单位:" + absUnit.unitName + " id=" + absUnit.id);
            //     if (_enemyList.Count == 0)
            //     {
            //         Debug.Log("敌人全部死亡");
            //         TipsModule.ComfirmTips("提示", "恭喜你，你赢了", () => { });
            //     }
            // });

            // EventsHandle.RemoveOneEventByEventName<AbsUnit>(EventsNameConst.ABSUNIT_DIE, absUnit =>
            // {
            //     UnitDie(absUnit);
            // });
        }

        public void SetLevel(Level level)
        {
            _level = level;

            // _enemyList = new QArray<AbsUnit>(level.GetWavasCount());
            // for (int i = 0; i < _enemyList.Count; i++)
            // {
            //     enemies[i].id = i;
            //     enemies[i].InitSystem(this.GetSystem<IBattleSystemModule>());
            //     _enemyList.Add(enemies[i]);
            // }

            // //初始化玩家和敌人
            // _player = player;
            // _enemyList = new QArray<AbsUnit>(enemies.Count);
            // _player.InitSystem(this.GetSystem<IBattleSystemModule>());
            // for (var i = 0; i < enemies.Count; i++)
            // {
            //     enemies[i].id = i;
            //     enemies[i].InitSystem(this.GetSystem<IBattleSystemModule>());
            //     _enemyList.Add(enemies[i]);
            // }

            //  //设定敌人位置
            // var count = enemies.Count;
            // for (var i = 0; i < count; i++)
            // {
            //     var rangeIndex = Random.Range(0, _enemyPosTransList.Count);
            //     var enemyPosTrans = _enemyPosTransList[rangeIndex];
            //     enemies[i].transform.position = enemyPosTrans.position;
            //     _enemyPosTransList.Remove(enemyPosTrans);
            // }

            // //设定玩家位置
            // _player.transform.position = _playerPosTrans.position;
        }

        public Level GetLevel()
        {
            return _level;
        }

        public WavasData GetNowWava()
        {
            return GetLevel().GetWavaData(_nowWaveIndex);
        }

        public void SetEnemyAbsUnit(AbsUnit absUnit)
        {
            _enemyList.Add(absUnit);
        }

        public void SetPlayerAbsUnit(AbsUnit absUnit)
        {
            _player = absUnit;
        }

        public void EnemyStartRoundSettle()
        {
            _enemyList[_nowEnemyIndex++].StartRoundSettle();
        }

        public void PlayerStartRoundSettle()
        {
            _player.StartRoundSettle();
        }

        public bool IsEnemiesAfterAct()
        {
            var isEnemiesAfterAct = _nowEnemyIndex == _enemyList.Count;
            _nowEnemyIndex = isEnemiesAfterAct ? 0 : _nowEnemyIndex;
            return isEnemiesAfterAct;
        }

        public int GetEnemyCount()=> _enemyList.Count;
        
        public AbsUnit GetPlayerUnit()=> _player;

        public void UnitDie(AbsUnit unit)
        {
            if (unit is Enemy)
            {
                _enemyList.Remove(unit);
            }
            else
            {
                Debug.Log("玩家死亡");
            }
        }
    }
}