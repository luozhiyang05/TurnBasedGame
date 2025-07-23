using Assets.GameSystem.CToggleGroupSystem;
using Assets.GameSystem.RedPointSystem;
namespace Framework
{
    public class Global : FrameworkMgr<Global>
    {
        protected override void OnInitModule()
        {
			this.RegisterModule<ICToggleGroupSystemModule>(new CToggleGroupSystemModule());
			this.RegisterModule<IRedPointSystemModule>(new RedPointSystemModule());
        }
    }
}