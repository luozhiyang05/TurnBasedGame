using GameSystem.BattleSystem.Scripts;
using UnityEngine;

namespace GameSystem.CardSystem.Scripts.DefenceCard
{
    [CreateAssetMenu(menuName = "CardSystem/Def", fileName = "DefCard")]

    public class DefenceCard : DefCardSo
    {
        public override void OnDefenceToSelf(AbsUnit self)
        {
            self.armor += armor;
            Debug.LogWarning($"{self.gameObject.name}增加{armor}点护甲");
        }
    }
}