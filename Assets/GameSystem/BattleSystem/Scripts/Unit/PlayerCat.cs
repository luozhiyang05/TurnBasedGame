using UnityEngine;

namespace GameSystem.BattleSystem.Scripts.Unit
{
    public class PlayerCat : Player
    {
        protected override void OnStartTurnSettle()
        {
            Debug.Log("玩家回合开始时结算逻辑");
        }

        protected override void OnAction()
        {
            Debug.Log("玩家具体攻击逻辑");
        }

        protected override void SettleTurn()
        {
            Debug.Log("玩家回合结束结算逻辑");
        }
    }
}