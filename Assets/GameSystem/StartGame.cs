using Assets.GameSystem.MenuSystem;
using Framework;
using UnityEngine;

namespace Assets.GameSystem
{
    public class StartGame : MonoBehaviour, ICanGetSystem
    {
        private void Start()
        {
            this.GetSystem<IMenuSystemModule>().ShowView();
        }

        public IMgr Ins => Global.GetInstance();
    }
}