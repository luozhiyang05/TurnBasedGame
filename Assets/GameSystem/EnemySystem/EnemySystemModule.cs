using Assets.GameSystem.BattleSystem.Scripts;
using Framework;
using Tool.ResourceMgr;
using UnityEngine;
namespace Assets.GameSystem.EnemySystem
{
    public interface IEnemySystemModule: IModule
    {
        public int GetAllEnemiesCnt();
        public Sprite GetEnemyIcon(int id);
        public string GetEnemyName(int id);
        public string GetEnemyDesc(int id);
    }

    public class EnemySystemModule : AbsModule, IEnemySystemModule
    {
        private EnemiesSo enemiesSo;

        public int GetAllEnemiesCnt()
        {
            return enemiesSo.GetEnemyCnt();
        }

        public string GetEnemyDesc(int id)
        {
            return enemiesSo.GetEnemyDesc(id);
        }

        public Sprite GetEnemyIcon(int id)
        {
            return enemiesSo.GetEnemyIcon(id);
        }

        public string GetEnemyName(int id)
        {
            return enemiesSo.GetEnemyName(id);
        }

        protected override void OnInit()
        {
            enemiesSo = ResMgr.GetInstance().SyncLoad<EnemiesSo>("敌人库");
        }
    }
}