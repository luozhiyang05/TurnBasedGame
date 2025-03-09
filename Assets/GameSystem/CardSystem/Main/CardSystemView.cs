using Assets.GameSystem.BattleSystem;
using Assets.GameSystem.BattleSystem.Scripts;
using Assets.GameSystem.CardSystem.Scripts;
using Assets.GameSystem.FlyTextSystem;
using Assets.GameSystem.MenuSystem;
using Framework;
using GameSystem.MVCTemplate;
using GlobalData;
using Tips;
using Tool.Utilities;
using UIComponents;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.GameSystem.CardSystem.Main
{
    public class CardSystemView : BaseView
    {
        #region 自动生成UI组件区域，内部禁止手动更改！
		public Text Txt_actCnt;
		public CButton Btn_setting;
		public CButton Btn_exitRound;
		public CButton Btn_obsCards;
		public CButton Btn_history;
		public CButton Btn_useCards;
        protected override void AutoInitUI()
        {
			Txt_actCnt = transform.Find("Main/Txt_actCnt").GetComponent<Text>();
			Btn_setting = transform.Find("Main/Btn_setting").GetComponent<CButton>();
			Btn_exitRound = transform.Find("Main/Btn_exitRound").GetComponent<CButton>();
			Btn_obsCards = transform.Find("Main/Btn_obsCards").GetComponent<CButton>();
			Btn_history = transform.Find("Main/Btn_history").GetComponent<CButton>();
			Btn_useCards = transform.Find("Main/Btn_useCards").GetComponent<CButton>();
        }
		#endregion 自动生成UI组件区域结束！

        /// <summary>d
        /// 绑定model回调事件
        /// </summary>
        protected override void BindModelListener()
        {
            _model.SetUpdateViewCallback(UpdateView);
            _model.SetUseCardCallback(UpdateHeadCard);
        }

        private IBattleSystemModule _battleSystemModule;
        private ICardSystemModule _cardSystemModule;
        private CardSystemViewModel _model;
        private GameObject _cardTemp;
        private GameObject _cardsContent;
        /// <summary>
        /// 初始化
        /// </summary>
        protected override void OnInit()
        {
            _battleSystemModule = this.GetSystem<IBattleSystemModule>();
            _cardSystemModule = this.GetSystem<ICardSystemModule>();

            _cardsContent = transform.Find("Main/headCardsContent").gameObject;
            _cardTemp = _cardsContent.transform.GetChild(0).gameObject;

            //文本
            Btn_useCards.Label.text = GameManager.GetText("battle_tip_1001");
            Btn_exitRound.Label.text = GameManager.GetText("battle_tip_1002");
            Btn_obsCards.Label.text = GameManager.GetText("battle_tip_1003");
            Btn_history.Label.text = GameManager.GetText("battle_tip_1004");
            Btn_setting.Label.text = GameManager.GetText("menu_1002");

            //按钮
            Btn_exitRound.onClick.AddListener(() =>
            {
                (_battleSystemModule.GetPlayerUnit() as Player)?.EndRound();
            });
            Btn_obsCards.onClick.AddListener(() =>
            {
                _cardSystemModule.ShowObsCardsView(_model.GetDiscardCards(), false);
            });
            Btn_useCards.onClick.AddListener(() =>
            {
                _cardSystemModule.ShowObsCardsView(_model.GetUserCards(), true);
            });
            Btn_history.onClick.AddListener(() =>
            {
                TipsModule.HistoryTips(_model.GetHistory());
            });
            Btn_setting.onClick.AddListener(() =>
            {
                this.GetSystem<IMenuSystemModule>().ShowSettingView(Tool.UI.EuiLayer.GameUI);
            });
        }

        public override void OnShow()
        {
            base.OnShow();

            _model = Model as CardSystemViewModel;

            //删除可能残留的卡牌go
            DestroyAllCardsGo();
        }

        private QArray<BaseCard> _headCardQArray = new QArray<BaseCard>(5);
        private QArray<GameObject> _cardsGo = new QArray<GameObject>(5);

        /// <summary>
        /// 从出战卡牌中获取手牌
        /// </summary>
        private void GetHeadCards()
        {
            //获取玩家当前手牌
            _headCardQArray = _model.GetNowHeadCards();
        }

        /// <summary>
        /// 回合结束，删除所有卡牌go
        /// </summary>
        private void DestroyAllCardsGo()
        {
            foreach (GameObject cardGo in _cardsGo)
            {
                Destroy(cardGo);
            }
            _cardsGo.Clear();
        }

        /// <summary>
        /// 出牌后删除一张手牌go
        /// </summary>
        /// <param name="headCardIdx"></param>
        private void DestroyOneCardGo(int headCardIdx)
        {
            var cardGo = _cardsGo.Remove(_cardsGo[headCardIdx]);
            Destroy(cardGo);
        }

        /// <summary>
        /// 根据玩家手牌数，生成卡牌go
        /// </summary>
        private void CreateCardsGo()
        {
            //根据手牌牌数生成按钮
            for (int i = 0; i < _headCardQArray.Count; i++)
            {
                var cardGo = Instantiate(_cardTemp, _cardsContent.transform);
                _cardsGo.Add(cardGo);
            }
        }

        /// <summary>
        /// 更新玩家行动点
        /// </summary>
        private void UpdateActCnt()
        {
            var player = _battleSystemModule.GetPlayerUnit() as Player;
            Txt_actCnt.text = player.nowActPoint + "/" + player.maxActPoint;
        }

        /// <summary>
        /// 回合开始时更新卡牌视图
        /// </summary>
        private void UpdateView()
        {
            GetHeadCards();
            DestroyAllCardsGo();
            CreateCardsGo();
            _cardSystemModule.RenderHandCards(_cardsGo, _headCardQArray, _cardsContent.transform);
            UpdateActCnt();
        }

        /// <summary>
        /// 出牌后更新玩家手牌视图
        /// </summary>
        /// <param name="headCardIdx"></param>
        private void UpdateHeadCard(int headCardIdx)
        {
            GetHeadCards();
            DestroyOneCardGo(headCardIdx);
            print(_cardsContent);
            _cardSystemModule.RenderHandCards(_cardsGo, _headCardQArray, _cardsContent.transform);
            UpdateActCnt();
        }

        public override void OnHide()
        {
            base.OnHide();
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                print(_cardsContent);
            }
        }
    }
}