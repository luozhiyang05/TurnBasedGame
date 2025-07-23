using Assets.GameSystem.CToggleGroupSystem;
using Assets.GameSystem.RedPointSystem;
using Framework;
using Tool.Mono;
using UnityEngine;

namespace GameSystem
{
    public class RedPointStartGame : MonoBehaviour, ICanGetSystem
    {
        private void Start()
        {
            this.GetSystem<IRedPointSystemModule>().ShowView();
        }

        private void Update()
        {

        }

        public IMgr Ins => Global.GetInstance();
    }
}