using Assets.GameSystem.BattleSystem.Scripts;
using Assets.GameSystem.CardSystem.Scripts.Cmd;
using Assets.GameSystem.EffectsSystem;
using Framework;

namespace Assets.GameSystem.CardSystem.Scripts.FunctionCard
{
    public class WeakCard : BaseCard
    {
        protected override void OnUseCard(AbsUnit self, AbsUnit target)
        {
            this.SendCmd<WeakCmd, CardCmdData>(new CardCmdData
            {
                self = self,
                target = target,
                baseEffect = this.GetSystem<IEffectsSystemModule>().GetBaseEffectById(effectId),
                param1 = param1
            });
        }
    }
}