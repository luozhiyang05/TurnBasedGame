using System.Collections.Generic;
using Framework;
using GameSystem.BattleSystem;
using GameSystem.BattleSystem.Scripts;
using GameSystem.CardSystem;
using Tool.ResourceMgr;
using Tool.UI;
using UnityEngine;

namespace GameSystem
{
    public class StartGame : MonoBehaviour, ICanGetSystem
    {
        private void Start()
        {
            //打开战斗view
            this.GetSystem<IBattleSystemModule>()
                .StartGame(ResMgr.GetInstance().SyncLoad<GameObject>("Unit/Player").GetComponent<Player>(),
                    new List<AbsUnit>()
                    {
                        ResMgr.GetInstance().SyncLoad<GameObject>("Unit/Enemy_1").GetComponent<Enemy>(),
                        ResMgr.GetInstance().SyncLoad<GameObject>("Unit/Enemy_2").GetComponent<Enemy>(),
                    });

            //打开卡组vie
            this.GetSystem<ICardSystemModule>().ShowView();
        }

        private void Update()
        {
        }

        public IMgr Ins => Global.GetInstance();
    }
}