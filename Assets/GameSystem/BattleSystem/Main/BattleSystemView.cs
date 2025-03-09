using Assets.GameSystem.BattleSystem.Scripts;
using Assets.GameSystem.BattleSystem.Scripts.Unit;
using Assets.GameSystem.BattleSystem.Scripts.Unit.EnemyUnit;
using Assets.GameSystem.BattleSystem.Scripts.Unit.PlayerUnit;
using Assets.GameSystem.MenuSystem;
using Assets.GameSystem.MenuSystem.CharacterChose.Scripts;
using Assets.GameSystem.MenuSystem.LevelChose.Scripts;
using Framework;
using GameSystem.MVCTemplate;
using GlobalData;
using Tips;
using Tool.UI;
using UIComponents;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.GameSystem.BattleSystem.Main
{
    public class BattleSystemView : BaseView
    {
        #region 自动生成UI组件区域，内部禁止手动更改！
		public CButton Btn_setting;
		public CButton Btn_exitRound;
		public Text Txt_waveCnt;
		public Text Txt_roundCnt;
        protected override void AutoInitUI()
        {
			Btn_setting = transform.Find("Main/Btn_setting").GetComponent<CButton>();
			Btn_exitRound = transform.Find("Main/Btn_exitRound").GetComponent<CButton>();
			Txt_waveCnt = transform.Find("Main/BattleInfos/Txt_waveCnt").GetComponent<Text>();
			Txt_roundCnt = transform.Find("Main/BattleInfos/Txt_roundCnt").GetComponent<Text>();
        }
		#endregion 自动生成UI组件区域结束！

        private IBattleSystemModule _battleSystemModule;

        /// <summary>
        /// 绑定model回调事件
        /// </summary>
        protected override void BindModelListener()
        {
            (Model as BattleSystemViewModel).SetNextWavaEnemiesDataAction(SetWaveEnemiesData);
            (Model as BattleSystemViewModel).SetPassLevelAction(PassLevel);
            (Model as BattleSystemViewModel).SetLoseLevelAction(LoseLevel);
            (Model as BattleSystemViewModel).SetUpdateRoundCntTxtAction(UpdateRoundCntTxt);
            (Model as BattleSystemViewModel).SetSetUseCardLimitAction(SetUseCardLimit);
        }


        /// <summary>
        /// 初始化
        /// </summary>
        protected override void OnInit()
        {
            // 获取系统
            _battleSystemModule = this.GetSystem<IBattleSystemModule>();

            //文本
            Btn_exitRound.Label.text = GameManager.GetText("battle_tip_1002");
            Btn_setting.Label.text = GameManager.GetText("menu_1002");
            Txt_waveCnt.text = GameManager.GetText("battle_tip_1010");
            Txt_roundCnt.text = GameManager.GetText("battle_tip_1011") + 0;

            //按钮
            Btn_exitRound.onClick.AddListener(() =>
            {
                (_battleSystemModule.GetPlayerUnit() as Player)?.EndRound();
                (Model as BattleSystemViewModel).BanUseCard();  //玩家推出回合后，禁止使用卡牌
            });
            Btn_setting.onClick.AddListener(() =>
            {
                this.GetSystem<IMenuSystemModule>().ShowSettingView(Tool.UI.EuiLayer.GameUI);
            });
        }


        public override void OnShow()
        {
            base.OnShow();

            //设置当前波次的敌人信息
            SetWaveEnemiesData();

            //设置玩家信息
            SetPlayerData();

            //设置当前回合为玩家回合
            this.GetSystem<IBattleSystemModule>().SetIsStartBattle(true);
            this.GetSystem<IBattleSystemModule>().SwitchPlayerTurn();
        }


        public override void OnHide()
        {
            base.OnHide();
        }


        /// <summary>
        /// 设置玩家信息
        /// </summary>
        public void SetPlayerData()
        {
            //初始化玩家信息，并且保存到模型层
            BattleSystemViewModel model = Model as BattleSystemViewModel;
            var player = transform.Find("Main/absUnit/player");
            var playerBody = player.Find("body");
            var characterData = model.GetCharacterData();
            AddPlayerUnit(playerBody, characterData);
            player.gameObject.SetActive(true);
        }

        /// <summary>
        /// 设置当前波次的敌人信息
        /// </summary>
        public void SetWaveEnemiesData()
        {
            //初始化敌人信息，并且保存到模型层
            BattleSystemViewModel model = Model as BattleSystemViewModel;
            WavasData wava = model.GetNowWava();
            Transform enemiesTrans = transform.Find("Main/absUnit/enemies");
            for (int i = 0; i < wava.enemies.Count; i++)
            {
                var enemy = enemiesTrans.GetChild(i);
                AddEnemyUnit(enemy, i + 1, wava.enemies[i]);
                enemy.gameObject.SetActive(true);
            }

            // 设置文本提示
            Txt_waveCnt.text = GameManager.GetText("battle_tip_1010") + model.GetNowWaveIndex() + "/" + model.GetWaveCnt();
        }

        /// <summary>
        /// 添加敌人单位脚本，并且初始化数据
        /// </summary>
        /// <param name="enemy"></param>
        /// <param name="id"></param>
        /// <param name="enemyData"></param>
        private void AddEnemyUnit(Transform enemy, int id, EnemyData enemyData)
        {
            //保存关卡敌人脚本到model
            BattleSystemViewModel model = Model as BattleSystemViewModel;
            AbsUnit absUnit = null;
            var body = enemy.Find("body");
            var oldAbsUnit = body.GetComponent<AbsUnit>();
            if (oldAbsUnit) Destroy(oldAbsUnit);
            switch (enemyData.enemyType)
            {
                case EnemyType.enemy_slime:
                    absUnit = body.AddComponent<EnemySlime>();
                    break;
                case EnemyType.enemy_store:
                    absUnit = body.AddComponent<EnemyStore>();
                    break;
                default: break;
            }
            (absUnit as Enemy).InitData(id, enemyData);
            model.SetEnemyAbsUnit(absUnit);
        }

        /// <summary>
        /// 添加玩家角色脚本，并且初始化数据
        /// </summary>
        private void AddPlayerUnit(Transform playerBody, CharacterData characterData)
        {
            BattleSystemViewModel model = Model as BattleSystemViewModel;
            AbsUnit absUnit = null;
            switch (characterData.characterType)
            {
                case CharacterType.character_cat:
                    absUnit = playerBody.AddComponent<PlayerCat>();
                    break;
                default:
                    absUnit = playerBody.AddComponent<PlayerCat>();
                    break;
            }
            (absUnit as Player).InitData(characterData);
            model.SetPlayerAbsUnit(absUnit);
        }

        /// <summary>
        /// 通关提示
        /// </summary>
        private void PassLevel()
        {
            //TODO：通关提示
            TipsModule.ComfirmTips("tips_1001", "win_1001", () =>
            {
                UIManager.GetInstance().CloseAllViewByLayer(EuiLayer.GameUI);
                this.GetSystem<IMenuSystemModule>().ShowView();
            });
        }

        /// <summary>
        /// 失败提示
        /// </summary>
        private void LoseLevel()
        {
            //TODO：失败提示
            TipsModule.ComfirmTips("tips_1002", "lose_1001", () =>
            {
                UIManager.GetInstance().CloseAllViewByLayer(EuiLayer.GameUI);
                this.GetSystem<IMenuSystemModule>().ShowView();
            });
        }

        /// <summary>
        /// 更新回合数文本
        /// </summary>
        /// <param name="nowRoundCnt"></param>
        private void UpdateRoundCntTxt(int nowRoundCnt)
        {
            Txt_roundCnt.text = GameManager.GetText("battle_tip_1011") + nowRoundCnt;
        }

        private void SetUseCardLimit(bool value)
        {
            Btn_exitRound.interactable = value;
        }
    }
}