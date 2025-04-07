using System;
using Assets.GameSystem.BattleSystem.Scripts;
using Assets.GameSystem.BattleSystem.Scripts.Effect;
using Framework;
using UnityEngine;

namespace Assets.GameSystem.CardSystem.Scripts
{
    [Serializable]
    public abstract class BaseCard : ICanSendCmd,ICanGetSystem
    {
        [Header("卡牌基础信息")]
        public int id;
        public string cardName;
        public string petName;
        public int depletePoint;
        public bool canAutoUse;
        public bool discard;
        [Header("基础属性")]
        public int atk;
        public int armor;
        [Header("额外参数")]
        public int param1;
        public int param2;
        [Header("卡片描述")]
        [TextArea]
        public string cardDesc;
        public int effectId;

        public IMgr Ins => Global.GetInstance();

        public void Init(BaseCardDataPacking baseCardDataPacking)
        {
            id = baseCardDataPacking.id;
            cardName = baseCardDataPacking.cardName;
            depletePoint = baseCardDataPacking.depletePoint;
            canAutoUse = baseCardDataPacking.canAutoUse;
            discard = baseCardDataPacking.discard;
            atk = baseCardDataPacking.atk;
            armor = baseCardDataPacking.armor;
            param1 = baseCardDataPacking.param1;
            param2 = baseCardDataPacking.param2;
            cardDesc = baseCardDataPacking.cardDesc;
            effectId = baseCardDataPacking.effectId;
        }

        public void UseCard(AbsUnit self, AbsUnit target)
        {
            //消耗卡牌点数
            if (self is Player player)
            {
                player.ModifyActPoint(-depletePoint);
            }

            //使用卡牌
            OnUseCard(self, target);
        }

        protected abstract void OnUseCard(AbsUnit self, AbsUnit target);
    }
}