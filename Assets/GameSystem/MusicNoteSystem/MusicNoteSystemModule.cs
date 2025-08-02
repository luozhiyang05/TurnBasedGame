using Framework;
namespace Assets.GameSystem.MusicNoteSystem
{
    public interface IMusicNoteSystemModule : IModule
    {

    }

    public class MusicNoteSystemModule : AbsModule, IMusicNoteSystemModule
    {
        protected override void InitModule()
        {
        }
    }
}