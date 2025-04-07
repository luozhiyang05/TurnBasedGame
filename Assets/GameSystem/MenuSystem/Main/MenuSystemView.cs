using Assets.GameSystem.CardSystem;
using Assets.GameSystem.EffectsSystem;
using Assets.GameSystem.EnemySystem;
using Framework;
using GameSystem.MVCTemplate;
using GlobalData;
using Tips;
using Tool.Utilities;
using UIComponents;

namespace Assets.GameSystem.MenuSystem.Main
{
    public class MenuSystemView : BaseView
    {
        #region 自动生成UI组件区域，内部禁止手动更改！
		public CButton Btn_startGame;
		public CButton Btn_setting;
		public CButton Btn_effects;
		public CButton Btn_cards;
		public CButton Btn_enemies;
		public CButton Btn_quit;
        protected override void AutoInitUI()
        {
			Btn_startGame = transform.Find("Main/Btn_startGame").GetComponent<CButton>();
			Btn_setting = transform.Find("Main/Btn_setting").GetComponent<CButton>();
			Btn_effects = transform.Find("Main/Btn_effects").GetComponent<CButton>();
			Btn_cards = transform.Find("Main/Btn_cards").GetComponent<CButton>();
			Btn_enemies = transform.Find("Main/Btn_enemies").GetComponent<CButton>();
			Btn_quit = transform.Find("Main/Btn_quit").GetComponent<CButton>();
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
            Btn_startGame.Text = GameManager.GetText("menu_1001");
            Btn_setting.Text = GameManager.GetText("menu_1002");

            Btn_startGame.onClick.AddListener(OpenCharacterChoseView);
            Btn_setting.onClick.AddListener(OpenSettingView);
            Btn_effects.onClick.AddListener(OpenEffectsHandbookTips);
            Btn_cards.onClick.AddListener(OpenCardsHandbookTips);
            Btn_enemies.onClick.AddListener(OpenEnemiesHandbookTips);
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
        private void OpenSettingView()
        {
            this.GetSystem<IMenuSystemModule>().ShowSettingView();
        }
        private void OpenEffectsHandbookTips()
        {
            var qArray = new QArray<InfosPacking>();
            var effectsSystemModule = this.GetSystem<IEffectsSystemModule>();
            for (int i = 0; i < effectsSystemModule.GetEffectsCnt(); i++)
            {
                qArray.Add(new InfosPacking
                {
                    sprite = effectsSystemModule.GetEffectIcon(i+1),
                    name = effectsSystemModule.GetEffectName(i+1),
                    desc = effectsSystemModule.GetEffectDesc(i+1)
                });
            }
            TipsModule.HandbookDisplayTips(qArray,HandbookType.Effect);
        }
        private void OpenCardsHandbookTips()
        {
            var qArray = new QArray<InfosPacking>();
            var cardsSystemModule = this.GetSystem<ICardSystemModule>();
            for (int i = 0; i < cardsSystemModule.GetAllCardsCnt(); i++)
            {
                qArray.Add(new InfosPacking
                {
                    sprite = cardsSystemModule.GetCardIcon(i+1),
                    name = cardsSystemModule.GetCardName(i+1),
                    desc = cardsSystemModule.GetCardDesc(i+1)
                });
            }
            TipsModule.HandbookDisplayTips(qArray,HandbookType.Card);
        }
        private void OpenEnemiesHandbookTips()
        {
            var qArray = new QArray<InfosPacking>();
            var enemySystemModule = this.GetSystem<IEnemySystemModule>();
            for (int i = 0; i < enemySystemModule.GetAllEnemiesCnt(); i++)
            {
                qArray.Add(new InfosPacking
                {
                    sprite = enemySystemModule.GetEnemyIcon(i+1),
                    name = enemySystemModule.GetEnemyName(i+1),
                    desc = enemySystemModule.GetEnemyDesc(i+1)
                });
            }
            TipsModule.HandbookDisplayTips(qArray,HandbookType.Enemy);
        }
        #endregion
    }
}