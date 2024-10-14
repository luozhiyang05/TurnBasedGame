using Framework;
using GameSystem.BattleSystem;
using GameSystem.BattleSystem.Scripts;
using GameSystem.CardSystem.Scripts;
using GameSystem.MVCTemplate;
using Tool.Utilities;
using UIComponents;
using UnityEngine;
using UnityEngine.UI;

namespace GameSystem.CardSystem.Main
{
    public class CardSystemView : BaseView
    {
        #region 自动生成UI组件区域，内部禁止手动更改！
		public CButton Btn_exitRound;
		public Text Txt_actCnt;
        protected override void AutoInitUI()
        {
			Btn_exitRound = transform.Find("Main/Btn_exitRound").GetComponent<CButton>();
			Txt_actCnt = transform.Find("Main/Txt_actCnt").GetComponent<Text>();
        }
		#endregion 自动生成UI组件区域结束！

        /// <summary>d
        /// 绑定model回调事件
        /// </summary>
        protected override void BindModelListener()
        {
            _model.SetUpdateViewCallback(UpdateView);
        }

        private IBattleSystemModule _battleSystemModule;
        private ICardSystemModule _cardSystemModule;
        private CardSystemViewModel _model;
        
        private GameObject _cardTemp;
        private GameObject _cardsContent;
        private HorizontalLayoutGroup _cardsLayout;

        /// <summary>
        /// 初始化
        /// </summary>
        protected override void OnInit()
        {
            _battleSystemModule = this.GetSystem<IBattleSystemModule>();
            _cardSystemModule = this.GetSystem<ICardSystemModule>();
            
            //文本
            Btn_exitRound.Label.text = "结束回合";
            
            //按钮
            Btn_exitRound.onClick.AddListener(() =>
            {
                (_battleSystemModule.GetPlayerUnit() as Player)?.EndRound();
            });

            _cardsContent = transform.Find("Main/headCardsContent").gameObject;
            _cardTemp = _cardsContent.transform.GetChild(0).gameObject;
            _cardsLayout = _cardsContent.GetComponent<HorizontalLayoutGroup>();
        }


        public override void OnShow()
        {
            base.OnShow();
            _model = Model as CardSystemViewModel;

            //加载玩家出战卡牌
            _model?.LoadUseCards();
        }

        private QArray<BaseCardSo> _headCardQArray = new QArray<BaseCardSo>(5);
        private QArray<GameObject> _cardsGo = new QArray<GameObject>(5);

        /// <summary>
        /// 从出战卡牌中获取手牌
        /// </summary>
        private void GetHeadCards()
        {
            //获取玩家当前手牌
            _headCardQArray =  _model.GetNowHeadCards();
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
        /// 根据玩家手牌数，生成卡牌go
        /// </summary>
        private void CreateCardsGo()
        {
            //删除所有卡牌Go
            DestroyAllCardsGo();
            _cardsLayout.enabled = true;
            //根据手牌牌数生成按钮
            for (int i = 0; i < _headCardQArray.Count; i++)
            {
                var card = _headCardQArray[i];

                var cardGo = Instantiate(_cardTemp, _cardsContent.transform);
                var dc = cardGo.gameObject.AddComponent<DragCard>();
                dc.headCardIdx = i;
                dc.BaseCardSo = card;
                cardGo.transform.Find("bg/txt_name").GetComponent<Text>().text = card.cardName;
                cardGo.transform.Find("bg/txt_desc").GetComponent<Text>().text = card.cardDesc;
                cardGo.transform.SetParent(_cardsContent.transform);
                cardGo.gameObject.SetActive(true);
                _cardsGo.Add(cardGo);
            }
            LayoutRebuilder.ForceRebuildLayoutImmediate(_cardsContent.transform as RectTransform);
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
            //获取手牌，更新视图
            GetHeadCards();
            CreateCardsGo();
            //更新行动点
            UpdateActCnt();
        }


        public override void OnHide()
        {
            base.OnHide();
        }
    }
}