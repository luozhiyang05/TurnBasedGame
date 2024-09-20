using Framework;
using GameSystem.MVCTemplate;
using Tool.Single;
using Tool.UI;

namespace Tips
{
    public static class TipsModule 
    {
        private static UIManager uiMgr =>UIManager.GetInstance();
        public static void ReComfirmTips()
        {
            BaseTips baseTips = uiMgr.LoadTips<BaseTips>(nameof(ReConfirmTips));
            baseTips.Init(null);
            baseTips.OnShow();
        }
    }
}