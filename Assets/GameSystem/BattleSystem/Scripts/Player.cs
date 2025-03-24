using Assets.GameSystem.CardSystem;
using Assets.GameSystem.FlyTextSystem;
using Assets.GameSystem.MenuSystem.CharacterChose.Scripts;
using Framework;
using GlobalData;
using Tool.ResourceMgr;
using Tool.Utilities;
using UnityEngine;

namespace Assets.GameSystem.BattleSystem.Scripts
{
    public abstract class Player : AbsUnit, ICanGetSystem
    {
        public int maxActPoint;
        public int nowActPoint;
        public int skillCardId;
        public CharacterData characterData;

        /// <summary>
        /// 回合开始时结算逻辑
        /// </summary>
        protected abstract void OnStartRoundSettle();

        public override void StartRoundSettle()
        {
            // 每回合最大行动点+1，直到到达角色最大行动点为止
            if (++maxActPoint >= characterData.maxActPoint)
                maxActPoint = characterData.maxActPoint;

            nowActPoint = maxActPoint;  //恢复行动点

            base.StartRoundSettle();    //结算单位身上的效果

            OnStartRoundSettle();       //具体重写的 回合开始时 逻辑

            this.GetSystem<ICardSystemModule>().UpdateHeadCardInSr();       //获取手牌
        }

        public override void Action()
        {
            Debug.Log("玩家开始出牌");
        }

        /// <summary>
        /// 手动结束回合
        /// </summary>
        public void EndRound()
        {
            // 回合结束 弹幕
            this.GetSystem<IFlyTextSystemModule>().FlyText(0, "battle_tip_1006", 1f, 0.5f);

            AfterAction();
        }

        /// <summary>
        /// 结算回合逻辑
        /// </summary>
        protected abstract void SettleRound();

        protected override void ExitRound()
        {
            base.ExitRound();   //结算单位身上的效果

            SettleRound();      //具体重写的 结算回合 逻辑

            SwitchRound(() =>   //回合切换
            {
                this.GetSystem<ICardSystemModule>().UpdateHeadCardInEr();   //丢弃手牌
            });
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
            this.characterData = characterData;
            unitName = characterData.characterType.ToString();
            maxHp.Value = characterData.maxHp;
            nowHp.Value = characterData.maxHp;
            maxActPoint = characterData.startMaxActCnt;
            nowActPoint = characterData.maxActPoint;
            skillCardId = characterData.skillCardId;

            // 动画
            for (int i = 1; i <= GameManager.UnitIconCnt; i++)
            {
                var sprite = ResMgr.GetInstance().SyncLoad<Sprite>(GameManager.GetIconPath(characterData.iconName, i));
                if (sprite != null)
                {
                    var animation2D = transform.Find("img_body").GetComponent<Animation2D>();
                    if (animation2D)
                    {
                        animation2D.SetSprites(sprite);
                        animation2D.SetFrames(GameManager.AnimationFrame);
                    }
                }
            }
        }
    }
}