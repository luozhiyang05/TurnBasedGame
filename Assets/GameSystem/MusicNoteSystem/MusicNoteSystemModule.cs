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
        void PressNote(NoteMove noteMove, float nowTime, bool isUp);
    }

    public class MusicNoteSystemModule : BaseModule, IMusicNoteSystemModule
    {
        private MusicData _musicData;
        protected override void InitModule()
        {
            ctrl = new MusicNoteFloatViewCtrl(nameof(MusicNoteSystemModule));
            _musicData = ctrl.GetModel() as MusicData;
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

        public void PressNote(NoteMove noteMove, float nowTime, bool isUp)
        {
            Debug.Log("点击DF");
            NoteData noteData = _musicData.PeekReady2ClickNote(isUp);

            var time = Mathf.Abs(noteData.judgeTime - nowTime);
            var perfectTime = _musicData.GetPerfectTime();
            var greatTime = _musicData.GetGreatTime();
            var perfect = time <= perfectTime;
            var great = time <= greatTime;

            if (perfect || great)
            {
                //移除音符数据
                _musicData.RemoveReady2ClickNote(isUp);
                //移除音符实体
                noteMove.PressNote();
                if (perfect)
                {
                    //得分
                    // _score += 10;
                    //播放音效
                    // _audioSource.PlayOneShot(_perfectAudioClip);
                    Debug.LogWarning("Perfect!");
                }
                else
                {
                    //得分
                    // _score += 5;
                    //播放音效
                    // _audioSource.PlayOneShot(_greatAudioClip);
                    Debug.LogWarning("Great!");
                }
            }
        }
    }
}