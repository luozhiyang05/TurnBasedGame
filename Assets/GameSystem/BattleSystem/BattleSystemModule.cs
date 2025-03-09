using System;
using Assets.GameSystem.BattleSystem.Main;
using Assets.GameSystem.BattleSystem.Scripts;
using Assets.GameSystem.FlyTextSystem;
using Assets.GameSystem.MenuSystem.CharacterChose.Scripts;
using Assets.GameSystem.MenuSystem.LevelChose.Scripts;
using Framework;
using GlobalData;
using Tool.Mono;
using Tool.UI;
using Tool.Utilities;
using UnityEngine;

namespace Assets.GameSystem.BattleSystem
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
        /// 获取玩家角色数据
        /// </summary>
        /// <returns></returns>
        CharacterData GetCharacterData();

        /// <summary>
        /// 获取当前波次的敌人列表
        /// </summary>
        /// <returns></returns>
        QArray<AbsUnit> GetNowWavaAllEnemies();

        /// <summary>
        /// 切换回合
        /// </summary>
        void SwitchRound();

        /// <summary>
        /// 开始游戏
        /// </summary>
        void ShowView(CharacterData characterData, LevelData levelData);

        /// <summary>
        /// 切换至玩家回合
        /// </summary>
        void SwitchPlayerTurn();
    }

    public class BattleSystemModule : AbsModule, IBattleSystemModule
    {
        private BattleSystemViewCtrl _viewCtrl;

        public void ShowView(CharacterData characterData, LevelData levelData)
        {
            //打开试图
            _viewCtrl ??= new BattleSystemViewCtrl();
            _viewCtrl.ShowView(EuiLayer.GameUI, characterData, levelData);
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
                    // 敌人回合 弹幕
                    this.GetSystem<IFlyTextSystemModule>().FlyText(1, "battle_tip_1007", 1f, 0.5f);
                    SwitchEnemyTurn();
                    break;
                case ETurnBased.EnemyTurn:
                    JudgeIsHaveMoreEnemies();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void SwitchPlayerTurn()
        {
            // 你的回合 弹幕。弹幕结束后进入 你都会和
            this.GetSystem<IFlyTextSystemModule>().FlyText(0, "battle_tip_1005", 1f, 0.5f, () =>
            {
                _nowTurnBased = ETurnBased.PlayerTurn;
                (_viewCtrl.GetModel() as BattleSystemViewModel).UpdateRoundCnt();
                (_viewCtrl.GetModel() as BattleSystemViewModel).PlayerStartRoundSettle();
            });
        }

        private void SwitchEnemyTurn()
        {
            //间隔后切换敌人回合
            ActionKit.GetInstance().DelayTime(GameManager.actTntervalTime, () =>
            {
                _nowTurnBased = ETurnBased.EnemyTurn;
                (_viewCtrl.GetModel() as BattleSystemViewModel).EnemyStartRoundSettle();
            });
        }

        private void JudgeIsHaveMoreEnemies()
        {
            //所有敌人行动完毕
            if ((_viewCtrl.GetModel() as BattleSystemViewModel).IsEnemiesAfterAct())
            {
                // 敌人回合结束 弹幕，间隔后进入 你的回合
                this.GetSystem<IFlyTextSystemModule>().FlyText(1, "battle_tip_1008", 1f, 0.5f,()=>{
                    ActionKit.GetInstance().DelayTime(GameManager.actTntervalTime, () =>
                    {
                       SwitchPlayerTurn();
                    });
                });
            }
            else
            {
                SwitchEnemyTurn();
            }
        }

        public AbsUnit GetPlayerUnit()
        {
            return (_viewCtrl.GetModel() as BattleSystemViewModel).GetPlayerUnit();
        }

        public CharacterData GetCharacterData()
        {
            return (_viewCtrl.GetModel() as BattleSystemViewModel).GetCharacterData();
        }

        public QArray<AbsUnit> GetNowWavaAllEnemies()
        {
            return (_viewCtrl.GetModel() as BattleSystemViewModel).GetNowWavaAllEnemies();
        }
    }
}