using Assets.GameSystem.BattleSystem.Scripts;
using Assets.GameSystem.CardSystem.Scripts.Cmd;
using Framework;

namespace Assets.GameSystem.CardSystem.Scripts.AtkCard
{
    public class AttackCard : BaseCard
    {
        protected override void OnUseCard(AbsUnit self, AbsUnit target)
        {
            this.SendCmd<AtkCmd, AtkData>(new AtkData
            {
                self = self,
                target = target,
                atk = atk
            });
        }
    }
}