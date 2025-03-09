using Assets.GameSystem.FlyTextSystem;
using Framework;
using GlobalData;
using Tips;
namespace Assets.GameSystem.CardSystem.Scripts.Cmd
{

    public class PeekCardsFormTopCmd : AbsCommand<CardCmdData>
    {
        public override void Do(CardCmdData cardCmdData)
        {
            base.Do(cardCmdData);
            // 获取要peek的卡牌数量
            var peekCardCnt = cardCmdData.param1;
            // 获取随机的卡牌索引
            var cardIndexs = this.GetSystem<ICardSystemModule>().PeekCards(peekCardCnt);
            // =0表示牌库没有牌，直接耗费这张卡
            if (cardIndexs.Count == 0)
            {
                this.GetSystem<IFlyTextSystemModule>().FlyText(0, "battle_tip_1009", 1, 0.5f);
            }
            else
            {
                // 打开CardsCheckTips
                TipsModule.CardsCheckTips(cardIndexs, cardCmdData.param2,cardCmdData.action,GameManager.GetText("card_tip_1002"));
            }
        }
    }
}