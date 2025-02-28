using Assets.GameSystem.BattleSystem.Scripts;
using Assets.GameSystem.CardSystem.Scripts.Cmd;
using Framework;

namespace Assets.GameSystem.CardSystem.Scripts.FunctionCard
{
    public class GetActPointCard : BaseCard
    {
        protected override void OnUseCard(AbsUnit self, AbsUnit target)
        {
            this.SendCmd<GetActPointCmd, CardCmdData>(new CardCmdData
            {
                self = self,
                target = target,
                param1 = param1  // param1为获取的行动点
            });
        }
    }
}