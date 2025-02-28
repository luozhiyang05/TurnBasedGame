using Assets.GameSystem.BattleSystem.Scripts;
using Assets.GameSystem.CardSystem.Scripts.Cmd;
using Framework;

namespace Assets.GameSystem.CardSystem.Scripts.FunctionCard
{
    public class HitForAllCard : BaseCard
    {
        protected override void OnUseCard(AbsUnit self, AbsUnit target)
        {
            this.SendCmd<HitForAllCmd, CardCmdData>(new CardCmdData
            {
                self = self,
                target = target,
                param1 = param1  // param1为额外获取的卡牌数
            });
        }
    }
}