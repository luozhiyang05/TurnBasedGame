using Assets.GameSystem.BattleSystem;
using Assets.GameSystem.CardSystem;
using Assets.GameSystem.EffectsSystem;
using Assets.GameSystem.FlyTextSystem;
using Assets.GameSystem.MenuSystem;
using Assets.GameSystem.MotionSystem;
using Assets.GameSystem.SkillSystem;
using GlobalData;
namespace Framework
{
    public class Global : FrameworkMgr<Global>
    {
        protected override void OnInitModule()
        {
            GameManager.InitGlobalData();

			this.RegisterModule<IBattleSystemModule>(new BattleSystemModule());
			this.RegisterModule<ICardSystemModule>(new CardSystemModule());
			this.RegisterModule<IEffectsSystemModule>(new EffectsSystemModule());
			this.RegisterModule<IMenuSystemModule>(new MenuSystemModule());
			this.RegisterModule<ISkillSystemModule>(new SkillSystemModule());
			this.RegisterModule<IFlyTextSystemModule>(new FlyTextSystemModule());
            this.RegisterModule<IMotionSystemModule>(new MotionSystemModule());
        }
    }
}