using System;
using System.Collections.Generic;
using GameSystem.BattleSystem.Scripts.Effect;
using GlobalData;
using Tool.Mono;
using Tool.Utilities;
using Tool.Utilities.Bindery;
using Tool.Utilities.Events;
using UnityEngine;
using UnityEngine.UI;

namespace GameSystem.BattleSystem.Scripts
{
    public abstract class AbsUnit : MonoBehaviour
    {
        public int id;
        public string unitName; //单位名称
        public ValueBindery<int> maxHp = new ValueBindery<int>(5); //最大血量
        public ValueBindery<int> nowHp = new ValueBindery<int>(5); //当前血量
        public ValueBindery<int> armor = new ValueBindery<int>(); //护盾
        private IBattleSystemModule _battleSystemModule;
        private readonly QArray<BaseEffect> _effQueue = new QArray<BaseEffect>(1);
        private Image _imgHealth;
        private Text _txtArmor;

        public virtual void Awake()
        {
            _imgHealth = transform.Find("UnitCanvas/healthBar/img_Health").GetComponent<Image>();
            _txtArmor = transform.Find("UnitCanvas/img_armor/txt_cnt").GetComponent<Text>();

            nowHp.Value = maxHp.Value;
            _imgHealth.fillAmount = (float)nowHp.Value / maxHp.Value;
            _txtArmor.text = armor.Value.ToString();

            //绑定事件
            nowHp.OnRegister(value =>
            {
                _imgHealth.fillAmount = (float)value / maxHp.Value;
                if (IsDie())
                {
                    //分发事件，单位死亡
                    EventsHandle.EventTrigger(EventsNameConst.ABSUNIT_DIE, this);
                    Destroy(gameObject);
                }
            });
            armor.OnRegister(value =>
            {
                _txtArmor.text = value.ToString();
            });
        }

        /// <summary>
        /// 初始化数据
        /// </summary>
        /// <param name="iBattleSystemModule"></param>
        public void InitSystem(IBattleSystemModule iBattleSystemModule)
        {
            this._battleSystemModule = iBattleSystemModule;
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
        //回合开始结算
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
                _battleSystemModule.BulletScreenTimeDelegate(ExitRound, "回合结束");
            });
        }

        //回合结束
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

        //切换回合
        protected void SwitchRound()
        {
            //切换回合时间
            _battleSystemModule.SwitchTurnTimeDelegate(() =>
            {
                _battleSystemModule.SwitchRound();
            });
        }
        #endregion
    }
}