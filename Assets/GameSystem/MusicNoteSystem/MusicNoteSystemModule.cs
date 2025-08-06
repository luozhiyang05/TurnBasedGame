using System.Collections.Generic;
using Assets.GameSystem.MusicNoteSystem.Main;
using Framework;
using Tool.Mono;
using Tool.ResourceMgr;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
namespace Assets.GameSystem.MusicNoteSystem
{
    public interface IMusicNoteSystemModule : IModule
    {
        void OpenView(params object[] args);
        void LoadMusic(EMusicName eMusicName);
        void StartGame();
        void PressNote(float nowTime, bool isUp);
    }

    public class MusicNoteSystemModule : BaseModule, IMusicNoteSystemModule
    {
        private MusicData _musicData;
        protected override void InitModule()
        {
            ctrl = new MusicNoteFloatViewCtrl(nameof(MusicNoteSystemModule));
            _musicData = ctrl.GetModel() as MusicData;
            this.RegisterModel(_musicData);
        }

        public void LoadMusic(EMusicName eMusicName)
        {
            _musicData.LoadMusic(eMusicName);
        }

        public void OpenView(params object[] args)
        {
            (ctrl as MusicNoteFloatViewCtrl).OpenView(args);
        }

        public void StartGame()
        {
            _musicData.SetPlayState(true);
        }

        public void PressNote(float nowTime, bool isUp)
        {
            NoteData noteData = _musicData.PeekReady2ClickNote(isUp);

            var time = Mathf.Abs(noteData.judgeTime - nowTime);
            var perfectTime = _musicData.GetPerfectTime();
            var greatTime = _musicData.GetGreatTime();
            var perfect = time <= perfectTime;
            var great = time <= greatTime;

            if (perfect || great)
            {
                //设置音符为已经点击
                noteData.GetNoteMove().PressNote();

                if (perfect)
                {
                    Debug.LogWarning("Perfect!");
                }
                else
                {
                    Debug.LogWarning("Great!");
                }
            }
        }
    }
}