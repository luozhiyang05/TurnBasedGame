using System.Collections.Generic;
using Framework;
using GameSystem.BattleSystem.Scripts;
using GameSystem.BattleSystem.Scripts.Effect;
using GameSystem.CardSystem.Scripts.Cmd;
using UnityEngine;

namespace GameSystem.CardSystem.Scripts.DefenceCard
{
    [CreateAssetMenu(menuName = "CardSystem/Def", fileName = "DefCard")]

    public class DefenceCard : BaseCardSo
    {
        public DefenceEffect defenceEff;

        protected override void OnUseCard(AbsUnit self, AbsUnit target)
        {
            this.SendCmd<DefCmd, DefData>(new DefData
            {
                self = self,
                target = target,
                defenceEffect = defenceEff,
                armor = armor
            });
        }
    }
}