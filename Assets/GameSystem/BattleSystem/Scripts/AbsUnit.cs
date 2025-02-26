using System;
using System.Collections.Generic;
using Assets.GameSystem.BattleSystem;
using Assets.GameSystem.BattleSystem.Scripts.Effect;
using Assets.GameSystem.SkillSystem;
using Framework;
using GlobalData;
using Tool.Mono;
using Tool.Utilities;
using Tool.Utilities.Bindery;
using Tool.Utilities.Events;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.GameSystem.BattleSystem.Scripts
{
    public class UnitInfoPacking
    {
        public string iconName;
        public string unitName;
        public int maxHp;
        public int nowHp;
        public int armor;
        public int atk;
        public int skillId;
        public QArray<int> effectIds;
        public UnitInfoPacking(string iconName, string unitName, int maxHp, int nowHp, int armor, int atk,int skillId, QArray<int> effectIds)
        {
            this.iconName = iconName;
            this.unitName = unitName;
            this.maxHp = maxHp;
            this.nowHp = nowHp;
            this.armor = armor;
            this.atk = atk;
            this.skillId = skillId;
            this.effectIds = effectIds;
        }
    }
    public abstract class AbsUnit : MonoBehaviour, ICanSendCmd, ICanGetSystem
    {
        public int id;
        public string unitName; //单位名称
        public ValueBindery<int> maxHp = new ValueBindery<int>(5); //最大血量
        public ValueBindery<int> nowHp = new ValueBindery<int>(5); //当前血量
        public ValueBindery<int> armor = new ValueBindery<int>(); //护盾
        protected IBattleSystemModule _battleSystemModule;
        protected ISkillSystemModule _skillSystemModule;
        protected readonly QArray<BaseEffect> _effQueue = new QArray<BaseEffect>(1);
        protected Slider _hpBar;
        protected Text _txtArmor;

        public virtual void Awake()
        {
            // 获取系统
            _battleSystemModule = this.GetSystem<IBattleSystemModule>();
            _skillSystemModule = this.GetSystem<ISkillSystemModule>();

            // 获取组件
            _hpBar = transform.Find("hp_bar").GetComponent<Slider>();
            _txtArmor = transform.Find("armor/txt_armor").GetComponent<Text>();

            // 显示数据
            _hpBar.value = (float)nowHp.Value / maxHp.Value;
            _txtArmor.text = armor.Value.ToString();

            //绑定事件
            nowHp.OnRegister(value =>
            {
                _hpBar.value = (float)value / maxHp.Value;
                //面板刷新
                DisplayInfo();

                //如果当前血量已死亡，先判断有无亡语技能（类似复活）
                if (IsDie())
                {
                    //对效果结算，用于触发亡语效果
                    for (int i = 0; i < _effQueue.Count; i++)
                    {
                        if (_effQueue[i].isDieEff)
                        {
                            _effQueue[i].DieEffectSettle();
                            _effQueue.Remove(_effQueue[i]);
                        }
                    }
                }

                // 亡语效果判断完后重新判断是否已经死亡，是的话则发布事件
                if (IsDie())
                {
                    //分发事件，单位死亡
                    EventsHandle.EventTrigger(EventsNameConst.ABSUNIT_DIE, this);
                    Destroy(gameObject.GetComponent<AbsUnit>());
                    gameObject.transform.parent.gameObject.SetActive(false);
                }
            });
            armor.OnRegister(value =>
            {
                _txtArmor.text = value.ToString();
                //面板刷新
                DisplayInfo();
            });

            //面板展示
            DisplayInfo();
        }

        #region 收到卡牌影响逻辑

        //是否死亡
        public bool IsDie()
        {
            return nowHp.Value <= 0;
        }

        //添加效果
        public void AddEffect(BaseEffect addEff)
        {
            _effQueue.Add(addEff);
        }

        //移除效果
        public void RemoveAllEffect()
        {
            _effQueue.Clear();
        }

        #endregion

        #region 回合逻辑
        /// <summary>
        /// 回合开始时，结算单位身上的效果
        /// 具体单位大类重写，可添加额外的 回合开始时 的逻辑
        /// </summary>
        public virtual void StartRoundSettle()
        {
            //对效果结算
            int effCount = _effQueue.Count;
            for (int i = 0; i < effCount; i++)
            {
                var eff = _effQueue.GetFromHead();
                eff.StartRoundSettle();
                if (!eff.IsEnd())
                {
                    _effQueue.Add(eff);
                }
            }
        }

        /// <summary>
        /// 回合开始时的逻辑结算结束，间隔时间后 单位行动
        /// </summary>
        protected void AfterStartRoundSettle()
        {
            //玩家当前没有死亡，则继续行动
            if (!_battleSystemModule.GetPlayerUnit().IsDie())
            {
                //行动间隔
                _battleSystemModule.ActInternalTimeDelegate(() =>
                {
                    //弹幕时间
                    _battleSystemModule.BulletScreenTimeDelegate(Action, "开始行动");
                });
            }
        }

        /// <summary>
        /// 抽象 单位行动 方法
        /// 需要子类重写
        /// </summary>
        public abstract void Action();

        /// <summary>
        /// 单位行动 结束，间隔后退出回合
        /// </summary>
        protected void AfterAction()
        {
            //玩家当前没有死亡，则继续行动
            if (!_battleSystemModule.GetPlayerUnit().IsDie())
            {
                //行动间隔
                _battleSystemModule.ActInternalTimeDelegate(() =>
                {
                    //弹幕时间
                    _battleSystemModule.BulletScreenTimeDelegate(ExitRound, "行动结束");
                });
            }
        }

        /// <summary>
        /// 退出回合时，结算单位身上的效果
        /// 具体单位类可以重写，可添加额外 退出回合时 逻辑
        /// </summary>
        protected virtual void ExitRound()
        {
            //对效果结算
            int effCount = _effQueue.Count;
            for (int i = 0; i < effCount; i++)
            {
                var eff = _effQueue.GetFromHead();
                eff.EndRoundSettle();
                if (!eff.IsEnd())
                {
                    _effQueue.Add(eff);
                }
            }
        }

        /// <summary>
        /// 该单位完全行动后，切换回合
        /// </summary>
        protected void SwitchRound()
        {
            //切换回合时间
            _battleSystemModule.SwitchTurnTimeDelegate(() =>
            {
                _battleSystemModule.SwitchRound();
            });
        }

        #endregion

        #region  inspector面板展示
        [Header("面板展示")]
        [SerializeField] private int _maxHp;
        [SerializeField] private int _nowHp;
        [SerializeField] private int _armor;

        public IMgr Ins => Global.GetInstance();

        private void DisplayInfo()
        {
            _maxHp = maxHp.Value;
            _nowHp = nowHp.Value;
            _armor = armor.Value;
        }
        #endregion
    }
}