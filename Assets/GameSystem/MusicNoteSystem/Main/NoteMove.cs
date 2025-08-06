using Framework;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Assets.GameSystem.MusicNoteSystem.Main
{
    public class NoteMove : MonoBehaviour,ICanGetModel
    {
        private bool _isMove;
        private bool _isPass;
        private bool _isPress;
        private NoteData noteData;
        private UnityAction<NoteData> _realeaseCallback;
        private MusicData _musicData;

        public IMgr Ins => Global.GetInstance();

        void OnEnable()
        {
            _isMove = false;
            _isPass = false;
            _isPress = false;
        }
        public void SetNoteData(NoteData noteData, UnityAction<NoteData> releaseCallback)
        {
            _isMove = true;
            this.noteData = noteData;
            _realeaseCallback = releaseCallback;
            _musicData = this.GetModel<MusicData>();
        }
        public int GetId()
        {
            return noteData.id;
        }
        public void PressNote()
        {
            _isPress = true;
            _realeaseCallback?.Invoke(noteData);
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
                    _realeaseCallback?.Invoke(noteData);
                }

                //超过击打界限，则从准备点击的音符数据列表中移除
                if (!_isPass && !_isPress)
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
                        _isPass = true;
                    }
                }
            }
        }
    } 
}