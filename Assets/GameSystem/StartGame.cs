using Framework;
using Tool.UI;
using UnityEngine;

namespace GameSystem
{
    public class StartGame : MonoBehaviour, ICanGetSystem
    {
        private void Start()
        {
            
        }

        private void Update()
        {
   
        }

        public IMgr Ins => Global.GetInstance();
    }
}