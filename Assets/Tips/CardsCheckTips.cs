using System.Collections.Generic;
using Assets.GameSystem.CardSystem;
using Framework;
using GameSystem.MVCTemplate;
using GlobalData;
using Tool.Utilities;
using UIComponents;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Tips
{
    public class CardsCheckTips : BaseTips
    {
        public Text Txt_tip;
        public CButton Btn_certain;
        public Transform bg;
        public Transform card;
        private QArray<int> cardIndexInUseCards;
        private QArray<bool> _selectCards;
        private int _needChooseCnt;
        private int _nowChooseCnt;
        private QArray<int> _chooseCardIds;
        private UnityAction<QArray<int>> _chooseAction;
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
        #endregion

        protected override void Init()
        {

        }

        protected override void OnOpen(params object[] args)
        {
            // 多语言配置
            Txt_tip.text = args[2] as string;
            Btn_certain.Label.text = GameManager.GetText("tips_1003");

            // 初始化cardGo
            for (int i = 0; i < bg.childCount; i++)
            {
                bg.GetChild(i).Find("selectBg").gameObject.SetActive(false);
            }

            // 获取数据
            cardIndexInUseCards = args[0] as QArray<int>;
            _needChooseCnt = int.Parse(args[1].ToString());

            // 初始化选择的卡牌数量
            _nowChooseCnt = 0;
            _selectCards = null;
            _selectCards = new QArray<bool>(cardIndexInUseCards.Count);
            for (int i = 0; i < cardIndexInUseCards.Count; i++)
            {
                // 默认不选中
                _selectCards.Add(false);

                // 获取卡牌Trans
                Transform cardTrans = bg.childCount < (i + 1) ? Instantiate(card.gameObject, bg).transform : bg.GetChild(i);
                cardTrans.gameObject.SetActive(true);

                // 渲染卡牌
                var cardSystemModule = this.GetSystem<ICardSystemModule>();
                var cardId = cardSystemModule.GetCardIdByUseCardsIndex(cardIndexInUseCards[i] - 1);
                var baseCard = cardSystemModule.GetCardById(cardId);
                cardSystemModule.RenderCardInfo(cardTrans, baseCard);

                // 绑定点击事件
                var tempIdx = i;
                cardTrans.GetComponent<Button>().onClick.RemoveAllListeners();
                cardTrans.GetComponent<Button>().onClick.AddListener(() =>
                {
                    SelectCard(tempIdx);
                });
            }

            // 设置点击事件
            Btn_certain.onClick.RemoveAllListeners();
            Btn_certain.onClick.AddListener(() =>
            {
                var temp = new QArray<int>();
                for (int i = 0; i < cardIndexInUseCards.Count; i++)
                {
                    if (_selectCards[i] == true)
                    {
                        temp.Add(cardIndexInUseCards[i]);
                    }
                }
                if (temp.Count > 0)
                {
                    _chooseAction(temp);
                    OnHide();
                }
            });
        }

        private void SelectCard(int index)
        {
            if (_selectCards[index] == false && _nowChooseCnt < _needChooseCnt)
            {
                _nowChooseCnt++;
                _selectCards[index] = true;
            }
            else if (_selectCards[index] == true && _nowChooseCnt > 0)
            {
                _nowChooseCnt--;
                _selectCards[index] = false;
            }
            bg.GetChild(index).Find("selectBg").gameObject.SetActive(_selectCards[index]);
        }

        public void SetChooseAction(UnityAction<QArray<int>> chooseAction)
        {
            _chooseAction = chooseAction;
        }
        public override void OnRelease()
        {

        }
    }
}