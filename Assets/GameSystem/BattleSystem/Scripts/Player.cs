using Framework;
using Tool.Mono;
using UnityEngine;

namespace GameSystem.BattleSystem.Scripts
{
    public class Player :  AbsUnit, ICanGetSystem
    {
        
        public override void Enter()
        {
            Debug.Log("玩家回合开始");
            //结算回合开始前buff效果
            armor = 0; 
            
            //回合开始时结算完后执行Action
            ActionKit.GetInstance().DelayTime(1f,Action);
        }

        public override void Action()
        {
            Debug.Log("玩家开始行动");
            //玩家回合结束取决于玩家选取
        }

        public override void Exit()
        {
            Debug.Log("玩家回合结束");
        }

        public IMgr Ins => Global.GetInstance();
    }
}