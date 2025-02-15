using Framework;
using GameSystem.MVCTemplate;
using UIComponents;

namespace Assets.GameSystem.MenuSystem.Main
{
    public class MenuSystemView : BaseView
    {
        #region 自动生成UI组件区域，内部禁止手动更改！
		public CButton Btn_startGame;
        protected override void AutoInitUI()
        {
			Btn_startGame = transform.Find("Main/Btn_startGame").GetComponent<CButton>();
        }
		#endregion 自动生成UI组件区域结束！

        #region 遮罩相关
        // /// <summary>
        // /// 是否启用MaskPanel，启用的话只需要取消注释
        // /// </summary>
        // /// <returns></returns>
        // public override bool MaskPanel()
        // {
        //     return true;
        // }

        // /// <summary>
        // /// 是否开启点击遮罩关闭View，启用的话只需要取消注释
        // /// </summary>
        // /// <returns></returns>
        // public override bool ClickMaskPanel()
        // {
        //     return true;
        // }

        // /// <summary>
        // /// 是否重写遮罩事件，重写后不执行父类点击遮罩关闭事件
        // /// </summary>
        // /// <returns></returns>
        // public override void OnClickMaskPanel()
        // {
        //     Debug.Log("点击了遮罩！");
        // }
        #endregion

        /// <summary>
        /// 绑定model回调事件
        /// </summary>
        protected override void BindModelListener()
        {
        }

        /// <summary>
        /// 初始化,时机在Awake中
        /// </summary>
        protected override void OnInit()
        {
            Btn_startGame.Text = "开始游戏";
            Btn_startGame.onClick.AddListener(OpenCharacterChoseView);
        }


        public override void OnShow()
        {
            base.OnShow();
        }

        public override void OnHide()
        {
            base.OnHide();
        }

        public override void OnRelease()
        {
            base.OnRelease();
        }

        #region 按钮事件
        private void OpenCharacterChoseView()
        {
            this.GetSystem<IMenuSystemModule>().ShowCharacterChoseView();
        }
        #endregion
    }
}