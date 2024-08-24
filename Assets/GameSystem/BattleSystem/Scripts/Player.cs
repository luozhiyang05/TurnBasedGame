using Framework;
using GameSystem.CardSystem.Scripts;
using GlobalData;
using Tool.Mono;
using UnityEngine;

namespace GameSystem.BattleSystem.Scripts
{
    public abstract class Player : AbsUnit, ICanGetSystem
    {
        public bool canAction = false;
        
        /// <summary>
        /// 回合开始时结算逻辑
        /// </summary>
        protected abstract void OnStartRoundSettle();
        public override void StartRoundSettle()
        {
            base.StartRoundSettle();
            
            OnStartRoundSettle();
            
            canAction = true;
        }


        /// <summary>
        /// 行动逻辑
        /// </summary>
        protected abstract void OnAction(BaseCardSo card, AbsUnit target);

        public override void Action()
        {
            Debug.Log("玩家开始出牌");
            canAction = true;
        }

        /// <summary>
        /// 玩家使用卡牌
        /// </summary>
        /// <param name="card"></param>
        /// <param name="target"></param>
        public void UseCard(BaseCardSo card, AbsUnit target)
        {
            if (!canAction) return;
            
            OnAction(card, target);
        }

        public void EndRound() => AfterAction();


        /// <summary>
        /// 结算回合逻辑
        /// </summary>
        protected abstract void SettleRound();
        protected override void ExitRound()
        {
            base.ExitRound();
            
            SettleRound();
            
            SwitchRound();
            
            canAction = false;
        }


        public IMgr Ins => Global.GetInstance();
    }
}