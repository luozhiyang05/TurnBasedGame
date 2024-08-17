using GameSystem.BattleSystem;
namespace Framework
{
    public class Global : FrameworkMgr<Global>
    {
        protected override void OnInitModule()
        {
			this.RegisterModule<IBattleSystemModule>(new BattleSystemModule());
        }
    }
}