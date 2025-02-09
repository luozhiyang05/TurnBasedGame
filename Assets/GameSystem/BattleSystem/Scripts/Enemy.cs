using System;
using Framework;
using Tool.Mono;
using UnityEngine;

namespace GameSystem.BattleSystem.Scripts
{
    public abstract class Enemy : AbsUnit, ICanGetSystem
    {
        
        /// <summary>
        /// 回合开始时结算逻辑
        /// </summary>
        protected abstract void OnStartRoundSettle();
        public override void StartRoundSettle()
        {
            base.StartRoundSettle();    //结算单位身上的效果
            
            OnStartRoundSettle();       //具体重写的 回合开始时 逻辑

            AfterStartRoundSettle();    //弹幕时间，结束后进入 行动逻辑
        }
        
        /// <summary>
        /// 行动逻辑
        /// </summary>
        protected abstract void OnAction();
        public override void Action()
        {
            OnAction();     //具体重写的 单位行动 逻辑

            AfterAction();  //弹幕时间，结束后进入 结算回合 逻辑
        }

        
        /// <summary>
        /// 结算回合逻辑
        /// </summary>
        protected abstract void SettleRound();
        protected override void ExitRound()
        {
            base.ExitRound();   //结算单位身上的效果

            SettleRound();      //具体重写的 结算回合 逻辑
            
            SwitchRound();      //回合切换
        }



        public IMgr Ins => Global.GetInstance();
    }
}