using System;
using System.Collections.Generic;
using Framework;
using GameSystem.BattleSystem.Main;
using GameSystem.BattleSystem.Scripts;
using GlobalData;
using Tool.Mono;
using UnityEngine;
using UnityEngine.Events;

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
        /// 玩家行动
        /// </summary>
        void PlayerAct();

        /// <summary>
        /// 弹幕时间
        /// </summary>
        /// <param name="callback"></param>
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

        void SwitchTurn();

        public void ShowView();
    }

    public class BattleSystemModule : AbsModule, IBattleSystemModule
    {
        private BattleSystemViewCtrl _viewCtrl;


        public void ShowView()
        {
            _viewCtrl ??= new BattleSystemViewCtrl();
            _viewCtrl.OnShowView();
        }

        private AbsUnit _player;
        private List<AbsUnit> _enemies;
        private int _nowEnemyIndex;
        private ETurnBased _nowTurnBased = ETurnBased.Start; //当前状态枚举
        private AbsUnit currentUnit;

        protected override void OnInit()
        {
            _enemies = new List<AbsUnit>();
            _nowEnemyIndex = 0;
        }


        public void StartGame(AbsUnit player, List<AbsUnit> enemies)
        {
            //打开view
            ShowView();

            //获取玩家和敌人IUnit单位
            _player = player;
            _player.InitSystem(this);
            _enemies = enemies;
            for (var i = 0; i < enemies.Count; i++)
            {
                enemies[i].id = i;
                enemies[i].InitSystem(this);
            }

            //设置当前回合为玩家回合
            SwitchPlayerTurn();
        }

        public void SwitchTurn()
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
                _player.StartTurnSettle();
            },"玩家回合开始");
        }

        private void SwitchEnemyTurn()
        {
            //弹幕时间
            BulletScreenTimeDelegate(() =>
            {
                _nowTurnBased = ETurnBased.EnemyTurn;
                _enemies[_nowEnemyIndex++].StartTurnSettle();
            },"敌人回合开始");
        }

        private void JudgeIsHaveMoreEnemies()
        {
            //所有敌人行动完毕
            if (_nowEnemyIndex == _enemies.Count)
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
    }
}