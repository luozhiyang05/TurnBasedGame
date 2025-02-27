using Assets.GameSystem.MenuSystem.CharacterChose.Main;
using Assets.GameSystem.MenuSystem.CharacterChose.Scripts;
using Assets.GameSystem.MenuSystem.LevelChose.Main;
using Assets.GameSystem.MenuSystem.LevelChose.Scripts;
using Assets.GameSystem.MenuSystem.Main;
using Assets.GameSystem.MenuSystem.Setting.Main;
using Framework;

namespace Assets.GameSystem.MenuSystem
{
    public interface IMenuSystemModule : IModule
    {
        public void ShowView();
        public void ShowCharacterChoseView();
        public void ShowLevelChoseView();
        public void ShowSettingView();
        public CharacterData GetNowChoseCharacterData();
        public LevelData GetNowChoseLevelData();
    }

    public class MenuSystemModule : AbsModule, IMenuSystemModule
    {
        private MenuSystemViewCtrl _viewCtrl;
        private CharacterChoseViewCtrl _characterCHoseViewCtrl;
        private LevelChoseViewCtrl _levelChoseViewCtrl;
        private SettingViewCtrl _settingViewCtrl;

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

        public void ShowSettingView()
        {
            _settingViewCtrl ??= new SettingViewCtrl();
            _settingViewCtrl.ShowView(Tool.UI.EuiLayer.MenuUI);
        }
        public CharacterData GetNowChoseCharacterData()
        {
            return (_characterCHoseViewCtrl.GetModel() as CharacterChoseViewModel).GetChoseCharacter();
        }
        public LevelData GetNowChoseLevelData()
        {
            return (_levelChoseViewCtrl.GetModel() as LevelChoseViewModel).GetChooseLevel();
        }

    }
}