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

        public override BaseModel GetModel()=>Model ??= new CardSystemViewModel();

        public override BaseView GetView()=>View;

        public override string GetPrefabPath()=>"CardSystemView";
        public override void OnShowComplate()
        {
        }
    }
}