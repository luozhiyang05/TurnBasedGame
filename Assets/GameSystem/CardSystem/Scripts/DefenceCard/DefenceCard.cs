using System.Collections.Generic;
using GameSystem.BattleSystem.Scripts;
using GameSystem.BattleSystem.Scripts.Effect;
using UnityEngine;

namespace GameSystem.CardSystem.Scripts.DefenceCard
{
    [CreateAssetMenu(menuName = "CardSystem/Def", fileName = "DefCard")]

    public class DefenceCard : BaseCardSo
    {
        public DefenceEffect defenceEff;
        public override void UseCard(AbsUnit self, AbsUnit target)
        {
            defenceEff.Init(self,target,armor);
            self.AddEffect(defenceEff);
            
            target.armor += armor;
            Debug.LogWarning($"{self.gameObject.name}对{target.gameObject.name}增加{armor}点护甲");
        }
    }
}