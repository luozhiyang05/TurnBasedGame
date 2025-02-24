namespace Assets.GameSystem.CardSystem.Scripts
{
    using System.Collections.Generic;
    using UnityEngine;

    [CreateAssetMenu(fileName = "卡牌库", menuName = "CardLibrarySo", order = 0)]
    public class CardLibrarySo : ScriptableObject
    {
        public List<CardGroupSo> cardGroupSos;
        public CardGroupSo GetCardGroupById(int cardGroupId)
        {
            return cardGroupSos.Find(cardGroupSo => cardGroupSo.cardGroupId == cardGroupId);
        }
    }
}