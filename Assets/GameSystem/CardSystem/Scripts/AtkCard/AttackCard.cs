using GameSystem.BattleSystem.Scripts;
using UnityEngine;

namespace GameSystem.CardSystem.Scripts.AtkCard
{
    [CreateAssetMenu(menuName = "CardSystem/Atk",fileName = "AtkCard")]
    public class AttackCard : AtkCardSo
    {
        public override void OnAttackToCard(AbsUnit self, AbsUnit target)
        {
            var reduceHp = target.armor - atk;
            var targetNowHp = target.nowHp - reduceHp;
            target.nowHp = targetNowHp;
        }
    }
}