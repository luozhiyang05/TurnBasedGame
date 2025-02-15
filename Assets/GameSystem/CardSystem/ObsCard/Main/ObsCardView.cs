using Assets.GameSystem.CardSystem.Scripts;
using Framework;
using GameSystem.CardSystem.ObsCard.Main;
using GameSystem.MVCTemplate;
using Tool.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.GameSystem.CardSystem.ObsCard.Main
{
    public class ObsCardView : BaseView
    {
        #region 自动生成UI组件区域，内部禁止手动更改！
        public Text Txt_title;
        protected override void AutoInitUI()
        {
            Txt_title = transform.Find("Main/bg/Txt_title").GetComponent<Text>();
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

        private Transform _content;
        private GameObject _cardGo;
        private QArray<BaseCardSo> _obsCards;
        private ObsCardViewModel _model;
        private bool _isUseCards;
        /// <summary>
        /// 初始化,时机在Awake中
        /// </summary>
        protected override void OnInit()
        {
            _content = transform.Find("Main/bg/Scroll View/Viewport/Content");
            _cardGo = _content.transform.Find("card").gameObject;
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
            _model = Model as ObsCardViewModel;

            //文本    
            Txt_title.text = _isUseCards ? "牌库" : "弃牌堆";

            //视图更新
            UpdateObsCardsView();
        }

        public override void OnHide()
        {
            base.OnHide();
        }

        public override void OnRelease()
        {
            base.OnRelease();
        }

        public void SetDataSource(QArray<BaseCardSo> obsCards, bool isUseCards)
        {
            _obsCards = obsCards;
            _isUseCards = isUseCards;
        }

        private void UpdateObsCardsView()
        {
            for (int i = 0; i < _content.childCount; i++)
            {
                _content.GetChild(i).gameObject.SetActive(false);
            }
            var cardCellCnt = _content.childCount - 1;
            for (int i = 1; i <= _obsCards.Count; i++)
            {
                GameObject cardGo = i > cardCellCnt ? Instantiate(_cardGo, _content) : _content.GetChild(i).gameObject;
                var card = _obsCards[i - 1];
                this.GetSystem<ICardSystemModule>().RenderCardInfo(cardGo.transform, card);
                cardGo.SetActive(true);
            }
        }
    }
}