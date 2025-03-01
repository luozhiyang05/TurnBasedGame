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

        public static void CardsCheckTips(QArray<int> cardIndexInUseCards,UnityAction<QArray<int>> chooseAction,string titleKey)
        {
            uiMgr.GetFromPool("CardsCheckTips", EuiLayer.TipsUI, (tips) =>
            {
                var cardsCheckTips = tips as CardsCheckTips;
                cardsCheckTips.Open(cardIndexInUseCards, 2, titleKey);
                cardsCheckTips.SetChooseAction(chooseAction);
            });
        }
    }
}