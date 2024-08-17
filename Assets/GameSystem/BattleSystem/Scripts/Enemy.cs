using System;
using Framework;
using Tool.Mono;
using UnityEngine;

namespace GameSystem.BattleSystem.Scripts
{
    public abstract class Enemy : AbsUnit, ICanGetSystem
    {
        public override void Enter(ETurnBased eTurnBased)
        {
            Debug.Log($"{gameObject.name}回合开始");
            OnEnter();
            base.Enter(eTurnBased);
        }

        protected abstract void OnEnter();

        public override void Action()
        {
            Debug.Log($"{gameObject.name}行动");
            OnAction();
            base.Action();
        }

        protected abstract void OnAction();


        public override void Exit()
        {
            Debug.Log($"{gameObject.name}回合结束");
            OnExit();
            base.Exit();
        }

        protected abstract void OnExit();

        public IMgr Ins => Global.GetInstance();
    }
}