using Tool.Mono;
using UnityEngine;

namespace GameSystem.BattleSystem.Scripts
{
    public abstract class AbsUnit : MonoBehaviour
    {
        public int id;
        public string unitName; //单位名称
        public float maxHp;     //最大血量
        public float nowHp;     //当前血量
        public float atk;       //攻击
        public float armor;     //护盾
        private IBattleSystemModule _battleSystem;

        /// <summary>
        /// 内部使用延迟切换状态
        /// </summary>
        /// <param name="self"></param>
        /// <param name="time"></param>
        /// <param name="eTurnBased"></param>
        protected void DelaySwitchState(AbsUnit self, float time, ETurnBased eTurnBased)
        {
            ActionKit.GetInstance().DelayTime(time, () => { _battleSystem.SwitchState(self, eTurnBased); });
        }

        /// <summary>
        /// 初始化数据
        /// </summary>
        /// <param name="iBattleSystemModule"></param>
        public void InitSystem(IBattleSystemModule iBattleSystemModule)
        {
            this._battleSystem = iBattleSystemModule;
            nowHp = maxHp;
        }

        //是否死亡
        public bool IsDie()
        {
            return nowHp <= 0;
        }

        //进入回合
        public virtual void Enter()
        {
        }

        //单位行动
        public virtual void Action()
        {
        }

        //退出回合
        public virtual void Exit()
        {
        }
    }
}