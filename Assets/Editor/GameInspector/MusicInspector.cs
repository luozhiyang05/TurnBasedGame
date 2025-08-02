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
            //在 EditorApplication.update ，Event会报空。这里用于快速添加音符
            if (Event.current.type == EventType.KeyDown && _isStartUpdate && _nowMusicTime >= _target.reachToBitPosTime)
            {
                if (Event.current.keyCode == KeyCode.J || Event.current.keyCode == KeyCode.K)
                {
                    _target.AddNote(_nowMusicTime, ENotePos.Up, ENoteType.Click);
                }
                else if (Event.current.keyCode == KeyCode.D || Event.current.keyCode == KeyCode.F)
                {
                    _target.AddNote(_nowMusicTime, ENotePos.Down, ENoteType.Click);
                }
                Event.current.Use();  // 阻止事件冒泡
            }

            //容错
            if (null == _target.audioClip) EditorGUILayout.HelpBox("请先设置预览音频源！", MessageType.Error);

            //按钮
            if (GUILayout.Button(!_isStartUpdate ? "播放" : "暂停"))
            {
                if (null == _target.audioClip)
                {
                    Debug.LogError("请先设置预览音频源！");
                    return;
                }

                _isStartUpdate = !_isStartUpdate;
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
            if (GUILayout.Button("重置"))
            {
                _isStartUpdate = false;
                _nowMusicTime = 0;
                _previewAudioSource.Stop();
            }
            //滑动条
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("当前播放时间：",GUILayout.MaxWidth(80));
            _nowMusicTime = EditorGUILayout.Slider(_nowMusicTime, 0, _target.musicTime);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.HelpBox("在音频播放时，D/F快速添加上轨迹音符，J/K快速添加下轨迹音符（时间小于音符到达打击点时间时无响应）", MessageType.Info);
            // if (GUILayout.Button("添加音符"))
            // {
            //     _target.AddNote(_nowMusicTime);
            // }
            if (GUILayout.Button("删除所有音符"))
            {
                _target.ClearNote();
            }
            base.OnInspectorGUI();
        }
    }
}
