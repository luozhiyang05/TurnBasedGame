using GameSystem.BattleSystem;
using GameSystem.CardSystem;
namespace Framework
{
    public class Global : FrameworkMgr<Global>
    {
        protected override void OnInitModule()
        {
			this.RegisterModule<IBattleSystemModule>(new BattleSystemModule());
			this.RegisterModule<ICardSystemModule>(new CardSystemModule());
        }
    }
}