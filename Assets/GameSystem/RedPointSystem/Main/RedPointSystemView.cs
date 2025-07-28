using Assets.Wights.Scripts;
using Framework;
using GameSystem.MVCTemplate;
using UIComponents;

namespace Assets.GameSystem.RedPointSystem.Main
{
    public class RedPointSystemView : BaseView
    {
        #region 自动生成UI组件区域，内部禁止手动更改！
        protected override void AutoInitUI()
        {
        }
        #endregion 自动生成UI组件区域结束！

        #region 遮罩相关
        /// <summary>
        /// 是否启用MaskPanel，启用的话只需要取消注释
        /// </summary>
        /// <returns></returns>
        public override bool MaskPanel()
        {
            return true;
        }

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

        public CButton btn_one, btn_two, btn_open, btn_add, btn_reduce,btn_fireFun;

        /// <summary>
        /// 初始化,时机在Awake中
        /// </summary>
        protected override void OnInit()
        {
            SetPath("RedPointSystem/RedPointSystemView");
            btn_one.Text = "一级按钮";
            btn_two.Text = "二级按钮";
            btn_add.Text = "添加Test1红点";
            btn_reduce.Text = "减少Test1红点";
            btn_fireFun.Text = "执行方法触发Test2红点";
            btn_open.Text = "打开面板";


            RedPointMgr.GetInstance().Register(btn_one.gameObject, RedPointDef.ONE);
            RedPointMgr.GetInstance().Register(btn_two.gameObject, RedPointDef.ONE_TWO);
        }


        public override void OnShow(params object[] args)
        {
            base.OnShow(args);

            btn_open.AddListener(() =>
            {
                this.GetSystem<IRedPointSystemModule>().OpenSon(this);
            });

            btn_add.AddListener(() =>
            {
                RedPointMgr.GetInstance().Fire(RedPointDef.ONE_TWO_TEST1,1);
            });
            btn_fireFun.AddListener(() =>
            {
                RedPointMgr.GetInstance().FireWithFun(RedPointDef.ONE_TWO_TEST2);
            });
            btn_reduce.AddListener(() =>
            {
                RedPointMgr.GetInstance().Fire(RedPointDef.ONE_TWO_TEST1, -1);
            });
        }

        public override void OnHide()
        {
            base.OnHide();
        }

        public override void OnRelease()
        {
            base.OnRelease();
        }
    }
}