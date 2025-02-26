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
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.GameSystem.BattleSystem.Main
{
    public class BattleSystemView : BaseView
    {
        #region 自动生成UI组件区域，内部禁止手动更改！
        protected override void AutoInitUI()
        {
        }
        #endregion 自动生成UI组件区域结束！

        /// <summary>
        /// 绑定model回调事件
        /// </summary>
        protected override void BindModelListener()
        {
            (Model as BattleSystemViewModel).SetNextWavaEnemiesDataAction(SetWaveEnemiesData);
            (Model as BattleSystemViewModel).SetPassLevelAction(PassLevel);
            (Model as BattleSystemViewModel).SetLoseLevelAction(LoseLevel);
        }


        /// <summary>
        /// 初始化
        /// </summary>
        protected override void OnInit()
        {

        }


        public override void OnShow()
        {
            base.OnShow();

            //设置当前波次的敌人信息
            SetWaveEnemiesData();

            //设置玩家信息
            SetPlayerData();

            //设置当前回合为玩家回合
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
            switch (enemyData.enemyType)
            {
                case EnemyType.史莱姆:
                    absUnit = body.AddComponent<EnemySlime>();
                    break;
                case EnemyType.石头怪:
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
                case CharacterType.猫猫:
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
    }
}