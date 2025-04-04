using Assets.GameSystem.BattleSystem.Scripts;
using Assets.GameSystem.BattleSystem.Scripts.Effect;
using Tool.Utilities;
using UnityEngine.Events;

namespace Assets.GameSystem.CardSystem.Scripts.Cmd
{
    public class CardCmdData
    {
        public AbsUnit self;
        public AbsUnit target;

        #region 附带效果的信息
        public BaseEffect baseEffect;
        public int maxRoundCnt;
        #endregion
        
        public int param1;
        public int param2;
        public UnityAction<QArray<int>> action;
    }
}