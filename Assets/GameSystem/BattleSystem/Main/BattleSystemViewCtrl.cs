using GameSystem.MVCTemplate;
using Tool.UI;
using UnityEngine;

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

        protected override void Init()
        {
        }

        public override BaseModel GetModel()
        {
            return Model ??= new BattleSystemViewModel();
        }

        public override BaseView GetView()
        {
            return  UIManager.GetInstance().LoadViewGo("BattleSystemView", EuiLayer.Mid);
        }

        public override void OnCompleteLoad()
        {
            
        }

        /// <summary>
        /// 展示主要view
        /// </summary>
        public void OnShowView() => OnOpen();

    }
}