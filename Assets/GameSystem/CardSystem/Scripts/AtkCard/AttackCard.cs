using System;
using GameSystem.BattleSystem.Scripts;
using UnityEngine;

namespace GameSystem.CardSystem.Scripts.AtkCard
{
    [CreateAssetMenu(menuName = "CardSystem/Atk", fileName = "AtkCard")]
    public class AttackCard : BaseCardSo
    {
        protected override void OnUseCard(AbsUnit self,AbsUnit target)
        {
            var reduceHp = target.armor.Value - atk;
            if (reduceHp < 0)
            {
                var targetNowHp = target.nowHp.Value - Mathf.Abs(reduceHp);
                target.nowHp.Value = targetNowHp;
                target.armor.Value = 0;
                Debug.LogWarning(
                    $"{self.gameObject.name}对{target.gameObject.name}{Mathf.Abs(reduceHp)}点伤害,{target.gameObject.name}目前血量为{target.nowHp}/{target.maxHp},护甲为{target.armor}");
            }
            else
            {
                target.armor.Value -= atk;
                Debug.LogWarning($"{self.gameObject.name}对{target.gameObject.name}造成{atk}点护甲伤害,目前{target.gameObject.name}护甲为{target.armor}");
            }
        }
    }
}