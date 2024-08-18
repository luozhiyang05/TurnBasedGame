using GameSystem.BattleSystem.Scripts;
using UnityEngine;

namespace GameSystem.CardSystem.Scripts
{
    public abstract class AtkCardSo : BaseCardSo
    {
        public override void AttackToTarget(AbsUnit self, AbsUnit target)
        {
            OnAttackToCard(self,target);
        }

        public abstract void OnAttackToCard(AbsUnit self, AbsUnit target);
    }
}