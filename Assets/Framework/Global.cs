using Assets.GameSystem.CToggleGroupSystem;
using Assets.GameSystem.MusicNoteSystem;
using Assets.GameSystem.RedPointSystem;
namespace Framework
{
    public class Global : FrameworkMgr<Global>
    {
        protected override void OnInitModule()
        {
			this.RegisterModule<ICToggleGroupSystemModule>(new CToggleGroupSystemModule());
			this.RegisterModule<IMusicNoteSystemModule>(new MusicNoteSystemModule());
			this.RegisterModule<IRedPointSystemModule>(new RedPointSystemModule());
        }
    }
}