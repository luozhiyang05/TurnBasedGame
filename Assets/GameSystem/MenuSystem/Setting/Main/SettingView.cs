using Assets.GameSystem.BattleSystem;
using Framework;
using GameSystem.MVCTemplate;
using GlobalData;
using Tips;
using Tool.AudioMgr;
using Tool.UI;
using UIComponents;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.GameSystem.MenuSystem.Setting.Main
{
    public class SettingView : BaseView
    {
        #region 自动生成UI组件区域，内部禁止手动更改！
		public CButton Btn_close;
		public CButton Btn_backToMenu;
		public Text Txt_title;
		public Text Txt_effectAudio;
		public Text Txt_bgmAudio;
		public Text Txt_language;
        protected override void AutoInitUI()
        {
			Btn_close = transform.Find("Main/bg/Btn_close").GetComponent<CButton>();
			Btn_backToMenu = transform.Find("Main/bg/Btn_backToMenu").GetComponent<CButton>();
			Txt_title = transform.Find("Main/bg/Txt_title").GetComponent<Text>();
			Txt_effectAudio = transform.Find("Main/bg/Slider_effect/Txt_effectAudio").GetComponent<Text>();
			Txt_bgmAudio = transform.Find("Main/bg/Slider_bgm/Txt_bgmAudio").GetComponent<Text>();
			Txt_language = transform.Find("Main/bg/Dropdown/Txt_language").GetComponent<Text>();
        }
		#endregion 自动生成UI组件区域结束！

        #region 遮罩相关
        // /// <summary>
        // /// 是否启用MaskPanel，启用的话只需要取消注释
        // /// </summary>
        // /// <returns></returns>
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

        public Dropdown dropdown;
        public Slider Slider_effect;
        public Slider Slider_bgm;

        /// <summary>
        /// 初始化,时机在Awake中
        /// </summary>
        protected override void OnInit()
        {
            // 多语言
            Txt_title.text = GameManager.GetText("menu_1002");
            Txt_effectAudio.text = GameManager.GetText("setting_1001");
            Txt_bgmAudio.text = GameManager.GetText("setting_1002");
            Txt_language.text = GameManager.GetText("setting_1003");
            Btn_backToMenu.Label.text = GameManager.GetText("menu_1003");

            // 音量控制
            Slider_effect = transform.Find("Main/bg/Slider_effect").GetComponent<Slider>();
            Slider_effect.value = AudioManager.GetInstance().GetVolume(EAudioType.Effect);
            Slider_effect.onValueChanged.AddListener(value =>
            {
                AudioManager.GetInstance().ModifyVolume(EAudioType.Effect, value);
            });
            Slider_bgm = transform.Find("Main/bg/Slider_bgm").GetComponent<Slider>();
            Slider_bgm.value = AudioManager.GetInstance().GetVolume(EAudioType.Bgm);
            Slider_bgm.onValueChanged.AddListener(value =>
            {
                AudioManager.GetInstance().ModifyVolume(EAudioType.Bgm, value);
            });

            // 语言下拉框
            dropdown = transform.Find("Main/bg/Dropdown").GetComponent<Dropdown>();
            var options = dropdown.options;
            options.Add(new Dropdown.OptionData(GameManager.GetText("multi_langague_1001")));
            options.Add(new Dropdown.OptionData(GameManager.GetText("multi_langague_1002")));
            dropdown.options = options;

            // 语言选择事件
            dropdown.value = PlayerPrefs.GetInt("languages", 0);
            dropdown.onValueChanged.AddListener(_ =>
            {
                Debug.Log("选择语言：" + dropdown.options[dropdown.value].text);
                int nowLanguesIndex = dropdown.value;

                // 确认更换语言提示
                TipsModule.ReComfirmTips("tips_1001", "tips_1004", () =>
                {
                    Debug.Log("退出游戏");
                    PlayerPrefs.SetInt("languages", nowLanguesIndex);
                    Application.Quit();
                }, () =>
                {
                    dropdown.SetValueWithoutNotify(PlayerPrefs.GetInt("languages", 0));
                });
            });

            // 按钮事件
            Btn_close.onClick.AddListener(() =>
            {
                OnHide();
            });
            Btn_backToMenu.onClick.AddListener(() =>
            {
                this.GetSystem<IBattleSystemModule>().SetIsStartBattle(false);
                UIManager.GetInstance().CloseAllViewByLayer(EuiLayer.GameUI);
                this.GetSystem<IMenuSystemModule>().ShowView();
                OnHide();
            });
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