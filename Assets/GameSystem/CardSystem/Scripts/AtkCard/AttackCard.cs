using System;
using Framework;
using GameSystem.BattleSystem.Scripts;
using GameSystem.CardSystem.Scripts.Cmd;
using UnityEngine;

namespace GameSystem.CardSystem.Scripts.AtkCard
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