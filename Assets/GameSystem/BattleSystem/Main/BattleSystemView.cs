using Framework;
using GameSystem.BattleSystem.Scripts;
using GameSystem.CardSystem;
using GameSystem.CardSystem.Scripts.AtkCard;
using GameSystem.CardSystem.Scripts.DefenceCard;
using GameSystem.MVCTemplate;
using UnityEngine.UI;

namespace GameSystem.BattleSystem.Main
{
    public class BattleSystemView : BaseView
    {
        #region 自动生成UI组件区域，内部禁止手动更改！
		public Button Btn_attack;
		public Button Btn_defense;
		public Button Btn_exitRound;
        protected override void AutoInitUI()
        {
			Btn_attack = transform.Find("Main/Btn_attack").GetComponent<Button>();
			Btn_defense = transform.Find("Main/Btn_defense").GetComponent<Button>();
			Btn_exitRound = transform.Find("Main/Btn_exitRound").GetComponent<Button>();
        }
		#endregion 自动生成UI组件区域结束！

        /// <summary>
        /// 绑定model回调事件
        /// </summary>
        protected override void BindModelListener()
        {
        }

        private IBattleSystemModule _battleSystemModule;
        private ICardSystemModule _cardSystemModule;

        /// <summary>
        /// 初始化
        /// </summary>
        protected override void OnInit()
        {
            _battleSystemModule = this.GetSystem<IBattleSystemModule>();
            _cardSystemModule = this.GetSystem<ICardSystemModule>();

            Btn_attack.onClick.AddListener(() =>
            {
                var card = _cardSystemModule.GetCardFromUseCards<AttackCard>("普通攻击卡");
                var enemyUnit = _battleSystemModule.GetEnemyUnit(0);
                var playerUnit = _battleSystemModule.GetPlayerUnit();
                (playerUnit as Player)?.UseCard(card, enemyUnit);
            });
            Btn_defense.onClick.AddListener(() =>
            {
                var card = _cardSystemModule.GetCardFromUseCards<DefenceCard>("普通防御卡");
                var playerUnit = _battleSystemModule.GetPlayerUnit();
                (playerUnit as Player)?.UseCard(card, playerUnit);
            });
            Btn_exitRound.onClick.AddListener(() =>
            {
                (_battleSystemModule.GetPlayerUnit() as Player)?.EndRound();
            });
        }


        public override void OnShow()
        {
            base.OnShow();
        }

        public override void OnHide()
        {
            base.OnHide();
        }
    }
}