using Assets.GameSystem.MenuSystem.CharacterChose.Scripts;
using Assets.GameSystem.MenuSystem.LevelChose.Scripts;
using GameSystem.MVCTemplate;

namespace GameSystem.BattleSystem.Main
{
    public class BattleSystemViewCtrl : BaseCtrl
    {
        protected override void InitListener()
        {
        }

        protected override void RemoveListener()
        {

        }
        public BattleSystemViewCtrl() : base()
        {
        }
        public BattleSystemViewCtrl(params object[] args) : base(args)
        {
        }

        protected override void Init(params object[] args)
        {
           (GetModel() as BattleSystemViewModel).SetBattleData(args[0] as CharacterData,args[1] as LevelData);
        }

        public override BaseModel GetModel() => Model ??= new BattleSystemViewModel();
        public override BaseView GetView() => View;

        public override string GetPrefabPath()=>"BattleSystemView";

        public override void OnShowComplate()
        {
        }

        public override void OnBeforeShow()
        {

        }
    }
}