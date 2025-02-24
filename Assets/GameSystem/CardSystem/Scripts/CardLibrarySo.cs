namespace Assets.GameSystem.CardSystem.Scripts
{
    using System.Collections.Generic;
    using System.Reflection;
    using Tool.Utilities.CSV;
    using UnityEngine;

    [CreateAssetMenu(fileName = "卡牌库", menuName = "CardLibrarySo", order = 0)]
    public class CardLibrarySo : ScriptableObject
    {
        public TextAsset readAsset;
        public List<BaseCard> BaseCards;
        private void OnValidate()
        {
            if (readAsset == null) return;
            BaseCards.Clear();
            CsvKit.Read<BaseCard>(readAsset, BindingFlags.Public | BindingFlags.Instance, value =>
           {
               BaseCards.Add(value);
           });
        }
    }
}