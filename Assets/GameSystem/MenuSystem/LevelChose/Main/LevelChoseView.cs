using Assets.GameSystem.BattleSystem;
using Assets.GameSystem.CardSystem;
using Framework;
using GameSystem.MVCTemplate;
using Tool.UI;
using UIComponents;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.GameSystem.MenuSystem.LevelChose.Main
{
    public class LevelChoseView : BaseView
    {
        #region 自动生成UI组件区域，内部禁止手动更改！
		public CButton Btn_certain;
		public CButton Btn_close;
		public Text Txt_title;
        protected override void AutoInitUI()
        {
			Btn_certain = transform.Find("Main/Btn_certain").GetComponent<CButton>();
			Btn_close = transform.Find("Main/Btn_close").GetComponent<CButton>();
			Txt_title = transform.Find("Main/Txt_title").GetComponent<Text>();
        }
		#endregion 自动生成UI组件区域结束！

        #region 遮罩相关
        // /// <summary>
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

         private ToggleGroup _toggleGroup;
        private Transform _levels;

        /// <summary>
        /// 初始化,时机在Awake中
        /// </summary>
        protected override void OnInit()
        {
            //TODO:多语言
            Txt_title.text = "选择角色";
            Btn_certain.Text = "确定";

            _toggleGroup = transform.Find("Main/Levels").GetComponent<ToggleGroup>();
            _levels = transform.Find("Main/Levels");
            for (int i = 0; i < _levels.childCount; i++)
            {
                _levels.GetChild(i).GetComponent<Toggle>().onValueChanged.AddListener(ChoseLevel);
            }
            Btn_close.onClick.AddListener(Close);
            Btn_certain.onClick.AddListener(FinishChose);
        }

        /// <summary>
        /// 绑定model回调事件
        /// </summary>
        protected override void BindModelListener()
        {
        }

        public override void OnShow()
        {
            base.OnShow();
            // 初始化关卡信息
            InitLevelInfo();
        }

        public override void OnHide()
        {
            base.OnHide();
        }

        public override void OnRelease()
        {
            base.OnRelease();
        }

        /// <summary>
        /// 初始化关卡信息
        /// </summary>
        public void InitLevelInfo()
        {
            var model = Model as LevelChoseViewModel;
            for (int i = 0; i < _levels.childCount; i++)
            {
                var level = model.GetLevelById(i + 1);
                var txtName = _levels.GetChild(i).Find("txt_name").GetComponent<Text>();
                txtName.text = level.levelName;
            }
        }

        #region 事件
        private void ChoseLevel(bool isOn)
        {
            if (isOn)
            {
                for (int i = 0; i < _levels.childCount; i++)
                {
                    var toggle = _levels.GetChild(i).GetComponent<Toggle>();
                    if (toggle.isOn)
                    {
                        (Model as LevelChoseViewModel).SetChooseLevelId(i + 1);
                    }
                }
            }
        }
        private void FinishChose()
        {
            var nowCharacterId = (Model as LevelChoseViewModel).GetChooseLevelId();
            if (nowCharacterId != -1)
            {
                // 关闭Menu曾所有View
                UIManager.GetInstance().CloseAllViewByLayer(EuiLayer.MenuUI);
                //开始游戏
                var menuSystemModule = this.GetSystem<IMenuSystemModule>();
                Debug.Log($"选择了id为{menuSystemModule.GetNowChoseCharacterData().id}的角色和id为{menuSystemModule.GetNowChoseLevelData().id}的关卡");
                
                var nowCharacterData = menuSystemModule.GetNowChoseCharacterData();
                var nowLevelData = menuSystemModule.GetNowChoseLevelData();
                //打开战斗view
                this.GetSystem<IBattleSystemModule>().ShowView(nowCharacterData,nowLevelData);
                //打开卡组vie
                this.GetSystem<ICardSystemModule>().ShowView();
            }
        }
        private void Close()
        {
            _toggleGroup.SetAllTogglesOff();    //取消选中的角色
            (Model as LevelChoseViewModel).SetChooseLevelId(-1);
            OnHide();
        }
        #endregion
    }
}