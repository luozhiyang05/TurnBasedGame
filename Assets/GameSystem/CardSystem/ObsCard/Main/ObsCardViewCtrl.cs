using GameSystem.CardSystem.Scripts;
using GameSystem.MVCTemplate;
using Tool.UI;
using Tool.Utilities;
using UnityEngine;
using UnityEngine.Events;

namespace GameSystem.CardSystem.ObsCard.Main
{
    public class ObsCardViewCtrl : BaseCtrl
    {
        public override string GetPrefabPath() => "ObsCardView";
        public override BaseModel GetModel() => Model ??= new ObsCardViewModel();
        public override BaseView GetView() => View;
        public ObsCardViewCtrl() : base() { }
        public ObsCardViewCtrl(params object[] args) : base(args)
        {
            
        }
        
        private QArray<BaseCardSo> obsCards;
        private bool isUseCards;
        
        protected override void Init(params object[] args)
        {
            obsCards = args[0] as QArray<BaseCardSo>;
            isUseCards = (bool)args[1];
        }

        protected override void InitListener()
        {
        }
        protected override void RemoveListener()
        {
        }

        public override void OnBeforeShow(params object[] args)
        {
            ObsCardView obsCardView = View as ObsCardView;
            obsCardView.SetDataSource(obsCards.Clone(),isUseCards);
            obsCards.Clear();
            obsCards = null;
        }

        public override void OnShowComplate(params object[] args)
        {

        }
    }
}