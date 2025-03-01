using Framework;
using Tips;
using Tool.Utilities;
using UnityEngine;
using UnityEngine.Events;
namespace Assets.GameSystem.CardSystem.Scripts.Cmd
{

    public class GetCardsFormPeekCardsCmd : AbsCommand<CardCmdData>
    {
        public override void Do(CardCmdData cardCmdData)
        {
            base.Do(cardCmdData);
            this.SendCmd<PeekCardsFormTopCmd,CardCmdData>(new CardCmdData(){
                self = cardCmdData.self,
                target = cardCmdData.target,
                param1 = cardCmdData.param1,
                param2 = cardCmdData.param2,
                action = GetCards
            });
        }

        private void GetCards(QArray<int> chooseCardIndex)
        {
            var cards = new QArray<BaseCard>();
            var cardSystemModule = this.GetSystem<ICardSystemModule>();
            for (int i = 0; i < chooseCardIndex.Count; i++)
            {
                Debug.Log(chooseCardIndex[i] - 1);
                cards.Add(cardSystemModule.GetCardByIndexFormUseCards(chooseCardIndex[i] - 1));
            }
            // 将选中的卡牌索引，删除，表示已经获取到该卡牌
            for (int i = 0; i < chooseCardIndex.Count; i++)
            {
                cardSystemModule.DeleteCardIndex(chooseCardIndex[i]);
            }
            // 获取选中的卡牌
            cardSystemModule.GetCardsToHeadCardsFormUseCards(cards);
        }
    }
}