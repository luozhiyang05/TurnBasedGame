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
        protected IBattleSystemModule BattleSystemModule;

        /// <summary>
        /// 初始化数据
        /// </summary>
        /// <param name="iBattleSystemModule"></param>
        public void InitSystem(IBattleSystemModule iBattleSystemModule)
        {
            this.BattleSystemModule = iBattleSystemModule;
            nowHp = maxHp;
        }

        //是否死亡
        public bool IsDie()
        {
            return nowHp <= 0;
        }

        //回合开始结算
        public abstract void StartTurnSettle();

        //回合开始结算结束
        protected void AfterStartTurnSettle()
        {
            //行动间隔
            BattleSystemModule.ActInternalTimeDelegate(Action);
        }
        
        //行动
        public abstract void Action();

        //行动结束
        protected void AfterAction()
        {
            //行动间隔
            BattleSystemModule.ActInternalTimeDelegate(() =>
            {
                //弹幕时间
                BattleSystemModule.BulletScreenTimeDelegate(Exit,"回合结束");
            });
        }

        //回合结束
        protected abstract void Exit();
        
        //切换回合
        protected void SwitchTurn()
        {
            //切换回合时间
            BattleSystemModule.SwitchTurnTimeDelegate(() =>
            {
                BattleSystemModule.SwitchTurn();
            });
        }
    }
}