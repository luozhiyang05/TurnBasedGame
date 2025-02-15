using Assets.GameSystem.BattleSystem;
using Assets.GameSystem.CardSystem;
using Assets.GameSystem.MenuSystem;
namespace Framework
{
    public class Global : FrameworkMgr<Global>
    {
        protected override void OnInitModule()
        {
			this.RegisterModule<IBattleSystemModule>(new BattleSystemModule());
			this.RegisterModule<ICardSystemModule>(new CardSystemModule());
			this.RegisterModule<IMenuSystemModule>(new MenuSystemModule());
        }
    }
}