using Assets.GameSystem.BattleSystem.Scripts;
using Assets.GameSystem.BattleSystem.Scripts.Effect;
using Assets.GameSystem.CardSystem.Scripts.Cmd;
using Assets.GameSystem.EffectsSystem;
using Framework;
using Tool.ResourceMgr;
using UnityEngine;

namespace Assets.GameSystem.CardSystem.Scripts.DefCard
{
    public class DefenceCard : BaseCard
    {

        protected override void OnUseCard(AbsUnit self, AbsUnit target)
        {
            this.SendCmd<DefCmd, DefData>(new DefData
            {
                self = self,
                target = target,
                maxRoundCnt = param1,
                defenceEffect = this.GetSystem<IEffectsSystemModule>().GetBaseEffectById(effectId) as DefenceEffect,
                armor = armor
            });
        }
    }
}