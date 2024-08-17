using Framework;
using GlobalData;
using Tool.Mono;
using UnityEngine;

namespace GameSystem.BattleSystem.Scripts
{
    public abstract class Player : AbsUnit, ICanGetSystem
    {
        public override void Enter(ETurnBased eTurnBased)
        {
            Debug.Log("玩家回合开始");
            ETurnBased = eTurnBased;
            OnEnter();

            //结算回合开始前buff效果
            armor = 0;
        }

        protected abstract void OnEnter();

        public override void Action()
        {
            Debug.Log("玩家行动");
            base.Action();
            OnAction();
            //玩家回合结束取决于玩家选取
        }

        protected abstract void OnAction();

        public override void Exit()
        {
            Debug.Log("玩家回合结束");
            base.Exit();
            OnExit();
        }

        protected abstract void OnExit();
        
        public IMgr Ins => Global.GetInstance();
    }
}