using System.Collections.Generic;
using Assets.GameSystem.BattleSystem.Scripts;
using GameSystem.BattleSystem.Scripts;
using GameSystem.MVCTemplate;
using Tool.UI;
using Tool.Utilities;
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
        public BattleSystemViewCtrl() : base()
        {
        }
        public BattleSystemViewCtrl(params object[] args) : base(args)
        {
        }

        protected override void Init(params object[] args)
        {
           (GetModel() as BattleSystemViewModel).SetLevel(args[0] as Level);
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