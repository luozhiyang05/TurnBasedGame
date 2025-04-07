using Assets.GameSystem.BattleSystem.Scripts;
using Assets.GameSystem.CardSystem.Scripts;
using GlobalData;
using Tool.UI;
using Tool.Utilities;
using UnityEngine;
using UnityEngine.Events;

namespace Tips
{
    public static class TipsModule
    {
        private static UIManager uiMgr => UIManager.GetInstance();
        public static void ReComfirmTips(string titleKey, string contentKey, UnityAction comfirm, UnityAction cancel)
        {
            uiMgr.GetFromPool("ReConfirmTips", EuiLayer.TipsUI, (tips) =>
            {
                var reConfirmTips = tips as ReConfirmTips;
                reConfirmTips.Open(titleKey, contentKey, comfirm, cancel);
            });
        }

        public static void ComfirmTips(string titleKey, string contentKey, UnityAction comfirm)
        {
            uiMgr.GetFromPool("ComfirmTips", EuiLayer.TipsUI, (tips) =>
            {
                var confirmTips = tips as ComfirmTips;
                confirmTips.Open(GameManager.GetText(titleKey), GameManager.GetText(contentKey), comfirm);
            });
        }

        public static void HistoryTips(QArray<UseCardHistory> useCardsHistory)
        {
            uiMgr.GetFromPool("HistoryTips", EuiLayer.TipsUI, (tips) =>
            {
                var historyTips = tips as HistoryTips;
                historyTips.Open(useCardsHistory);
            });
        }

        public static void UnitInfoTips(UnitInfoPacking unitInfoPacking)
        {
            uiMgr.GetFromPool("UnitInfoTips", EuiLayer.TipsUI, (tips) =>
            {
                var unitInfoTips = tips as UnitInfoTips;
                unitInfoTips.Open(unitInfoPacking);
            });
        }

        public static void DescTips(string descKey)
        {
            uiMgr.GetFromPool("DescTips", EuiLayer.TipsUI, (tips) =>
            {
                var descTips = tips as DescTips;
                descTips.Open(descKey);
            });
        }

        /// <summary>
        /// 打开卡牌检测Tips
        /// </summary>
        /// <param name="cardIndexInUseCards">牌库中的卡牌下标</param>
        /// <param name="chooseCardsCnt">选择的卡牌数量</param>
        /// <param name="chooseAction">确定的方法</param>
        /// <param name="titleKey">标题</param>
        public static void CardsCheckTips(QArray<int> cardIndexInUseCards,int chooseCardsCnt,UnityAction<QArray<int>> chooseAction,string titleKey)
        {
            uiMgr.GetFromPool("CardsCheckTips", EuiLayer.TipsUI, (tips) =>
            {
                var cardsCheckTips = tips as CardsCheckTips;
                cardsCheckTips.Open(cardIndexInUseCards, chooseCardsCnt, titleKey);
                cardsCheckTips.SetChooseAction(chooseAction);
            });
        }

        public static void HandbookDisplayTips(QArray<InfosPacking> qArray,HandbookType handbookType)
        {
            uiMgr.GetFromPool("HandbookDisplayTips", EuiLayer.TipsUI, (tips) =>
            {
                var HandbookDisplayTips = tips as HandbookDisplayTips;
                HandbookDisplayTips.SetHandbookType(handbookType);
                HandbookDisplayTips.Open(qArray);
            });
        }

        public static void InfoTips(InfosPacking infosPacking)
        {
            uiMgr.GetFromPool("InfoTips", EuiLayer.TipsUI, (tips) =>
            {
                var infoTips = tips as InfoTips;
                infoTips.Open(infosPacking);
            });
        }
    }
}