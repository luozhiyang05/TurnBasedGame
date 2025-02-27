using Assets.GameSystem.BattleSystem;
using Assets.GameSystem.CardSystem;
using Assets.GameSystem.EffectsSystem;
using Assets.GameSystem.MenuSystem;
using Assets.GameSystem.SkillSystem;
namespace Framework
{
    public class Global : FrameworkMgr<Global>
    {
        protected override void OnInitModule()
        {
			this.RegisterModule<IBattleSystemModule>(new BattleSystemModule());
			this.RegisterModule<ICardSystemModule>(new CardSystemModule());
			this.RegisterModule<IEffectsSystemModule>(new EffectsSystemModule());
			this.RegisterModule<IMenuSystemModule>(new MenuSystemModule());
			this.RegisterModule<ISkillSystemModule>(new SkillSystemModule());
        }
    }
}