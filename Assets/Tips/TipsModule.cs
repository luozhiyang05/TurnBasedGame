using Assets.GameSystem.CardSystem.Scripts;
using GlobalData;
using Tool.UI;
using Tool.Utilities;
using UnityEngine.Events;

namespace Tips
{
    public static class TipsModule
    {
        private static UIManager uiMgr => UIManager.GetInstance();
        public static void ReComfirmTips(string title, string content, UnityAction comfirm, UnityAction cancel)
        {
            uiMgr.GetFromPool("ReConfirmTips", EuiLayer.TipsUI, (tips) =>
            {
                var reConfirmTips = tips as ReConfirmTips;
                reConfirmTips.Open(title, content, comfirm, cancel);
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
    }
}