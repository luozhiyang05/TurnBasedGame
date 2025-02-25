using Assets.GameSystem.CardSystem.Scripts.Cmd;
using Framework;
using UnityEngine;

namespace Assets.GameSystem.BattleSystem.Scripts.Unit
{
    public class EnemyStore : Enemy
    {
        protected override void OnStartRoundSettle()
        {
            Debug.Log($"{gameObject.name}回合开始时结算逻辑");
        }

        protected override void OnAction()
        {
            Debug.Log($"{gameObject.name}具体攻击逻辑");
            this.SendCmd<AtkCmd, AtkData>(new AtkData
            {
                self = this,
                target = _battleSystemModule.GetPlayerUnit(),
                atk = atk.Value
            });
        }

        protected override void SettleRound()
        {
            Debug.Log($"{gameObject.name}回合结束结算逻辑");
        }
    }
}