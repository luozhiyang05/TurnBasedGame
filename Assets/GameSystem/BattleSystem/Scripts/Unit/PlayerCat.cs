using Framework;
using GameSystem.CardSystem;
using GameSystem.CardSystem.Scripts;
using UnityEngine;

namespace GameSystem.BattleSystem.Scripts.Unit
{
    public class PlayerCat : Player
    {
        protected override void OnStartRoundSettle()
        {
            Debug.Log("玩家回合开始时结算逻辑");
        }

        protected override void OnUseCard(BaseCardSo card, AbsUnit target)
        {
            this.GetSystem<ICardSystemModule>().UnitUseCard(card,this,target);
        }
        

        protected override void SettleRound()
        {
            Debug.Log("玩家回合结束结算逻辑");
        }
    }
}