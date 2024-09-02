using GameSystem.BattleSystem.Main;
using GameSystem.MVCTemplate;
using Tool.UI;
using UnityEngine;

namespace GameSystem.CardSystem.Main
{
    public class CardSystemViewCtrl : BaseCtrl
    {
        protected override void InitListener()
        {
        }

        protected override void RemoveListener()
        {
            
        }

        protected override void Init()
        {
        }

        public override BaseModel GetModel()
        {
            return Model ??= new CardSystemViewModel();
        }

        public override BaseView GetView()
        {
            return UIManager.GetInstance().LoadViewGo("CardSystemView", EuiLayer.System) as CardSystemView;
        }

        /// <summary>
        /// 展示主要view
        /// </summary>
        public void OnShowView() => OnOpen();
    }
}