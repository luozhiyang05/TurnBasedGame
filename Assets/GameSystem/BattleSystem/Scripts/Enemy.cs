using Assets.GameSystem.BattleSystem.Scripts.Effect;
using Framework;
using Tips;
using Tool.Utilities;
using Tool.Utilities.Bindery;
using Unity.VisualScripting;
using UnityEngine.UI;

namespace Assets.GameSystem.BattleSystem.Scripts
{
    public abstract class Enemy : AbsUnit, ICanGetSystem
    {
        public QArray<int> usedSkillIds = new QArray<int>();
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
        /// 行动逻辑
        /// </summary>
        protected abstract void OnAction();
        public override void Action()
        {
            actCnt.Value++;

            if (_skillSystemModule.CheckIsHadSkill(usedSkillIds, enemyData.skillId, actCnt.Value))    // 技能和行动二选一
            {
                _skillSystemModule.UseSkill(enemyData.skillId, this);
                usedSkillIds.Add(enemyData.skillId);
                actCnt.Value = 0;
            }
            else
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
        public void InitData(int id, EnemyData enemyData)
        {
            base.id = id;

            this.enemyData = enemyData;

            unitName = enemyData.enemyType.ToString();
            maxHp.Value = enemyData.maxHp;
            nowHp.Value = maxHp.Value;
            armor.Value = enemyData.maxArmor;
            atk.Value = enemyData.atk;

            // 绑定点击事件，打开UnitInfoTips面板
            var img_body = transform.Find("img_body");
            var btn_body = img_body.GetComponent<Button>();
            btn_body.onClick.RemoveAllListeners();
            btn_body.onClick.AddListener(() =>
            {
                // 获取所有效果的id
                var effectIds = new QArray<int>();
                foreach (BaseEffect eff in _effQueue)
                {
                    effectIds.Add(eff.id);
                }

                // 打包数据传递
                TipsModule.UnitInfoTips(new UnitInfoPacking("", enemyData.enemyType.ToSafeString(), enemyData.maxHp, nowHp.Value, armor.Value, enemyData.atk, enemyData.skillId, effectIds));
            });
        }
    }
}