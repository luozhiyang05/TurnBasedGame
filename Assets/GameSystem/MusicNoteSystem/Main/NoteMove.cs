using Framework;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Assets.GameSystem.MusicNoteSystem.Main
{
    public class NoteMove : MonoBehaviour,ICanGetModel
    {
        public int id;
        private bool _isMove;
        private bool _isPass;
        private bool _isPress;
        private NoteData noteData;
        private UnityAction<ENotePos,int> _realeaseCallback;
        private MusicData _musicData;

        public IMgr Ins => Global.GetInstance();

        void OnEnable()
        {
            _isMove = false;
            _isPress = false;
        }
        public void SetNoteData(NoteData noteData)
        {
            _isMove = true;
            _musicData = this.GetModel<MusicData>();
            this.noteData = noteData;
        }
        public void SetReleaseCallback(UnityAction<ENotePos,int> releaseCallback)
        {
            _realeaseCallback = releaseCallback; 
        }
        public int GetId()
        {
            return noteData.id;
        }
        public void PressNote()
        {
            _isPress = true;
            _realeaseCallback?.Invoke(noteData.notePos, noteData.id);
            if (noteData.notePos == ENotePos.Up)
            {
                _musicData.RemoveReady2ClickNote(true);
            }
            else if (noteData.notePos == ENotePos.Down)
            {
                _musicData.RemoveReady2ClickNote(false);
            }
        }
        void Update()
        {
            if (_isMove)
            {
                float speed = 1596 / 3;
                var targetPos = transform.localPosition;
                transform.localPosition = new Vector2(targetPos.x - speed * Time.deltaTime, targetPos.y);

                //超过屏幕界限，则从准备点击的实体音符列表中移除，（音符进入池子）
                if (transform.localPosition.x <= -2040)
                {
                    _realeaseCallback?.Invoke(noteData.notePos, noteData.id);
                    _isMove = false;
                }

                //超过击打界限，则从准备点击的音符数据列表中移除
                if (!_isPress)
                {
                    if (_musicData.GetNowMusicTime() > noteData.judgeTime + _musicData.GetGreatTime() / 2)
                    {
                        if (noteData.notePos == ENotePos.Up)
                        {
                            _musicData.RemoveReady2ClickNote(true);
                        }
                        else if (noteData.notePos == ENotePos.Down)
                        {
                            _musicData.RemoveReady2ClickNote(false);
                        }
                        _isPress = true;
                    }
                }
            }
        }
    } 
}