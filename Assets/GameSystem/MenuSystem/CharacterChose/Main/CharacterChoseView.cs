using Framework;
using GameSystem.MVCTemplate;
using GlobalData;
using Tool.ResourceMgr;
using UIComponents;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.GameSystem.MenuSystem.CharacterChose.Main
{
    public class CharacterChoseView : BaseView
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

        private ToggleGroup _toggleGroup;
        private Transform _charactes;

        /// <summary>
        /// 初始化,时机在Awake中
        /// </summary>
        protected override void OnInit()
        {
            //TODO:多语言
            Txt_title.text = GameManager.GetText("choose_character_1001");
            Btn_certain.Text = GameManager.GetText("tips_1003");

            _toggleGroup = transform.Find("Main/Characters").GetComponent<ToggleGroup>();
            _charactes = transform.Find("Main/Characters");
            for (int i = 0; i < _charactes.childCount; i++)
            {
                _charactes.GetChild(i).GetComponent<Toggle>().onValueChanged.AddListener(ChoseCharacter);
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
            // 初始化角色信息
            InitCharactersInfo();
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
        /// 初始化角色信息
        /// </summary>
        public void InitCharactersInfo()
        {
            var model = Model as CharacterChoseViewModel;
            for (int i = 0; i < _charactes.childCount; i++)
            {
                var characterData = model.GetCharacterDataById(i + 1);
                var characterCode = _charactes.GetChild(i);
                var bg = characterCode.Find("Background");
                var txtName = characterCode.Find("txt_name").GetComponent<Text>();
                txtName.text = GameManager.GetText(characterData.characterType.ToString());
                var sprite = ResMgr.GetInstance().SyncLoad<Sprite>(GameManager.GetIconPath(characterData.iconName, 1));
                bg.GetComponent<Image>().sprite = sprite;
            }
        }

        #region 事件
        private void Close()
        {
            _toggleGroup.SetAllTogglesOff();    //取消选中的角色
            (Model as CharacterChoseViewModel).SetChoseCharacter(-1);
            OnHide();
        }
        private void FinishChose()
        {
            var nowCharacterId = (Model as CharacterChoseViewModel).GetChoseCharacterId();
            if (nowCharacterId != -1)
            {
                //打开选关界面
                this.GetSystem<IMenuSystemModule>().ShowLevelChoseView();
            }
        }
        private void ChoseCharacter(bool isOn)
        {
            if (isOn)
            {
                for (int i = 0; i < _charactes.childCount; i++)
                {
                    var toggle = _charactes.GetChild(i).GetComponent<Toggle>();
                    if (toggle.isOn)
                    {
                        (Model as CharacterChoseViewModel).SetChoseCharacter(i+1);
                    }
                }
            }
        }
        #endregion
    }
}