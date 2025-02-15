using GameSystem.MVCTemplate;

namespace Assets.GameSystem.CardSystem.Main
{
    public class CardSystemViewCtrl : BaseCtrl
    {
        protected override void InitListener()
        {
        }

        protected override void RemoveListener()
        {

        }

        public CardSystemViewCtrl() : base()
        {
        }
        public CardSystemViewCtrl(params object[] args) : base(args)
        {
        }

        protected override void Init(params object[] args)
        {

        }

        public override BaseModel GetModel() => Model ??= new CardSystemViewModel();

        public override BaseView GetView() => View;

        public override string GetPrefabPath() => "CardSystemView";
        public override void OnShowComplate(params object[] args)
        {
        }

        public override void OnBeforeShow(params object[] args)
        {

        }
    }
}