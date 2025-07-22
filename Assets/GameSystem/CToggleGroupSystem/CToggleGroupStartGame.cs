using Assets.GameSystem.CToggleGroupSystem;
using Framework;
using Tool.Mono;
using UnityEngine;

namespace GameSystem
{
    public class CToggleGroupStartGame : MonoBehaviour, ICanGetSystem
    {
        private void Start()
        {
            //CToggleGroup例子
            this.GetSystem<ICToggleGroupSystemModule>().ShowView();
            PublicMonoKit.GetInstance().OnRegisterUpdate(() =>
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    this.GetSystem<ICToggleGroupSystemModule>().ShowView();
                }
            });
        }

        private void Update()
        {

        }

        public IMgr Ins => Global.GetInstance();
    }
}