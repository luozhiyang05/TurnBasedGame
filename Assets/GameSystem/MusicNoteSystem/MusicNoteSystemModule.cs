using System.Collections.Generic;
using Assets.GameSystem.MusicNoteSystem.Main;
using Framework;
using Tool.Mono;
using Tool.ResourceMgr;
using Tool.Utilities;
using Tool.Utilities.Events;
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
        private Timer _doubleClickTimer;
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
            var hitScore = _musicData.CalculateHitLevel(noteData);
            if (hitScore != EHitLevel.None)
            {
                if (noteData.noteType == ENoteType.DoubleClick)
                {
                    CheckDoubleClick(noteData);
                }
                else
                {
                    //发布音符打击事件
                    EventsHandle.EventTrigger(EventsNameConst.BIT_NOTE, noteData, hitScore);
                }
            }
        }

        private void CheckDoubleClick(NoteData noteData)
        {
            var doubleClickNoteData = _musicData.GetDoubleClickNoteData();
            var isWaitingDoubleClick = _musicData.GetIsWaitingDoubleClick();
            if (doubleClickNoteData == null)
            {
                _musicData.SetDoubleClickNoteData(noteData);
                _musicData.SetIsWaitingDoubleClick(true);
                _doubleClickTimer = ActionKit.GetInstance().AddTimer(() =>
                {
                    _musicData.SetDoubleClickNoteData(null);
                    _musicData.SetIsWaitingDoubleClick(false);
                    ActionKit.GetInstance().RemoveTimer(_doubleClickTimer.GetName());
                }, _musicData.GetDoubleGreatTime(), "DoubleClick" + noteData.id);
            }
            else
            {
                //连续击打同一音符的容错
                if (noteData.id == doubleClickNoteData.id)
                {
                    return;
                }

                if (isWaitingDoubleClick)
                {
                    //判断后进入的notedata是否是属于一组双击音符，属于则取判断是否有击打中
                    if (noteData.createTime == doubleClickNoteData.createTime)
                    {
                        //击打判断
                        var oldNotEHitLevel = _musicData.CalculateHitLevel(doubleClickNoteData);
                        var newNotEHitLevel = _musicData.CalculateHitLevel(noteData);

                        //发布音符打击事件
                        EventsHandle.EventTrigger(EventsNameConst.BIT_NOTE, doubleClickNoteData, oldNotEHitLevel);
                        EventsHandle.EventTrigger(EventsNameConst.BIT_NOTE, noteData, newNotEHitLevel);

                        ActionKit.GetInstance().RemoveTimer(_doubleClickTimer.GetName());
                        _musicData.SetDoubleClickNoteData(null);
                        _musicData.SetIsWaitingDoubleClick(false);
                    }
                    else
                    {
                        //不属于则表明上一组双击音符pass
                        ActionKit.GetInstance().RemoveTimer(_doubleClickTimer.GetName());
                        _musicData.SetDoubleClickNoteData(null);
                        _musicData.SetIsWaitingDoubleClick(false);

                        //建立新的一组双击
                        CheckDoubleClick(noteData);
                    }
                }
                else
                {
                    _musicData.SetDoubleClickNoteData(null);
                }
            }
        }
    }
}