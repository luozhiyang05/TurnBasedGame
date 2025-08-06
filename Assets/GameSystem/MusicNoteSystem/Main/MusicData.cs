
using System.Collections.Generic;
using Assets.GameSystem.MusicNoteSystem.Scripts;
using GameSystem.MVCTemplate;
using Tool.Mono;
using Tool.ResourceMgr;
using Tool.Utilities;
using UnityEngine;

namespace Assets.GameSystem.MusicNoteSystem.Main
{
    public enum EMusicName
    {
        Test2
    }
    public class MusicData : BaseModel
    {
        private const string SYSTEM_PATH = "Assets/GameSystem/MusicNoteSystem/";
        private const string GLOBAL_SETTING_NAME = "GlobalMusicSettingSO";
        private const string MUSIC_ASSET_PREFIX = "MusicSO/";
        private const string MUSIC_ASSET_SUFFIX = "SO";
        private const string ASSET_SUFFIX = ".asset";
        private GlobalMusicSettingSO _globalMusicSettingSO;
        private MusicSo _musicSo;
        private QArray<NoteData> _upRemainderNoteDatas,_downRemainderNoteDatas;
        private QArray<NoteData> _upReady2ClickDatas,_downReady2ClickDatas;
        private bool _playStatus;
        private float _nowMusicTime;
        public override void Init()
        {
            PublicMonoKit.GetInstance().OnRegisterUpdate(() =>
            {
                if (_playStatus)
                {
                    _nowMusicTime += Time.deltaTime;
                }
            });
        }

        public void LoadMusic(EMusicName eMusicName)
        {
            _musicSo = ResMgr.GetInstance().LoadAsset<MusicSo>(SYSTEM_PATH + MUSIC_ASSET_PREFIX, eMusicName + MUSIC_ASSET_SUFFIX + ASSET_SUFFIX);
            _globalMusicSettingSO = ResMgr.GetInstance().LoadAsset<GlobalMusicSettingSO>(SYSTEM_PATH + MUSIC_ASSET_PREFIX, GLOBAL_SETTING_NAME + ASSET_SUFFIX);
            _upRemainderNoteDatas = _musicSo.GetUpNoteQArray();
            _downRemainderNoteDatas = _musicSo.GetDownNoteQArray();
            _upReady2ClickDatas = new QArray<NoteData>();
            _downReady2ClickDatas = new QArray<NoteData>();
        }
        public void SetPlayState(bool playStatus)
        {
            _playStatus = playStatus;
        }
        public bool GetPlayState()
        {
            return _playStatus;
        } 

        public float GetNowMusicTime()
        {
            return _nowMusicTime;
        }
        public int GetRemainderNoteDataCnt(bool isUp)
        {
            return isUp ? _upRemainderNoteDatas.Count : _downRemainderNoteDatas.Count;
        }
        public NoteData PeekOneNoteData(bool isUp)
        {
            return isUp ? _upRemainderNoteDatas.Peek() : _downRemainderNoteDatas.Peek();
        }
        public NoteData GetHeadNoteData(bool isUp,NoteMove noteMove)
        {
            var noteData = isUp ? _upRemainderNoteDatas.GetFromHead() : _downRemainderNoteDatas.GetFromHead();
            noteData.SetNoteMove(noteMove);
            if (isUp)
            {
                _upReady2ClickDatas.Add(noteData);
            }
            else
            {
                _downReady2ClickDatas.Add(noteData);
            }
            return noteData;
        }
        public NoteData PeekReady2ClickNote(bool isUp)
        {
            return isUp ? _upReady2ClickDatas.Peek() : _downReady2ClickDatas.Peek();
        }
        public void RemoveReady2ClickNote(bool isUp)
        {
            if (isUp)
            {
                var tempNote = _upReady2ClickDatas.RemoveAt(0);
                Debug.LogWarning("移除上音符数据：" + tempNote.id);
            }
            else _downReady2ClickDatas.RemoveAt(0);
        }
        public float GetPerfectTime()
        {
            return _globalMusicSettingSO.perfectTime;
        }
        public float GetGreatTime()
        {
            return _globalMusicSettingSO.greatTime; 
        }
        /// <summary>
        /// 监听某些数据更改事件,可以通知view更新
        /// </summary>
        public override void BindListener()
        {
        }

        /// <summary>
        /// 移除事件
        /// </summary>
        public override void RemoveListener()
        {
        }
    }
}