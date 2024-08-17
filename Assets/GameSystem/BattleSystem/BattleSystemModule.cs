using System;
using System.Collections.Generic;
using Framework;
using GameSystem.BattleSystem.Main;
using GameSystem.BattleSystem.Scripts;
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
        /// 切换状态
        /// </summary>
        /// <param name="self"></param>
        /// <param name="eTurnBased"></param>
        void SwitchState(AbsUnit self, ETurnBased eTurnBased);

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
        private ETurnBased _state = ETurnBased.Start; //当前状态枚举
        private AbsUnit currentUnit;
        protected override void OnInit()
        {
            _enemies = new List<AbsUnit>();
            _nowEnemyIndex = 0;
        }
        
 

        public void StartGame(AbsUnit player, List<AbsUnit> enemies)
        {
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
            SwitchState(_player, ETurnBased.Start);
        }

        public void SwitchState(AbsUnit self, ETurnBased eTurnBased)
        {
            switch (eTurnBased)
            {
                case ETurnBased.Start:
                    _player.Enter();
                    break;
                case ETurnBased.PlayerTurn:
                    _enemies[_nowEnemyIndex].Exit();
                    _nowEnemyIndex = 0; 
                    _player.Enter();
                    break;
                case ETurnBased.EnemyTurn:
                    _player.Exit();
                    _enemies[_nowEnemyIndex].Enter();
                    break;
                //TODO:玩家胜利
                case ETurnBased.Win:
                    Debug.LogError("玩家胜利");
                    break;
                //TODO:玩家失败
                case ETurnBased.Lose:
                    Debug.LogError("玩家失败");
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(eTurnBased), eTurnBased, null);
            }
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
                SwitchState(_enemies[_nowEnemyIndex],ETurnBased.PlayerTurn);
                return;
            }
            
            //下一个敌人开始行动
            _enemies[_nowEnemyIndex].Exit();
            _nowEnemyIndex++;
            _enemies[_nowEnemyIndex].Enter();
        }
    }
}