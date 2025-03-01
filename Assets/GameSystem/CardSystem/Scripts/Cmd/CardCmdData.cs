using Assets.GameSystem.BattleSystem.Scripts;
using Tool.Utilities;
using UnityEngine.Events;

namespace Assets.GameSystem.CardSystem.Scripts.Cmd
{
    public class CardCmdData
    {
        public AbsUnit self;
        public AbsUnit target;
        public int param1;
        public int param2;
        public UnityAction<QArray<int>> action;
    }
}