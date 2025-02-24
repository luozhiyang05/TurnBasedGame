namespace Assets.GameSystem.CardSystem.Scripts
{
    using System.Collections.Generic;
    using UnityEngine;

    [CreateAssetMenu(fileName = "卡牌组", menuName = "CardGroupSo", order = 0)]
    public class CardGroupSo : ScriptableObject
    {
        public int cardGroupId;
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
        public List<BaseCardSo> GetBaseCardSo() => new(cardSos);
    }
}