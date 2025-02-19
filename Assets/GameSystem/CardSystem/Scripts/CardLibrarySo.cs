namespace Assets.GameSystem.CardSystem.Scripts
{
    using System.Collections.Generic;
    using UnityEngine;

    [CreateAssetMenu(fileName = "卡牌库", menuName = "CardLibrarySo", order = 0)]
    public class CardLibrarySo : ScriptableObject
    {
        public List<BaseCardSo> cardSos;

        public BaseCardSo GetCardById(int cardId)
        {
            for (int i = 0; i < cardSos.Count; i++)
            {
                if (cardSos[i].cardId == cardId)
                {
                    return cardSos[i];
                }
            }
            return null;
        }
    }
}