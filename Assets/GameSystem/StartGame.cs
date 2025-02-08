using System.Collections.Generic;
using Assets.GameSystem.BattleSystem.Scripts;
using Framework;
using GameSystem.BattleSystem;
using GameSystem.BattleSystem.Scripts;
using GameSystem.BattleSystem.Scripts.Unit;
using GameSystem.CardSystem;
using Tool.ResourceMgr;
using Tool.UI;
using Tool.Utilities;
using UnityEngine;

namespace GameSystem
{
    public class StartGame : MonoBehaviour, ICanGetSystem
    {
        private void Start()
        {
            // var enemys = new QArray<AbsUnit>(2);
            // enemys.Add(ResMgr.GetInstance().SyncLoad<GameObject>("Unit/Enemy_1").GetComponent<Enemy>());
            // // enemys.Add(ResMgr.GetInstance().SyncLoad<GameObject>("Unit/Enemy_2").GetComponent<Enemy>());
            // var player = ResMgr.GetInstance().SyncLoad<GameObject>("Unit/Player").GetComponent<Player>();

            var levelsSo = ResMgr.GetInstance().SyncLoad<LevelsSo>("Waves/关卡设置");
            var level_1 = levelsSo.GetLevelData(1);

            //打开战斗view
            this.GetSystem<IBattleSystemModule>().ShowView(level_1);

            //打开卡组vie
            this.GetSystem<ICardSystemModule>().ShowView();
        }

        private void Update()
        {
        }

        public IMgr Ins => Global.GetInstance();
    }
}