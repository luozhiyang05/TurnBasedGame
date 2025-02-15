using Assets.GameSystem.BattleSystem.Scripts;
using Framework;
using Tool.Utilities;
using Tool.Utilities.Events;

namespace Assets.GameSystem.CardSystem.Scripts.Cmd
{
    public class UseCardCmd : AbsCommand<CardData>
    {
        public override void Do(CardData cardData)
        {
            base.Do(cardData);

            //对目标使用卡牌
            var absUnit = cardData.target.GetComponent<AbsUnit>();
            cardData.cardSo.UseCard(cardData.user, absUnit);

            //分发事件，成功使用卡牌
            EventsHandle.EventTrigger(EventsNameConst.SUCCESS_USE_CARD,cardData);
        }
    }
}