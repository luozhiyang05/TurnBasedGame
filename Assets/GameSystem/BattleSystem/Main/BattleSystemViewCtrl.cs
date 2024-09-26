using System.Collections.Generic;
using GameSystem.BattleSystem.Scripts;
using GameSystem.MVCTemplate;
using Tool.UI;
using Tool.Utilities;
using UnityEngine;

namespace GameSystem.BattleSystem.Main
{
    public class BattleSystemViewCtrl : BaseCtrl
    {
        public BattleSystemViewCtrl(AbsUnit player, QArray<AbsUnit> enemies) : base()
        {
            (GetModel() as BattleSystemViewModel).SetAbsUnit(player, enemies);
        }
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