using Editor;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MusicSo))]
public class MusicSoWindow : UnityEditor.Editor
{
    private MusicSo _target;
    private float _nowMusicTime;
    private double _lastUpdateTime;
    private bool _isStartUpdate;

    void OnEnable()
    {
        _target = target as MusicSo;
        EditorApplication.update += Update;
        Debug.Log("添加");
    }

    void OnDisable()
    {
        EditorApplication.update -= Update;
        Debug.Log("移除");
    }

    private void Update()
    {
        if (_isStartUpdate)
        {
            double currentTime = EditorApplication.timeSinceStartup;
            _nowMusicTime += (float)(currentTime - _lastUpdateTime);
            _lastUpdateTime = currentTime;

            Repaint();
        }
    }

    public override void OnInspectorGUI()
    {
        //按钮
        if (GUILayout.Button(!_isStartUpdate ? "播放" : "暂停"))
        {
            _isStartUpdate = !_isStartUpdate;
            if (_isStartUpdate)
            {
                _lastUpdateTime = EditorApplication.timeSinceStartup;
            }
        }
        if (GUILayout.Button("重置"))
        {
            _isStartUpdate = false;
            _nowMusicTime = 0;
        }
        //滑动条
        _nowMusicTime = EditorGUILayout.Slider(_nowMusicTime, 0, _target.musicTime);
        if (GUILayout.Button("添加音符"))
        {
            _target.AddNote(_nowMusicTime);
        }
        if (GUILayout.Button("删除所有音符"))
        {
            _target.ClearNote();
        }
        base.OnInspectorGUI();
    }
}