using Assets.GameSystem.BattleSystem.Scripts;
using Assets.GameSystem.BattleSystem.Scripts.Effect;
using Assets.GameSystem.CardSystem.Scripts.Cmd;
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
                defenceEffect = ResMgr.GetInstance().SyncLoad<CardLibrarySo>("卡牌库").GetEffectById(effectId) as DefenceEffect,
                armor = armor
            });
        }
    }
}