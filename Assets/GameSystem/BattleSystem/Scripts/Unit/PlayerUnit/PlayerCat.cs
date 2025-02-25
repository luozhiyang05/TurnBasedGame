using UnityEngine;
namespace Assets.GameSystem.BattleSystem.Scripts.Unit.PlayerUnit
{
    public class PlayerCat : Player
    {
        protected override void OnStartRoundSettle()
        {
            Debug.Log("玩家回合开始时结算逻辑");
        }

        protected override void SettleRound()
        {
            Debug.Log("玩家回合结束结算逻辑");
        }
    }
}