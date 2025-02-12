using Framework;
using GameSystem.MenuSystem;
using UnityEngine;

namespace GameSystem
{
    public class StartGame : MonoBehaviour, ICanGetSystem
    {
        private void Start()
        {
            // // var enemys = new QArray<AbsUnit>(2);
            // // enemys.Add(ResMgr.GetInstance().SyncLoad<GameObject>("Unit/Enemy_1").GetComponent<Enemy>());
            // // // enemys.Add(ResMgr.GetInstance().SyncLoad<GameObject>("Unit/Enemy_2").GetComponent<Enemy>());
            // // var player = ResMgr.GetInstance().SyncLoad<GameObject>("Unit/Player").GetComponent<Player>();

            // var levelsSo = ResMgr.GetInstance().SyncLoad<LevelsSo>("Waves/关卡设置");
            // var level_1 = levelsSo.GetLevelData(1);

            // //打开战斗view
            // this.GetSystem<IBattleSystemModule>().ShowView(level_1);

            // //打开卡组vie
            // this.GetSystem<ICardSystemModule>().ShowView();
            this.GetSystem<IMenuSystemModule>().ShowView();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                this.GetSystem<IMenuSystemModule>().ShowView();
            }
        }

        public IMgr Ins => Global.GetInstance();
    }
}