using System.Collections.Generic;
using Framework;
using GameSystem.BattleSystem.Scripts;
using GameSystem.BattleSystem.Scripts.Effect;
using GameSystem.CardSystem.Scripts.Cmd;
using UnityEngine;

namespace GameSystem.CardSystem.Scripts.DefenceCard
{
    [CreateAssetMenu(menuName = "CardSystem/AtkDef", fileName = "AttackDefCard")]

    public class AttackDefCard : BaseCardSo
    {
        public DefenceEffect defenceEff;

        protected override void OnUseCard(AbsUnit self, AbsUnit target)
        {
            //叠甲
            this.SendCmd<DefCmd, DefData>(new DefData
            {
                self = self,
                target = self,
                defenceEffect = defenceEff,
                armor = armor
            });

            //攻击
            this.SendCmd<AtkCmd, AtkData>(new AtkData
            {
                self = self,
                target = target,
                atk = atk
            });
        }
    }
}