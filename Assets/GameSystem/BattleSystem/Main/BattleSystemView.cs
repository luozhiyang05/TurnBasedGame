using Assets.GameSystem.MenuSystem.CharacterChose.Scripts;
using Assets.GameSystem.MenuSystem.LevelChose.Scripts;
using Framework;
using GameSystem.BattleSystem.Scripts;
using GameSystem.BattleSystem.Scripts.Unit;
using GameSystem.MVCTemplate;
using GlobalData;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms;

namespace GameSystem.BattleSystem.Main
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
            //设置关卡敌人信息
            SetLevelEnemies();
        }


        public override void OnHide()
        {
            base.OnHide();
        }

        public void SetLevelEnemies()
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

            //初始化玩家信息，并且保存到模型层
            var player = transform.Find("Main/absUnit/player");
            var playerBody = player.Find("body");
            var characterData = model.GetCharacterData();
            AddPlayerUnit(playerBody,characterData);
            player.gameObject.SetActive(true);

            //设置当前回合为玩家回合
            this.GetSystem<IBattleSystemModule>().SwitchPlayerTurn();
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
                case EnemyType.ENEMY_1:
                    absUnit = body.AddComponent<EnemyStore>();
                    break;
                case EnemyType.ENEMY_2:
                    absUnit = body.AddComponent<EnemyStore>();
                    break;
                default: break;
            }
            absUnit.InitSystem(this.GetSystem<IBattleSystemModule>());
            (absUnit as Enemy).InitData(id, enemyData);
            model.SetEnemyAbsUnit(absUnit);
        }

        /// <summary>
        /// 添加玩家角色脚本，并且初始化数据
        /// </summary>
        private void AddPlayerUnit(Transform playerBody,CharacterData characterData)
        {
            BattleSystemViewModel model = Model as BattleSystemViewModel;
            AbsUnit absUnit = null;
            switch(characterData.characterType)
            {
                case CharacterType.CAT:
                     absUnit = playerBody.AddComponent<PlayerCat>();
                    break;
                default: break;
            }
            absUnit.InitSystem(this.GetSystem<IBattleSystemModule>());
            (absUnit as Player).InitData(characterData);
            model.SetPlayerAbsUnit(absUnit);
        }
    }
}