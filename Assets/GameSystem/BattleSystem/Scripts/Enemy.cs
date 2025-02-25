using Assets.GameSystem.MenuSystem.LevelChose.Scripts;
using Assets.GameSystem.SkillSystem;
using Assets.GameSystem.SkillSystem.Scripts.Cmd;
using Framework;
using Tool.ResourceMgr;
using Tool.Utilities.Bindery;

namespace Assets.GameSystem.BattleSystem.Scripts
{
    public abstract class Enemy : AbsUnit, ICanGetSystem
    {
        public EnemyData enemyData;
        public ValueBindery<int> actCnt = new ValueBindery<int>();
        public ValueBindery<int> atk = new ValueBindery<int>();

        /// <summary>
        /// 回合开始时结算逻辑
        /// </summary>
        protected abstract void OnStartRoundSettle();
        public override void StartRoundSettle()
        {
            base.StartRoundSettle();    //结算单位身上的效果

            OnStartRoundSettle();       //具体重写的 回合开始时 逻辑

            AfterStartRoundSettle();    //弹幕时间，结束后进入 行动逻辑
        }

        /// <summary>
        /// 使用技能
        /// </summary>
        /// <returns></returns>
        private bool UseSkill()
        {
            var skillData = ResMgr.GetInstance().SyncLoad<SkillsSo>("技能库").GetSkillDataById(enemyData.skillId);
            if(skillData==null) return false;
            if(actCnt.Value<skillData.needActCnt) return false;
            UseSkill(skillData);
            return true;
        }

        private void UseSkill(SkillData skillData)
        {
            switch (skillData.id)
            {
                case 1001:
                    var skillDataPacking = new SkillDataPacking(skillData, this, skillData.useType == UseType.all ? null : (skillData.useType == UseType.target ? _battleSystemModule.GetPlayerUnit() : this));
                    this.SendCmd<AddSelfArmorSkillCmd, SkillDataPacking>(skillDataPacking);
                    break;
            }
            // 根据技能Id，实例化技能命令
            // 根据技能类型，实例化一个skillDataPacking，填入user，target
            // 发送命令
            actCnt.Value = 0;
        }

        /// <summary>
        /// 行动逻辑
        /// </summary>
        protected abstract void OnAction();
        public override void Action()
        {
            actCnt.Value++;

            if (!UseSkill())    // 技能和行动二选一
            {
                OnAction();     //具体重写的 单位行动 逻辑
            }

            AfterAction();  //弹幕时间，结束后进入 结算回合 逻辑
        }


        /// <summary>
        /// 结算回合逻辑
        /// </summary>
        protected abstract void SettleRound();
        protected override void ExitRound()
        {
            base.ExitRound();   //结算单位身上的效果

            SettleRound();      //具体重写的 结算回合 逻辑

            SwitchRound();      //回合切换
        }

        /// <summary>
        /// 初始化敌人数据
        /// </summary>
        public void InitData(int id,EnemyData enemyData)
        {
            base.id = id;

            this.enemyData = enemyData;

            unitName = enemyData.enemyType.ToString();
            maxHp.Value = enemyData.maxHp;
            nowHp.Value = maxHp.Value;
            armor.Value = enemyData.maxArmor;
            atk.Value = enemyData.atk;
        }
    }
}