using Framework;
using GameSystem.MVCTemplate;
using Tool.Single;

namespace Tips
{
    public static class TipsModule 
    {
        public static void ReComfirmTips()
        {
            BaseTips baseTips = BaseTips.New(nameof(ReConfirmTips));
            baseTips.Init(null);
            baseTips.OnShow();
        }
    }
}