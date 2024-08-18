using UnityEngine;

namespace GameSystem.BattleSystem.Scripts.Unit
{
    public class EnemyStore : Enemy
    {
        protected override void OnStartTurnSettle()
        {
            Debug.Log($"{gameObject.name}回合开始时结算逻辑");
        }

        protected override void OnAction()
        {
            Debug.Log($"{gameObject.name}具体攻击逻辑");
        }

        protected override void SettleTurn()
        {
            Debug.Log($"{gameObject.name}回合结束结算逻辑");
        }
    }
}