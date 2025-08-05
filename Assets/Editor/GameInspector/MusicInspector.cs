using Editor;
using UnityEditor;
using UnityEditor.EditorTools;
using UnityEngine;

namespace GameInspector
{
    [CustomEditor(typeof(MusicSo))]
    public class MusicInspector : UnityEditor.Editor
    {
        private MusicSo _target;
        private AudioSource _previewAudioSource;
        private float _nowMusicTime;
        private bool _isStartUpdate;
        private float _intervalNoteTime;
        private int _createIntervalNoteCnt;
        private bool _createNoteUpType = true;

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
            EditorGUILayout.LabelField("上轨迹音符：", GUILayout.MaxWidth(70));
            _createNoteUpType = EditorGUILayout.Toggle(_createNoteUpType);
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
            EditorGUILayout.HelpBox("在音频播放时，D/F快速添加上轨迹音符，J/K快速添加下轨迹音符（时间小于音符到达打击点时间时无响应），空格暂停", MessageType.Info);

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
            if (Event.current.keyCode == KeyCode.Space)
            {
                _isStartUpdate = !_isStartUpdate;
                if (_isStartUpdate)
                    _previewAudioSource.Play();
                else
                    _previewAudioSource.Pause();
            }
            if (Event.current.type == EventType.KeyDown && _nowMusicTime >= _target.reachToBitPosTime)
            {
                if (Event.current.keyCode == KeyCode.J || Event.current.keyCode == KeyCode.K && _isStartUpdate)
                {
                    _target.AddNote(_nowMusicTime, ENotePos.Up, ENoteType.Click);
                }
                else if (Event.current.keyCode == KeyCode.D || Event.current.keyCode == KeyCode.F && _isStartUpdate)
                {
                    _target.AddNote(_nowMusicTime, ENotePos.Down, ENoteType.Click);
                }
            }
        }

        private void AddIntervalNote()
        {
            if (_nowMusicTime >= _target.reachToBitPosTime)
            {
                for (int i = 0; i < _createIntervalNoteCnt; i++)
                {
                    _target.AddNote(_nowMusicTime + i * _intervalNoteTime, _createNoteUpType ? ENotePos.Up : ENotePos.Down, ENoteType.Click);
                }
            }
        }
    }
}
