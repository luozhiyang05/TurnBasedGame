using System;
using System.Collections.Generic;
using GameSystem.BattleSystem.Scripts.Effect;
using GlobalData;
using Tool.Mono;
using UnityEngine;

namespace GameSystem.BattleSystem.Scripts
{
    public abstract class AbsUnit : MonoBehaviour
    {
        public int id;
        public string unitName; //单位名称
        public int maxHp; //最大血量
        public int nowHp; //当前血量
        public int armor; //护盾
        private IBattleSystemModule _battleSystemModule;
        private readonly Queue<BaseEffect> _effQueue = new Queue<BaseEffect>();

        /// <summary>
        /// 初始化数据
        /// </summary>
        /// <param name="iBattleSystemModule"></param>
        public void InitSystem(IBattleSystemModule iBattleSystemModule)
        {
            this._battleSystemModule = iBattleSystemModule;
            nowHp = maxHp;
        }

        //是否死亡
        public bool IsDie()
        {
            return nowHp <= 0;
        }
        
        //添加效果
        public void AddEffect(BaseEffect addEff)
        {
            _effQueue.Enqueue(addEff);
        }
        
        //移除效果
        public void RemoveAllEffect()
        {
            _effQueue.Clear();
        }

        //回合开始结算
        public virtual void StartRoundSettle()
        {
            //对效果结算
            int effCount = _effQueue.Count;
            for (int i = 0; i < effCount; i++)
            {
                var eff = _effQueue.Dequeue();
                eff.StartRoundSettle();
                if (!eff.IsEnd())
                {
                    _effQueue.Enqueue(eff);
                }
            }
        }

        //回合开始结算结束
        protected void AfterStartRoundSettle()
        {
            //行动间隔
            _battleSystemModule.ActInternalTimeDelegate(Action);
        }
        
        //行动
        public abstract void Action();

        //行动结束
        protected void AfterAction()
        {
            //行动间隔
            _battleSystemModule.ActInternalTimeDelegate(() =>
            {
                //弹幕时间
                _battleSystemModule.BulletScreenTimeDelegate(ExitRound,"回合结束");
            });
        }

        //回合结束
        protected virtual void ExitRound()
        {
            //对效果结算
            int effCount = _effQueue.Count;
            for (int i = 0; i < effCount; i++)
            {
                var eff = _effQueue.Dequeue();
                eff.EndRoundSettle();
                if (!eff.IsEnd())
                {
                    _effQueue.Enqueue(eff);
                }
            }
        }
        
        //切换回合
        protected void SwitchRound()
        {
            //切换回合时间
            _battleSystemModule.SwitchTurnTimeDelegate(() =>
            {
                _battleSystemModule.SwitchRound();
            });
        }
    }
}