using Framework;
using GameSystem.MVCTemplate;
using Tool.Single;
using Tool.UI;

namespace Tips
{
    public static class TipsModule
    {
        private static UIManager uiMgr => UIManager.GetInstance();
        public static void ReComfirmTips()
        {
            uiMgr.GetFromPool("ReConfirmTips", EuiLayer.GameUI, (tips) =>
            {
                (tips as ReConfirmTips).Open();
            });
        }
    }
}