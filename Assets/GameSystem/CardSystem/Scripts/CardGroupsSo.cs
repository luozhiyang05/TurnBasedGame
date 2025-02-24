namespace Assets.GameSystem.CardSystem.Scripts
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using Tool.Utilities.CSV;
    using UnityEngine;

     [Serializable]
    public class CardGroupData
    {
        public int id;
        public string cardIds;
    }

    [CreateAssetMenu(fileName = "卡牌组", menuName = "CardGroupSo", order = 0)]
    public class CardGroupsSo : ScriptableObject
    {
        public TextAsset textAsset;
        public List<CardGroupData> cardGroups;

        private void OnValidate()
        {
            if (textAsset == null) return;
            cardGroups.Clear();
            CsvKit.Read<CardGroupData>(textAsset, BindingFlags.Public | BindingFlags.Instance, value =>
            {
                cardGroups.Add(value);
            });
        }

        public List<int> GetCardsId(int cardGroupId)
        {
            var cardGroupData = cardGroups.Find(value=>value.id==cardGroupId);
            if(cardGroupData!=null)
            {
                var ids = new List<int>();
                foreach (var id in cardGroupData.cardIds.Split('-'))
                {
                    ids.Add(int.Parse(id));
                }
                return ids;
            }
            throw new Exception("卡牌组不存在");
        }
    }
}