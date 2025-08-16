using System.Collections.Generic;
using Editor;
using Unity.VisualScripting.YamlDotNet.Core.Tokens;
using UnityEditor;
using UnityEditor.EditorTools;
using UnityEngine;

namespace GameInspector
{
    public class KeyCodeState
    {
        public bool isPress;
        public bool pressing;
        public float pressTime;
    }
    [CustomEditor(typeof(MusicSo))]
    public class MusicInspector : UnityEditor.Editor
    {
        private MusicSo _target;
        private AudioSource _previewAudioSource;
        private float _nowMusicTime;
        private bool _isStartUpdate;
        private float _intervalNoteTime;
        private int _createIntervalNoteCnt;
        private int _createNoteType = 0;    //生成音符类型，0上阶单击音符，1下阶单击音符，2双击音符
        private Dictionary<KeyCode, KeyCodeState> _keyCodeDict = new Dictionary<KeyCode, KeyCodeState>();
        private float _nowNoteCreateTime;
        private bool _isLongPress;
        void OnEnable()
        {
            _target = target as MusicSo;
            EditorApplication.update += Update;
            _previewAudioSource = EditorUtility.CreateGameObjectWithHideFlags(
            "PreviewAudioSource",
            HideFlags.HideAndDontSave,
            typeof(AudioSource)).GetComponent<AudioSource>();
        }

        void OnDisable()
        {
            EditorApplication.update -= Update;
            DestroyImmediate(_previewAudioSource.gameObject);
        }

        private void Update()
        {
            if (_isStartUpdate)
            {
                _nowMusicTime = _previewAudioSource.time;
                Repaint();
            }
        }

        public override void OnInspectorGUI()
        {
            //处理输入
            HandleKeyDown();

            //容错
            if (null == _target.audioClip) EditorGUILayout.HelpBox("请先设置预览音频源！", MessageType.Error);

            //播放/暂停 按钮
            if (GUILayout.Button(!_isStartUpdate ? "播放" : "暂停"))
            {
                if (null == _target.audioClip)
                {
                    Debug.LogError("请先设置预览音频源！");
                    return;
                }
                _isStartUpdate = !_isStartUpdate;
                HandleMusic();
            }

            //连续添加音符
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("间隔时间：", GUILayout.MaxWidth(60));
            _intervalNoteTime = EditorGUILayout.FloatField(_intervalNoteTime, GUILayout.MaxWidth(50));
            EditorGUILayout.LabelField("添加个数：", GUILayout.MaxWidth(60));
            _createIntervalNoteCnt = EditorGUILayout.IntField(_createIntervalNoteCnt, GUILayout.MaxWidth(30));
            _createNoteType = EditorGUILayout.Popup(_createNoteType, new string[] { "上阶单击音符", "下阶单击音符", "双击音符" }, GUILayout.MaxWidth(160));
            if (GUILayout.Button("连续添加音符"))
            {
                AddIntervalNote();
            }
            EditorGUILayout.EndHorizontal();

            //重置按钮
            if (GUILayout.Button("重置")) ResetMusic();

            //滑动条
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("当前播放时间：", GUILayout.MaxWidth(80));
            _nowMusicTime = EditorGUILayout.Slider(_nowMusicTime, 0, _target.musicTime);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.HelpBox("在音频播放时，D/F快速添加上轨迹音符，J/K快速添加下轨迹音符（时间小于音符到达打击点时间时无响应），H生成双击音符，空格暂停", MessageType.Info);

            //删除所有音符
            if (GUILayout.Button("删除所有音符"))
            {
                _target.ClearNote();
            }
            base.OnInspectorGUI();
        }

        private void HandleMusic()
        {
            if (_isStartUpdate)
            {
                _previewAudioSource.clip = _target.audioClip;
                _previewAudioSource.time = _nowMusicTime;
                _previewAudioSource.pitch = _target.musicSpeed;
                _previewAudioSource.Play();
            }
            else
            {
                _previewAudioSource.Pause();
            }
        }

        private void ResetMusic()
        {
            _isStartUpdate = false;
            _nowMusicTime = 0;
            _previewAudioSource.Stop();
        }

        private void HandleKeyDown()
        {
            //在 EditorApplication.update ，Event会报空。这里用于快速添加音符
            if (Event.current.type == EventType.KeyDown)
            {
                if (Event.current.keyCode == KeyCode.Space)
                {
                    _isStartUpdate = !_isStartUpdate;
                    if (_isStartUpdate)
                        _previewAudioSource.Play();
                    else
                        _previewAudioSource.Pause();
                }
            }

            
            HandleKeyLongPressDown();
            
            HandleKeyLongPressUp(KeyCode.D);
            HandleKeyLongPressUp(KeyCode.F);
            HandleKeyLongPressUp(KeyCode.J);
            HandleKeyLongPressUp(KeyCode.K);

            HandleKeyDown(KeyCode.D, true);
            HandleKeyDown(KeyCode.F, true);
            HandleKeyDown(KeyCode.J, false);
            HandleKeyDown(KeyCode.K, false);

            HandleKeyUp(KeyCode.D);
            HandleKeyUp(KeyCode.F);
            HandleKeyUp(KeyCode.J);
            HandleKeyUp(KeyCode.K);


            // if (Event.current.type == EventType.KeyDown && _nowMusicTime >= _target.reachToBitPosTime)
            // {
            //     //检测长按，用于生成长按音符
            //     if (_keyCodeDict.ContainsKey(Event.current.keyCode))
            //     {
            //         _isLongPress = true;
            //         return;
            //     }

            //     //检测短按，用于生成点击音符
            //     if (Event.current.keyCode == KeyCode.J || Event.current.keyCode == KeyCode.K && _isStartUpdate)
            //     {
            //         _target.AddSingleClickNote(_nowMusicTime, ENotePos.Up, ENoteType.Click);
            //         _nowNoteCreateTime = _nowMusicTime;
            //         _keyCodeDict[Event.current.keyCode] = true;
            //     }
            //     else if (Event.current.keyCode == KeyCode.D || Event.current.keyCode == KeyCode.F && _isStartUpdate)
            //     {
            //         _target.AddSingleClickNote(_nowMusicTime, ENotePos.Down, ENoteType.Click);
            //         _nowNoteCreateTime = _nowMusicTime;
            //         _keyCodeDict[Event.current.keyCode] = true;
            //     }
            //     else if (Event.current.keyCode == KeyCode.H && _isStartUpdate)
            //     {
            //         _target.AddSingleClickNote(_nowMusicTime, ENotePos.Up, ENoteType.DoubleClick);
            //         _target.AddSingleClickNote(_nowMusicTime, ENotePos.Down, ENoteType.DoubleClick);
            //     }
            // }

            // //按键抬起
            // if (Event.current.type == EventType.KeyUp)
            // {
            //     if (_keyCodeDict.ContainsKey(Event.current.keyCode))
            //     {
            //         _keyCodeDict[Event.current.keyCode] = false;

            //         //添加长按音符
            //         if (_isLongPress)
            //         {
            //             _isLongPress = false;
            //             if (Event.current.keyCode == KeyCode.J || Event.current.keyCode == KeyCode.K)
            //             {
            //                 _target.AddLongNote(_nowMusicTime, _nowMusicTime - _nowNoteCreateTime, ENotePos.Down);

            //             }
            //             else if (Event.current.keyCode == KeyCode.D || Event.current.keyCode == KeyCode.F)
            //             {
            //                 _target.AddLongNote(_nowMusicTime, _nowMusicTime - _nowNoteCreateTime, ENotePos.Up);
            //             }
            //         }
            //     }
            // }
        }

        private void HandleKeyDown(KeyCode keyCode, bool isUp)
        {
            if (Event.current.keyCode == keyCode && Event.current.type == EventType.KeyDown)
            {
                if (!_keyCodeDict.ContainsKey(keyCode))
                {
                    _keyCodeDict.Add(keyCode, new KeyCodeState() { isPress = false, pressTime = _nowMusicTime });
                }
                if (!_keyCodeDict[keyCode].isPress)
                {
                    _keyCodeDict[keyCode].isPress = true;
                    _keyCodeDict[keyCode].pressTime = _nowMusicTime;
                    Debug.LogWarning(_nowMusicTime);
                    _target.AddSingleClickNote(_nowMusicTime, isUp ? ENotePos.Up : ENotePos.Down, ENoteType.Click);
                }
            }
        }

        private void HandleKeyLongPressDown()
        {
            foreach (var key in _keyCodeDict.Keys)
            {
                var keyCodeState = _keyCodeDict[key];
                if (keyCodeState.isPress)
                {
                    if (_nowMusicTime - keyCodeState.pressTime > 0.2f)
                    {
                        _keyCodeDict[key].pressing = true;
                        Debug.LogWarning("按压中："+_nowMusicTime+"----"+keyCodeState.pressTime);
                    }
                }
            }
        }
        private void HandleKeyLongPressUp(KeyCode keyCode)
        {
            if (Event.current.keyCode == keyCode && Event.current.type == EventType.KeyUp)
            {
                var keyCodeState = _keyCodeDict[keyCode];
                if (keyCodeState.pressing)
                {
                    _target.AddLongNote(keyCodeState.pressTime, _nowMusicTime - keyCodeState.pressTime + 0.2f, ENotePos.Down);
                    keyCodeState.pressing = false;
                    keyCodeState.pressTime = 0;
                }
            }
        }

        private void HandleKeyUp(KeyCode keyCode)
        {
            if (Event.current.type == EventType.KeyUp)
            {
                if (_keyCodeDict.ContainsKey(keyCode))
                {
                    var keyCodeState = _keyCodeDict[keyCode];
                    keyCodeState.isPress = false;
                }
            }
        }



        private void AddIntervalNote()
        {
            if (_nowMusicTime >= _target.reachToBitPosTime)
            {
                for (int i = 0; i < _createIntervalNoteCnt; i++)
                {
                    if (_createNoteType == 2)
                    {
                        _target.AddSingleClickNote(_nowMusicTime + i * _intervalNoteTime, ENotePos.Up, ENoteType.DoubleClick);
                        _target.AddSingleClickNote(_nowMusicTime + i * _intervalNoteTime, ENotePos.Down, ENoteType.DoubleClick);
                    }
                    else
                    {
                        _target.AddSingleClickNote(_nowMusicTime + i * _intervalNoteTime, _createNoteType == 0 ? ENotePos.Up : ENotePos.Down, ENoteType.Click);
                    }
                }
            }
        }
    }
}
