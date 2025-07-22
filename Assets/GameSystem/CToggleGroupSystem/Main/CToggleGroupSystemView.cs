using System.Collections.Generic;
using Assets.Wights.Scripts;
using Framework;
using GameSystem.MVCTemplate;

namespace Assets.GameSystem.CToggleGroupSystem.Main
{
    public class CToggleGroupSystemView : BaseView
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

        /// <summary>
        /// 是否开启点击遮罩关闭View，启用的话只需要取消注释
        /// </summary>
        /// <returns></returns>
        public override bool ClickMaskPanel()
        {
            return true;
        }

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

        private CToggleGroup _cToggleGroup;

        /// <summary>
        /// 初始化,时机在Awake中
        /// </summary>
        protected override void OnInit()
        {
            SetName("CToggleGroupSystemView");
            _cToggleGroup = LoadAsset<CToggleGroup>("CToggleGroupSystem", "ToggleGroup", 0);
        }


        public override void OnShow(params object[] args)
        {
            base.OnShow(args);
            
            if (!_cToggleGroup.init)
            {
                var module = this.GetSystem<ICToggleGroupSystemModule>();
                var data = new ToggleGroupData()
                .SetTexts(new List<string>() { "测试1", "测试2", "测试3" })
                .SetCtrl(module.GetCtrl())
                .Add(ECToggleGroupSystemViews.ToggleOne, () => module.ToggleOne(this,"测试1"))
                .Add(ECToggleGroupSystemViews.ToggleTwo, () => module.ToggleOne(this,"测试2"))
                .Add(ECToggleGroupSystemViews.ToggleOne, () => module.ToggleOne(this, "测试3"));
                _cToggleGroup.InitToggleGroup(data);
            }
            _cToggleGroup.SelectIndex(0);
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