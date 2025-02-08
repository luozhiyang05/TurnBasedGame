using Assets.GameSystem.BattleSystem.Scripts;
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
            //给关卡敌人挂在脚本
            BattleSystemViewModel model = Model as BattleSystemViewModel;
            WavasData wava = model.GetNowWava();
            Transform enemiesTrans = transform.Find("Main/absUnit/enemies");
            for (int i = 0; i < wava.enemyTypes.Count; i++)
            {
                var enemy = enemiesTrans.GetChild(i);
                AddAbsUnit(enemy, wava.enemyTypes[i]);
                enemy.gameObject.SetActive(true);
            }

            //保存玩家脚本到model
            var player = transform.Find("Main/absUnit/player/body").GetComponent<AbsUnit>();
            player.InitSystem(this.GetSystem<IBattleSystemModule>());
            model.SetPlayerAbsUnit(player);

            //设置当前回合为玩家回合
            this.GetSystem<IBattleSystemModule>().SwitchPlayerTurn();
        }

        private void AddAbsUnit(Transform enemy, EnemyType enemyType)
        {
            //保存关卡敌人脚本到model
            BattleSystemViewModel model = Model as BattleSystemViewModel;
            AbsUnit absUnit;
            var body = enemy.Find("body");
            switch (enemyType)
            {
                case EnemyType.ENEMY_1:
                    body.AddComponent<EnemyStore>();
                    absUnit = body.GetComponent<AbsUnit>();
                    absUnit.InitSystem(this.GetSystem<IBattleSystemModule>());
                    model.SetEnemyAbsUnit(absUnit);
                    break;
                case EnemyType.ENEMY_2:
                    body.AddComponent<EnemyStore>();
                    absUnit = body.GetComponent<AbsUnit>();
                    absUnit.InitSystem(this.GetSystem<IBattleSystemModule>());
                    model.SetEnemyAbsUnit(absUnit);
                    break;

                default: break;
            }
        }
    }
}