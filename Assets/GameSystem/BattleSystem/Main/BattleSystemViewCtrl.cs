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

        public override BaseModel GetModel() => Model ??= new BattleSystemViewModel();
        public override BaseView GetView() => View;

        public override string GetPrefabPath()=>"BattleSystemView";

        public override void OnShowComplate()
        {
        }
    }
}