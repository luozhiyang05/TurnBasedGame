using Assets.GameSystem.BattleSystem.Scripts;
using Assets.GameSystem.BattleSystem.Scripts.Effect;
using Assets.GameSystem.CardSystem.Scripts.Cmd;
using Framework;
using UnityEngine;

namespace Assets.GameSystem.CardSystem.Scripts.AtkCard
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