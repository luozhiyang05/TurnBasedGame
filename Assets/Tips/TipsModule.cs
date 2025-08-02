using GameSystem.MVCTemplate;
using Tool.UI;
using UnityEngine.Events;

namespace Tips
{
    public static class TipsModule
    {
        private static string PATH = "Common/";
        private static UIManager uiMgr => UIManager.GetInstance();
        private static void OpenTips<T>(string tipsName,params object[] args) where T : BaseTips
        {
            uiMgr.GetFromPool(PATH + tipsName, EuiLayer.TipsUI, (view) =>
            {
                var tips = view as T;
                tips.Open(args);
            });
        }

        public static void ReComfirmTips(string title, string content, UnityAction comfirm, UnityAction cancel)
        {
            OpenTips<ReConfirmTips>("ReConfirmTips", title, content, comfirm, cancel);
        }

        public static void ComfirmTips(string title, string content, UnityAction comfirm)
        {
            OpenTips<ComfirmTips>("ComfirmTips", title, content, comfirm);
        }
    }
}