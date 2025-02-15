using Assets.GameSystem.CardSystem;
using Assets.GameSystem.CardSystem.Scripts;
using Framework;
using GameSystem.MVCTemplate;
using Tool.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace Tips
{
    public class HistoryTips : BaseTips, ICanGetSystem
    {

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

        private Transform _content;
        private Text _txtTitle;
        private Text _txtCntTip;
        private GameObject _historyCell;

        protected override void Init()
        {
            _content = transform.Find("Main/bg/Scroll View/Viewport/Content");
            _historyCell = _content.transform.Find("historyCell").gameObject;
            _txtTitle = transform.Find("Main/bg/Txt_title").GetComponent<Text>();
            _txtCntTip = transform.Find("Main/bg/Txt_cntTip").GetComponent<Text>();
        }

        protected override void OnOpen(params object[] args)
        {
            //文本处理
            _txtTitle.text = "使用卡牌记录";
            _txtCntTip.text = "最多存10条记录";

            //只保留最近10条记录
            var useCardsHistory = args[0] as QArray<UseCardHistory>;
            var moreCnt = useCardsHistory.Count - 10;
            int index = 0;
            int historyCnt = useCardsHistory.Count;
            if (moreCnt > 0)
            {
                index += moreCnt;
                historyCnt = 10;
            }

            //生成记录
            for (int i = 0; i < _content.childCount; i++)
            {
                _content.GetChild(i).gameObject.SetActive(false);
            }
            for (int i = 1; i <= historyCnt; i++, index++)
            {
                GameObject cellGo = i > _content.childCount ? Instantiate(_historyCell, _content) : _content.GetChild(i - 1).gameObject;
                cellGo.SetActive(true);
                this.GetSystem<ICardSystemModule>().RenderHistoryInfo(cellGo.transform, useCardsHistory[index]);
            }
        }

        public override void OnRelease()
        {

        }
    }
}