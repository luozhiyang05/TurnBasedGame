using GameSystem.BattleSystem.Scripts;
using UnityEngine;

namespace GameSystem.CardSystem.Scripts
{
    public abstract class BaseCardSo : ScriptableObject
    {
        [Header("卡牌基础信息")]
        public string cardName;
        public int depletePoint;
        public bool canAutoUse = false;
        [Header("基础属性")]
        public int atk;
        public int armor;
        [Header("卡片描述")]
        [TextArea]
        public string cardDesc;

        public void UseCard(AbsUnit self, AbsUnit target)
        {
            //消耗卡牌点数
            if (self is Player player)
            {
                player.ModifyActPoint(-depletePoint);
            }
            
            //使用卡牌
            OnUseCard(self,target);
        }

        protected abstract void OnUseCard(AbsUnit self, AbsUnit target);
    }
}