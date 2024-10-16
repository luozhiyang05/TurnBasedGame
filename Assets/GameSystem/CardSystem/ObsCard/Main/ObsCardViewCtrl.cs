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
        
        protected override void Init(params object[] args)
        {
            obsCards = args[0] as QArray<BaseCardSo>;
        }

        protected override void InitListener()
        {
        }
        protected override void RemoveListener()
        {
        }
        public override void OnShowComplate()
        {
            //一般做网络请求或给view层传递数据
            ObsCardView obsCardView = View as ObsCardView;
            obsCardView.SetDataSource(obsCards);
            obsCards.Clear();
            obsCards = null;
        }

      
    }
}