using Framework;
using GameSystem.CardSystem;
using GameSystem.CardSystem.Scripts;
using GlobalData;
using Tool.Mono;
using UnityEngine;

namespace GameSystem.BattleSystem.Scripts
{
    public abstract class Player : AbsUnit, ICanGetSystem
    {
        public int maxActPoint;
        public int nowActPoint;

        public bool canAction = false;

        /// <summary>
        /// 回合开始时结算逻辑
        /// </summary>
        protected abstract void OnStartRoundSettle();

        public override void StartRoundSettle()
        {
            nowActPoint = maxActPoint;

            base.StartRoundSettle();

            OnStartRoundSettle();
            
            this.GetSystem<ICardSystemModule>().UpdateHeadCardInSr();

            canAction = true;
        }

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
            if (canAction == false) return;

            if (nowActPoint - card.depletePoint < 0)
            {
                return;
            }

            OnUseCard(card, target);
        }

        /// <summary>
        /// 行动逻辑
        /// </summary>
        protected abstract void OnUseCard(BaseCardSo card, AbsUnit target);

        public void EndRound() => AfterAction();


        /// <summary>
        /// 结算回合逻辑
        /// </summary>
        protected abstract void SettleRound();

        protected override void ExitRound()
        {
            base.ExitRound();

            SettleRound();
            
            this.GetSystem<ICardSystemModule>().UpdateHeadCardInEr();

            SwitchRound();

            canAction = false;
        }

        /// <summary>
        /// 修改行动点
        /// </summary>
        /// <param name="mPoint"></param>
        /// <returns></returns>
        public void ModifyActPoint(int mPoint)
        {
            nowActPoint += mPoint;
        }


        public IMgr Ins => Global.GetInstance();
    }
}