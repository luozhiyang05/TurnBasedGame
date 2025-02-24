using Assets.GameSystem.CardSystem;
using Assets.GameSystem.MenuSystem.CharacterChose.Scripts;
using Framework;
using UnityEngine;

namespace Assets.GameSystem.BattleSystem.Scripts
{
    public abstract class Player : AbsUnit, ICanGetSystem
    {
        public int maxActPoint;
        public int nowActPoint;
        public int skillCardId;

        public bool canAction = false;

        /// <summary>
        /// 回合开始时结算逻辑
        /// </summary>
        protected abstract void OnStartRoundSettle();

        public override void StartRoundSettle()
        {
            nowActPoint = maxActPoint;  //恢复行动点

            base.StartRoundSettle();    //结算单位身上的效果

            OnStartRoundSettle();       //具体重写的 回合开始时 逻辑
            
            this.GetSystem<ICardSystemModule>().UpdateHeadCardInSr();       //获取手牌

            canAction = true;
        }

        public override void Action()
        {
            Debug.Log("玩家开始出牌");
            canAction = true;
        }

        /// <summary>
        /// 手动结束回合
        /// </summary>
        public void EndRound() => AfterAction();

        /// <summary>
        /// 结算回合逻辑
        /// </summary>
        protected abstract void SettleRound();

        protected override void ExitRound()
        {
            base.ExitRound();   //结算单位身上的效果

            SettleRound();      //具体重写的 结算回合 逻辑
            
            this.GetSystem<ICardSystemModule>().UpdateHeadCardInEr();   //丢弃手牌

            SwitchRound();      //回合切换

            canAction = false;
        }

        /// <summary>
        /// 修改行动点
        /// </summary>
        /// <param name="mPoint"></param>
        /// <returns></returns>
        public void ModifyActPoint(int mPoint)
        {
            nowActPoint += mPoint;
        }

          /// <summary>
        /// 初始化玩家数据
        /// </summary>
        public void InitData(CharacterData characterData)
        {
            unitName = characterData.characterType.ToString();
            maxHp.Value = characterData.maxHp;
            nowHp.Value = characterData.maxHp;
            maxActPoint = characterData.maxActPoint;
            nowActPoint = characterData.maxActPoint;
            skillCardId = characterData.skillCardId;
        }


        public IMgr Ins => Global.GetInstance();
    }
}