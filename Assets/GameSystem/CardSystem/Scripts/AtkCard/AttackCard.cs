using Assets.GameSystem.BattleSystem.Scripts;
using Assets.GameSystem.CardSystem.Scripts.Cmd;
using Framework;
using UnityEngine;

namespace Assets.GameSystem.CardSystem.Scripts.AtkCard
{
    [CreateAssetMenu(menuName = "CardSystem/Atk", fileName = "AtkCard")]
    public class AttackCard : BaseCardSo
    {
        protected override void OnUseCard(AbsUnit self, AbsUnit target)
        {
            this.SendCmd<AtkCmd, AtkData>(new AtkData
            {
                self = self,
                target = target,
                atk = atk
            });
        }
    }
}