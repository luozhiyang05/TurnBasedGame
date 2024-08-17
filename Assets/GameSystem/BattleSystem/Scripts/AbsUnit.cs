using System;
using GlobalData;
using Tool.Mono;
using UnityEngine;

namespace GameSystem.BattleSystem.Scripts
{
    public abstract class AbsUnit : MonoBehaviour
    {
        public int id;
        public string unitName; //单位名称
        public float maxHp; //最大血量
        public float nowHp; //当前血量
        public float atk; //攻击
        public float armor; //护盾
        private IBattleSystemModule _battleSystemModule;
        protected ETurnBased ETurnBased;
        
        /// <summary>
        /// 初始化数据
        /// </summary>
        /// <param name="iBattleSystemModule"></param>
        public void InitSystem(IBattleSystemModule iBattleSystemModule)
        {
            this._battleSystemModule = iBattleSystemModule;
            nowHp = maxHp;
        }

        //是否死亡
        public bool IsDie()
        {
            return nowHp <= 0;
        }

        //回合开始
        public virtual void Enter(ETurnBased eTurnBased)
        {
            ETurnBased = eTurnBased;
            //玩家的行动自己操控，除非是自动战斗
            if (ETurnBased is ETurnBased.PlayerTurn or ETurnBased.Start) return;
            ActionKit.GetInstance().DelayTime(GameManager.SwitchTurnTime, Action);
        }

        //单位行动
        public virtual void Action()
        {
            ActionKit.GetInstance().DelayTime(GameManager.ActInternalTime,Exit);
        }

        //回合结算
        public virtual void Exit()
        {
            switch (ETurnBased)
            {
                case ETurnBased.EnemyTurn:
                    _battleSystemModule.MoreEnemyTurn();
                    break;
                case ETurnBased.PlayerTurn:
                    _battleSystemModule.SwitchEnemyTurn();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}