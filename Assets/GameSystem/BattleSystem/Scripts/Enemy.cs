using Framework;
using Tool.Mono;
using UnityEngine;

namespace GameSystem.BattleSystem.Scripts
{
    public class Enemy : AbsUnit, ICanGetSystem
    {
        public override void Enter()
        {
            Debug.Log("敌人回合开始");
            
            ActionKit.GetInstance().DelayTime(1f,Action);
        }

        public override void Action()
        {
            Debug.Log("敌人开始行动");
            ActionKit.GetInstance().DelayTime(1f, () =>
            {
                Debug.Log("敌人行动完毕");
                this.GetSystem<IBattleSystemModule>().MoreEnemyTurn();
            });
            
            //敌人回合结束取决于它本身
        }

        public override void Exit()
        {
            Debug.Log("敌人回合结束");
        }

        public IMgr Ins => Global.GetInstance();
    }
}