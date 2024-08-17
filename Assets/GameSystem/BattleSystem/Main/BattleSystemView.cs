using Framework;
using GameSystem.MVCTemplate;
using UnityEngine.UI;

namespace GameSystem.BattleSystem.Main
{
    public class BattleSystemView : BaseView
    {
        #region 自动生成UI组件区域，内部禁止手动更改！

        public Button Btn_attack;
        public Button Btn_defense;

        protected override void AutoInitUI()
        {
            Btn_attack = transform.Find("Main/Btn_attack").GetComponent<Button>();
            Btn_defense = transform.Find("Main/Btn_defense").GetComponent<Button>();
        }

        #endregion 自动生成UI组件区域结束！

        /// <summary>
        /// 绑定model回调事件
        /// </summary>
        protected override void BindModelListener()
        {
        }

        /// <summary>
        /// 初始化
        /// </summary>
        protected override void OnInit()
        {
            Btn_attack.onClick.AddListener(() =>
            {
                this.GetSystem<IBattleSystemModule>().PlayerAct();
            });
            Btn_defense.onClick.AddListener(() =>
            {
                this.GetSystem<IBattleSystemModule>().PlayerAct();
            });
        }


        public override void OnShow()
        {
            base.OnShow();
        }

        public override void OnHide()
        {
            base.OnHide();
        }
    }
}