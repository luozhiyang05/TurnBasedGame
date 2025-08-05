using GameSystem.MVCTemplate;
using Tool.UI;

namespace Assets.GameSystem.MusicNoteSystem.Main
{
    public enum EMusicNoteFloatView
    {
        MusicNoteFloatView
    }
    public class MusicNoteFloatViewCtrl : BaseCtrl
    {
        public override BaseModel GetModel() => Model ??= new MusicData();
        public MusicNoteFloatViewCtrl(string moduleName, params object[] args) : base(moduleName, args)
        {

        }
        protected override void Init(params object[] args)
        {

        }
        protected override void InitListener()
        {
        }

        public void OpenView(params object[] args)
        {
            SetOpenViewName(EMusicNoteFloatView.MusicNoteFloatView.ToString());
            ShowView(EuiLayer.GameUI, args);
        }
    }
}