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
        [Header("额外参数")]
        public int param1;
        [Header("卡片描述")]
        [TextArea]
        public string cardDesc;
        public int effectId;
    }
    [Serializable]
    public class BaseEffectData
    {
        public int id;
        public string effName;
        public string effDesc;
        public bool isDieEff;
    }

    [CreateAssetMenu(fileName = "卡牌库", menuName = "CardLibrarySo", order = 0)]
    public class CardLibrarySo : ScriptableObject
    {
        public TextAsset cardAsset;
        public List<BaseCardDataPacking> baseCardDataPackings = new List<BaseCardDataPacking>();
        private void OnValidate()
        {
            if (cardAsset != null)
            {
                baseCardDataPackings.Clear();
                CsvKit.Read<BaseCardDataPacking>(cardAsset, BindingFlags.Public | BindingFlags.Instance, value =>
               {
                   baseCardDataPackings.Add(value);
               });
            }
        }

        /// <summary>
        /// 根据卡牌Id获取对应卡牌实例
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public BaseCard GetCardById(int id)
        {
            var baseCardDataPacking = baseCardDataPackings.Find(value => value.id == id) ?? throw new Exception("找不到对应的卡牌");
            switch (baseCardDataPacking.id)
            {
                case 1:
                    var atkCard = new AttackCard();
                    atkCard.Init(baseCardDataPacking);
                    return atkCard;
                case 2:
                    var defCard = new DefenceCard();
                    defCard.Init(baseCardDataPacking);
                    return defCard;
                case 3:
                    var atkDefCard = new AttackDefCard();
                    atkDefCard.Init(baseCardDataPacking);
                    return atkDefCard;
                default:
                    throw new Exception("找不到对应的卡牌");
            }
        }
    }
}