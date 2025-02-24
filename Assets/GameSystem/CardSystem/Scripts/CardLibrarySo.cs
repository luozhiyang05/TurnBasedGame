namespace Assets.GameSystem.CardSystem.Scripts
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using Assets.GameSystem.BattleSystem.Scripts.Effect;
    using Assets.GameSystem.CardSystem.Scripts.AtkCard;
    using Assets.GameSystem.CardSystem.Scripts.DefCard;
    using Tool.Utilities.CSV;
    using UnityEngine;

    [Serializable]
    public class BaseCardDataPacking
    {
        public BaseCard baseCard;
        [Header("卡牌基础信息")]
        public int id;
        public string cardName;
        public string petName;
        public int depletePoint;
        public bool canAutoUse = false;
        [Header("基础属性")]
        public int atk;
        public int armor;
        [Header("卡片描述")]
        [TextArea]
        public string cardDesc;
        public int effectId;
        public void SetUpData()
        {
            baseCard.id = id;
            baseCard.cardName = cardName;
            baseCard.petName = petName;
            baseCard.depletePoint = depletePoint;
            baseCard.canAutoUse = canAutoUse;
            baseCard.atk = atk;
            baseCard.armor = armor;
            baseCard.cardDesc = cardDesc;
            baseCard.effectId = effectId;
        }
    }
     [Serializable]
    public class BaseEffectDataPacking{
        public int id;
        public string effName;
        public int maxRoundCnt; //最大回合数
        public BaseEffect baseEffect;
        public void SetUpData()
        {
            baseEffect.id = id;
            baseEffect.maxRoundCnt = maxRoundCnt;
        }
    }

    [CreateAssetMenu(fileName = "卡牌库", menuName = "CardLibrarySo", order = 0)]
    public class CardLibrarySo : ScriptableObject
    {
        public TextAsset cardAsset;
        public TextAsset effAsset;
        public List<BaseCardDataPacking> baseCardDataPackings = new List<BaseCardDataPacking>();
        public List<BaseEffectDataPacking> baseEffectDataPackings = new List<BaseEffectDataPacking>();
        private void OnValidate()
        {
            if (effAsset != null)
            {
                baseEffectDataPackings.Clear();
                CsvKit.Read<BaseEffectDataPacking>(effAsset, BindingFlags.Public | BindingFlags.Instance, value =>
               {
                   AddCardEffectById(value);
                   baseEffectDataPackings.Add(value);
               });
            }

            if (cardAsset != null)
            {
                baseCardDataPackings.Clear();
                CsvKit.Read<BaseCardDataPacking>(cardAsset, BindingFlags.Public | BindingFlags.Instance, value =>
               {
                   AddCardCSharpById(value);
                   baseCardDataPackings.Add(value);
               });
            }
        }
        public BaseCard GetCardById(int cardId)
        {
            for (int i = 0; i < baseCardDataPackings.Count; i++)
            {
                if (baseCardDataPackings[i].id == cardId)
                {
                    return baseCardDataPackings[i].baseCard;
                }
            }
            return null;
        }
        public BaseEffect GetEffectById(int effectId)
        {
            for (int i = 0; i < baseEffectDataPackings.Count; i++)
            {
                if (baseEffectDataPackings[i].id == effectId)
                {
                    return baseEffectDataPackings[i].baseEffect;
                }
            }
            return null;
        }
        public void AddCardCSharpById(BaseCardDataPacking baseCardDataPacking)
        {
            switch (baseCardDataPacking.id)
            {
                case 1:
                    baseCardDataPacking.baseCard = new AttackCard();
                    break;
                case 2:
                    baseCardDataPacking.baseCard = new DefenceCard();
                    break;
                case 3:
                    baseCardDataPacking.baseCard = new AttackDefCard();
                    break;
            }
            baseCardDataPacking.SetUpData();
        }

        public void AddCardEffectById(BaseEffectDataPacking baseEffectDataPacking)
        {
            switch (baseEffectDataPacking.id)
            {
                case 0: return;
                case 1:
                    baseEffectDataPacking.baseEffect = new DefenceEffect();
                    break;
                case 2:
                    baseEffectDataPacking.baseEffect = new DefenceEffect();
                    break;
            }
            baseEffectDataPacking.SetUpData();
        }
    }
}