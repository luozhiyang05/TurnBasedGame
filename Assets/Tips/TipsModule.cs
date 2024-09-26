using Tool.UI;
using UnityEngine.Events;

namespace Tips
{
    public static class TipsModule
    {
        private static UIManager uiMgr => UIManager.GetInstance();
        public static void ReComfirmTips(string title, string content,UnityAction comfirm, UnityAction cancel)
        {
            uiMgr.GetFromPool("ReConfirmTips", EuiLayer.TipsUI, (tips) =>
            {
                var reConfirmTips = tips as ReConfirmTips;
                reConfirmTips.SetData(title, content, comfirm, cancel);
                reConfirmTips.Open();
            });
        }

            public static void ComfirmTips(string title, string content,UnityAction comfirm)
        {
            uiMgr.GetFromPool("ComfirmTips", EuiLayer.TipsUI, (tips) =>
            {
                var confirmTips = tips as ComfirmTips;
                confirmTips.SetData(title, content, comfirm);
                confirmTips.Open();
            });
        }
    }
}