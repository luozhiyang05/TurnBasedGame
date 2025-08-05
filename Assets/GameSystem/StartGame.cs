using Assets.GameSystem.MusicNoteSystem;
using Assets.GameSystem.MusicNoteSystem.Main;
using Framework;
using UnityEngine;

namespace GameSystem
{
    public class StartGame : MonoBehaviour, ICanGetSystem
    {
        private void Start()
        {
            var system = this.GetSystem<IMusicNoteSystemModule>();
            system.OpenView();
            system.LoadMusic(EMusicName.Test2);
            system.StartGame();
        }

        private void Update()
        {

        }

        public IMgr Ins => Global.GetInstance();
    }
}