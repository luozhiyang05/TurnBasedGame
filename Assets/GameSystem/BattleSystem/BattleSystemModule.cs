using System;
using System.Collections.Generic;
using Framework;
using GameSystem.BattleSystem.Main;
using GameSystem.BattleSystem.Scripts;
using GlobalData;
using Tips;
using Tool.Mono;
using Tool.Utilities;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace GameSystem.BattleSystem
{
    /// <summary>
    /// 回合
    /// </summary>
    public enum ETurnBased
    {
        Start,
        PlayerTurn,
        EnemyTurn,
        Win,
        Lose
    }

    public interface IBattleSystemModule : IModule
    {
        /// <summary>
        /// 开始回合制
        /// </summary>
        void StartGame(AbsUnit player, List<AbsUnit> enemies);
        
        /// <summary>
        /// 获取玩家Unit
        /// </summary>
        /// <returns></returns>
        AbsUnit GetPlayerUnit();

        /// <summary>
        /// 获取敌人Unit
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        AbsUnit GetEnemyUnit(int index);
        
        /// <summary>
        /// 玩家行动
        /// </summary>
        void PlayerAct();

        /// <summary>
        /// 弹幕时间
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="screenInfo"></param>
        void BulletScreenTimeDelegate(Action callback,string screenInfo);

        /// <summary>
        /// 行动间隔时间
        /// </summary>
        /// <param name="callback"></param>
        void ActInternalTimeDelegate(Action callback);

        /// <summary>
        /// 切换回合时间
        /// </summary>
        /// <param name="callback"></param>
        void SwitchTurnTimeDelegate(Action callback);
        
        /// <summary>
        /// 施加效果
        /// </summary>
        /// <param name="self"></param>
        /// <param name="target"></param>
        /// <param name="affect"></param>
        void Affect(AbsUnit self, AbsUnit target, UnityAction<AbsUnit, AbsUnit> affect);

        /// <summary>
        /// 单位死亡
        /// </summary>
        /// <param name="unit"></param>
        void UnitDie(AbsUnit unit);
        void SwitchRound();
        void ShowView();

    
    }

    public class BattleSystemModule : AbsModule, IBattleSystemModule
    {
        private BattleSystemViewCtrl _viewCtrl;
        private List<Transform> _enemyPosTransList;
        private Transform _playerPosTrans;
        private QArray<AbsUnit> _enemyList;


        public void ShowView()
        {
            _viewCtrl ??= new BattleSystemViewCtrl();
            _viewCtrl.ShowView();
        }

        public AbsUnit GetPlayerUnit()
        {
            return _player;
        }

        public AbsUnit GetEnemyUnit(int index)
        {
            if (index>=0 && index<_enemyList.Count)
            {
                return _enemyList[index];
            }

            throw new Exception("敌人下标错误");
        }

        private AbsUnit _player;
        private int _nowEnemyIndex;
        private ETurnBased _nowTurnBased = ETurnBased.Start; //当前状态枚举
        private AbsUnit currentUnit;

        protected override void OnInit()
        {
            _enemyPosTransList = new List<Transform>();
            _enemyList = new QArray<AbsUnit>(1);
            _nowEnemyIndex = 0;

            //绑定敌人列表死亡事件
            _enemyList.AddListenEvent(IListEventType.Remove, absUnit =>
            {
                Debug.Log("死亡单位:" + absUnit.unitName + " id=" + absUnit.id);
                if (_enemyList.Count == 0)
                {
                    Debug.Log("敌人全部死亡");
                    TipsModule.ComfirmTips("提示", "恭喜你，你赢了", ()=>{});
                }
            });

            //寻找敌人生成位置
            var _enemyPosTrans = GameObject.Find("EnemyPos").transform;
            foreach (Transform trans in _enemyPosTrans)
            {
                _enemyPosTransList.Add(trans);
            }
            
            //寻找玩家生成位置
            _playerPosTrans = GameObject.Find("PlayerPos").transform;
        }


        public void StartGame(AbsUnit player, List<AbsUnit> enemies)
        {
            //打开view
            ShowView();

            //获取玩家和敌人IUnit单位
            _player = player;
            _player.InitSystem(this);
            for (var i = 0; i < enemies.Count; i++)
            {
                enemies[i].id = i;
                enemies[i].InitSystem(this);
                _enemyList.Add(enemies[i]);
            }

            //设定敌人位置
            var count = enemies.Count;
            for (var i = 0; i < count; i++)
            {
                var rangeIndex = Random.Range(0, _enemyPosTransList.Count);
                var enemyPosTrans = _enemyPosTransList[rangeIndex];
                enemies[i].transform.position = enemyPosTrans.position;
                _enemyPosTransList.Remove(enemyPosTrans);
            }
            
            //设定玩家位置
            _player.transform.position = _playerPosTrans.position;

            //设置当前回合为玩家回合
            SwitchPlayerTurn();
        }

        public void SwitchRound()
        {
            switch (_nowTurnBased)
            {
                case ETurnBased.PlayerTurn:
                    SwitchEnemyTurn();
                    break;
                case ETurnBased.EnemyTurn:
                    JudgeIsHaveMoreEnemies();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void SwitchPlayerTurn()
        {
            //弹幕时间
            BulletScreenTimeDelegate(() =>
            {
                _nowTurnBased = ETurnBased.PlayerTurn;
                _player.StartRoundSettle();
            },"玩家回合开始");
        }

        private void SwitchEnemyTurn()
        {
            //弹幕时间
            BulletScreenTimeDelegate(() =>
            {
                _nowTurnBased = ETurnBased.EnemyTurn;
                _enemyList[_nowEnemyIndex++].StartRoundSettle();
            },"敌人回合开始");
        }

        private void JudgeIsHaveMoreEnemies()
        {
            //所有敌人行动完毕
            if (_nowEnemyIndex == _enemyList.Count)
            {
                _nowEnemyIndex = 0;
                SwitchPlayerTurn();
            }
            else
            {
                SwitchEnemyTurn();
            }
        }


        public void BulletScreenTimeDelegate(Action callback,string screenInfo)
        {
            Debug.Log($"（弹幕：{screenInfo}）..........");
            ActionKit.GetInstance().DelayTime(GameManager.BulletScreenTime, callback);
        }

        public void ActInternalTimeDelegate(Action callback)
        {
            Debug.Log("(行动间隔)........");
            ActionKit.GetInstance().DelayTime(GameManager.ActInternalTime, callback);
        }

        public void SwitchTurnTimeDelegate(Action callback)
        {
            Debug.Log("(切换回合)........");
            ActionKit.GetInstance().DelayTime(GameManager.SwitchTurnTime, callback);
        }

        public void PlayerAct()
        {
            _player.Action();
        }

        public void Affect(AbsUnit self, AbsUnit target, UnityAction<AbsUnit, AbsUnit> affect)
        {
            affect?.Invoke(self, target);
        }

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