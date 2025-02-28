using Assets.GameSystem.BattleSystem.Scripts;
using Assets.GameSystem.CardSystem.Scripts.Cmd;
using Framework;

namespace Assets.GameSystem.CardSystem.Scripts.AtkCard
{
    public class RestoreHpCard : BaseCard
    {
        protected override void OnUseCard(AbsUnit self, AbsUnit target)
        {
            this.SendCmd<AddHpCmd, AddHpData>(new AddHpData
            {
                self = self,
                target = target,
                addHp = param1  // param1为回复的血量
            });
        }
    }
}