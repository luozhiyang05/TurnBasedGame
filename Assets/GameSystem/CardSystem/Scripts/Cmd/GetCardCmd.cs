using Assets.GameSystem.BattleSystem.Scripts;
using Framework;
using GlobalData;
using UnityEngine;

namespace Assets.GameSystem.CardSystem.Scripts.Cmd
{
    public class GetCardCmd : AbsCommand<CardCmdData>
    {
        public override void Do(CardCmdData cardCmdData)
        {
            base.Do(cardCmdData);
            this.GetSystem<ICardSystemModule>().GetCardsFormUseCards(cardCmdData.param1);
            Debug.LogWarning($"{GameManager.GetText(cardCmdData.self.unitName)}获取了{cardCmdData.param1}张卡牌");
        }
    }
}