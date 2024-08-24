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
            this.GetSystem<IBattleSystemModule>()
                .StartGame(ResMgr.GetInstance().SyncLoad<GameObject>("Unit/Player").GetComponent<Player>(),
                    new List<AbsUnit>()
                    {
                        ResMgr.GetInstance().SyncLoad<GameObject>("Unit/Enemy_1").GetComponent<Enemy>(),
                        ResMgr.GetInstance().SyncLoad<GameObject>("Unit/Enemy_2").GetComponent<Enemy>(),
                    });

            this.GetSystem<ICardSystemModule>().LoadUseCards();
        }

        private void Update()
        {
        }

        public IMgr Ins => Global.GetInstance();
    }
}