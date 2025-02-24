using Assets.GameSystem.BattleSystem.Scripts;
using Assets.GameSystem.BattleSystem.Scripts.Effect;
using Assets.GameSystem.CardSystem.Scripts.Cmd;
using Framework;
using UnityEngine;

namespace Assets.GameSystem.CardSystem.Scripts.DefenceCard
{
    public class DefenceCard : BaseCard
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