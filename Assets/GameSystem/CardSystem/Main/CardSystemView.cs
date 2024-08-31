using Framework;
using GameSystem.BattleSystem;
using GameSystem.BattleSystem.Scripts;
using GameSystem.CardSystem.Scripts;
using GameSystem.MVCTemplate;
using Tool.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace GameSystem.CardSystem.Main
{
    public class CardSystemView : BaseView
    {
        #region 自动生成UI组件区域，内部禁止手动更改！
		public Button Btn_exitRound;
		public Text Txt_exitRound;
        protected override void AutoInitUI()
        {
			Btn_exitRound = transform.Find("Main/Btn_exitRound").GetComponent<Button>();
			Txt_exitRound = transform.Find("Main/Btn_exitRound/Txt_exitRound").GetComponent<Text>();
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

        /// <summary>
        /// 初始化
        /// </summary>
        protected override void OnInit()
        {
            _battleSystemModule = this.GetSystem<IBattleSystemModule>();
            _cardSystemModule = this.GetSystem<ICardSystemModule>();
            
            //文本
            Txt_exitRound.text = "结束回合";
            
            //按钮
            Btn_exitRound.onClick.AddListener(() =>
            {
                (_battleSystemModule.GetPlayerUnit() as Player)?.EndRound();
            });

            _cardTemp = transform.Find("Main/UseCardsContent/cardTemplate").gameObject;
            _cardsContent = transform.Find("Main/UseCardsContent").gameObject;
        }


        public override void OnShow()
        {
            base.OnShow();
            _model = Model as CardSystemViewModel;

            //加载玩家出战卡牌
            _model?.LoadUseCards();
        }

        private QArray<BaseCardSo> _headArrayDataSources;
        private GameObject[] _cards;

        /// <summary>
        /// 初始化数据源
        /// </summary>
        private void InitDataSources()
        {
            //获取玩家当前手牌
            var data = _model.GetNowHeadCards();
            _headArrayDataSources = data;
        }

        /// <summary>
        /// 回合结束，丢弃目前所有手牌
        /// </summary>
        private void DiscardCards()
        {
            if (_cards != null)
            {
                foreach (var item in _cards)
                {
                    Destroy(item);
                }
            }
        }
        
        /// <summary>
        /// 初始化视图
        /// </summary>
        private void InitView()
        {
            //更新丢弃卡牌的视图
            DiscardCards();
            
            //根据手牌牌数生成按钮
            _cards = new GameObject[_headArrayDataSources.Count];
            for (int i = 0; i < _headArrayDataSources.Count; i++)
            {
                var headArrayDataSource = _headArrayDataSources[i];
                if (headArrayDataSource == null)
                {
                    continue;
                }
                
                var index = i;
                var btn = Instantiate(_cardTemp, _cardsContent.transform);
                btn.gameObject.AddComponent<DragCard>();
                btn.GetComponentInChildren<Text>().text = headArrayDataSource.cardName;
                // btn.onClick.AddListener(() =>
                // {
                //     //获取当前玩家
                //     var player = _battleSystemModule.GetPlayerUnit();
                //     //获取当前敌人
                //     var enemy = _battleSystemModule.GetEnemyUnit(0);
                //     //使用卡牌
                //     (player as Player)?.UseCard(_headArrayDataSources[index], enemy);
                //     //删除卡牌按钮
                //     Destroy(btn.gameObject);
                // });
                btn.transform.SetParent(_cardsContent.transform);
                btn.gameObject.SetActive(true);
                _cards[i] = btn.gameObject;
            }
        }

        /// <summary>
        /// 回合开始时更新卡牌视图
        /// </summary>
        private void UpdateView()
        {
            //初始化数据源
            InitDataSources();
            
            //初始化视图
            InitView();
        }
        

        public override void OnHide()
        {
            base.OnHide();
        }
    }
}