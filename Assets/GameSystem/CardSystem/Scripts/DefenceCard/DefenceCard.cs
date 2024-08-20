using GameSystem.BattleSystem.Scripts;
using UnityEngine;

namespace GameSystem.CardSystem.Scripts.DefenceCard
{
    [CreateAssetMenu(menuName = "CardSystem/Def", fileName = "DefCard")]

    public class DefenceCard : BaseCardSo
    {
        public override void UseCard(AbsUnit self, AbsUnit target)
        {
            target.armor += armor;
            Debug.LogWarning($"{self.gameObject.name}对{target.gameObject.name}增加{armor}点护甲");
        }
    }
}