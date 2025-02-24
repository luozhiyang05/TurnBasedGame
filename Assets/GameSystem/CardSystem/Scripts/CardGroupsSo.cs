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
        public List<CardGroupData> cardSos;

        private void OnValidate()
        {
            if (textAsset == null) return;
            cardSos.Clear();
            CsvKit.Read<CardGroupData>(textAsset, BindingFlags.Public | BindingFlags.Instance, value =>
            {
                cardSos.Add(value);
            });
        }
    }
}