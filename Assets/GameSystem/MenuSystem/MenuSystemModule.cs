using Framework;
using GameSystem.MenuSystem.CharacterChose.Main;
using GameSystem.MenuSystem.LevelChose.Main;
using GameSystem.MenuSystem.Main;

namespace GameSystem.MenuSystem
{
    public interface IMenuSystemModule: IModule
    {
        public void ShowView();
        public void ShowCharacterChoseView();
        public void ShowLevelChoseView();
        public int GetCharacterId();
        public int GetLevelId();
    }

    public class MenuSystemModule : AbsModule, IMenuSystemModule
    {
        private MenuSystemViewCtrl _viewCtrl;
        private CharacterChoseViewCtrl _characterCHoseViewCtrl;
        private LevelChoseViewCtrl _levelChoseViewCtrl;

        protected override void OnInit()
        {
        }

        public void ShowView()
        {
            _viewCtrl ??= new MenuSystemViewCtrl();
            _viewCtrl.ShowView(Tool.UI.EuiLayer.MenuUI);
        }
        public void ShowCharacterChoseView()
        {
            _characterCHoseViewCtrl ??= new CharacterChoseViewCtrl();
            _characterCHoseViewCtrl.ShowView(Tool.UI.EuiLayer.MenuUI);
        }
        public void ShowLevelChoseView()
        {
            _levelChoseViewCtrl ??= new LevelChoseViewCtrl();
            _levelChoseViewCtrl.ShowView(Tool.UI.EuiLayer.MenuUI);
        }
        public int GetCharacterId()
        {
            return (_characterCHoseViewCtrl.GetModel() as CharacterChoseViewModel).GetChoseCharacterId();
        }
        public int GetLevelId()
        {
            return (_levelChoseViewCtrl.GetModel() as LevelChoseViewModel).GetChooseLevelId();
        }

    }
}