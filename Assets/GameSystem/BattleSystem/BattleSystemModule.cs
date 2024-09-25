using System;
using Framework;
using GameSystem.BattleSystem.Main;
using GameSystem.BattleSystem.Scripts;
using GlobalData;
using Tool.Mono;
using Tool.Utilities;
using UnityEngine;

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
        /// 获取玩家单位
        /// </summary>
        /// <returns></returns>
        AbsUnit GetPlayerUnit();
        
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
        /// 切换回合
        /// </summary>
        void SwitchRound();

        /// <summary>
        /// 开始游戏
        /// </summary>
        /// <param name="player"></param>
        /// <param name="enemies"></param>
        void ShowView(AbsUnit player, QArray<AbsUnit> enemies);
    }

    public class BattleSystemModule : AbsModule, IBattleSystemModule
    {
        private BattleSystemViewCtrl _viewCtrl;

        public void ShowView(AbsUnit player, QArray<AbsUnit> enemies)
        {
            //设置当前回合为玩家回合
            SwitchPlayerTurn();

            //打开试图
            _viewCtrl ??= new BattleSystemViewCtrl(player, enemies);
            _viewCtrl.ShowView();
        }

        private ETurnBased _nowTurnBased = ETurnBased.Start; //当前状态枚举

        protected override void OnInit()
        {
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
                (_viewCtrl.GetModel() as BattleSystemViewModel).PlayerStartRoundSettle();
            },"玩家回合开始");
        }

        private void SwitchEnemyTurn()
        {
            //弹幕时间
            BulletScreenTimeDelegate(() =>
            {
                _nowTurnBased = ETurnBased.EnemyTurn;
                (_viewCtrl.GetModel() as BattleSystemViewModel).EnemyStartRoundSettle();
            },"敌人回合开始");
        }

        private void JudgeIsHaveMoreEnemies()
        {
            //所有敌人行动完毕
            if ((_viewCtrl.GetModel() as BattleSystemViewModel).IsEnemiesAfterAct())
            {
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

        public AbsUnit GetPlayerUnit()
        {
           return (_viewCtrl.GetModel() as BattleSystemViewModel).GetPlayerUnit();
        }
    }
}