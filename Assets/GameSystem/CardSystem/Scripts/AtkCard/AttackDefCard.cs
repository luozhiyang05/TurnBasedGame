using Assets.GameSystem.BattleSystem.Scripts;
using Assets.GameSystem.BattleSystem.Scripts.Effect;
using Assets.GameSystem.CardSystem.Scripts.Cmd;
using Framework;
using Tool.ResourceMgr;
using UnityEngine;

namespace Assets.GameSystem.CardSystem.Scripts.AtkCard
{
    public class AttackDefCard : BaseCard
    {

        protected override void OnUseCard(AbsUnit self, AbsUnit target)
        {
            //叠甲
            this.SendCmd<DefCmd, DefData>(new DefData
            {
                self = self,
                target = self,
                // defenceEffect = ResMgr.GetInstance().SyncLoad<CardLibrarySo>("卡牌库").GetEffectById(effectId) as DefenceEffect,
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