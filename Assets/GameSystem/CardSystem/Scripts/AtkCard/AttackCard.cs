using System;
using GameSystem.BattleSystem.Scripts;
using UnityEngine;

namespace GameSystem.CardSystem.Scripts.AtkCard
{
    [CreateAssetMenu(menuName = "CardSystem/Atk", fileName = "AtkCard")]
    public class AttackCard : AtkCardSo
    {
        public override void OnAttackToCard(AbsUnit self, AbsUnit target)
        {
            var reduceHp = target.armor - atk;
            if (reduceHp < 0)
            {
                var targetNowHp = target.nowHp - Mathf.Abs(reduceHp);
                target.nowHp = targetNowHp;
                target.armor = 0;
                Debug.LogWarning(
                    $"{self.gameObject.name}对{target.gameObject.name}{Mathf.Abs(reduceHp)}点伤害,{target.gameObject.name}目前血量为{target.nowHp}/{target.maxHp},护甲为{target.armor}");
            }
            else
            {
                target.armor -= atk;
                Debug.LogWarning($"{self.gameObject.name}对{target.gameObject.name}造成{atk}点护甲伤害,目前{target.gameObject.name}护甲为{target.armor}");
            }
        }
    }
}