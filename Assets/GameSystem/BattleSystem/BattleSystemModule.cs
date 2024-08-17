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

        void SwitchPlayerTurn();

        void SwitchEnemyTurn();

        void PlayerAct();
        

        /// <summary>
        /// 施加效果
        /// </summary>
        /// <param name="self"></param>
        /// <param name="target"></param>
        /// <param name="affect"></param>
        void Affect(AbsUnit self, AbsUnit target, UnityAction<AbsUnit, AbsUnit> affect);

        /// <summary>
        /// 多敌人回合判断
        /// </summary>
        void MoreEnemyTurn();
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

        public void SwitchPlayerTurn()
        {
            
            ActionKit.GetInstance().DelayTime(GameManager.SwitchTurnTime, () =>
            {
                _player.Enter(ETurnBased.PlayerTurn);
            });
           
        }

        public void SwitchEnemyTurn()
        {
            ActionKit.GetInstance().DelayTime(GameManager.SwitchTurnTime, () =>
            {
                _nowEnemyIndex = 0;
                _enemies[_nowEnemyIndex].Enter(ETurnBased.EnemyTurn);
            });
        }

        public void PlayerAct()
        {
            _player.Action();
        }


        public void Affect(AbsUnit self, AbsUnit target, UnityAction<AbsUnit, AbsUnit> affect)
        {
            affect?.Invoke(self, target);
        }

        
        public void MoreEnemyTurn()
        {
            //所有敌人行动完毕
            if (_nowEnemyIndex == _enemies.Count - 1)
            {
                SwitchPlayerTurn();
                return;
            }
            
            //下一个敌人开始行动
            ActionKit.GetInstance().DelayTime(GameManager.SwitchTurnTime, () =>
            {
                _enemies[++_nowEnemyIndex].Enter(ETurnBased.EnemyTurn);
            });
        }

    }
}