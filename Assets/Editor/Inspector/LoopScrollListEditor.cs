
using Tool.CustomAttribute;
using UnityEditor;
using UnityEngine;
using Wights.Utilities;
namespace Editor.Attribute
{
    [CustomEditor(typeof(LoopScrollList))]
    public class LoopScrollListEditor : UnityEditor.Editor
    {
        private LoopScrollList _loopScrollList;
        private static bool _inEditor = false;
        void OnEnable()
        {
            _loopScrollList = target as LoopScrollList;
        }
        public override void OnInspectorGUI()
        {
            EditorGUILayout.BeginVertical();
            if (GUILayout.Button(_inEditor ? "展开调试数据" : "关闭调试数据"))
            {
                _inEditor = !_inEditor;
            }
            if (_inEditor)
            {
                EditorGUILayout.LabelField("拖拽距离：" + _loopScrollList.MoveDis.ToString());
                EditorGUILayout.LabelField("是否有最小补偿：" + _loopScrollList.NeedMinPeyHeight.ToString());
                EditorGUILayout.LabelField("补偿高度：" + _loopScrollList.PayHeight);
                EditorGUILayout.LabelField("缓冲速度：" + _loopScrollList.ScrollBufferSpeed);
                EditorGUILayout.LabelField("顶格cell下标：" + _loopScrollList.UpIndex);
                EditorGUILayout.LabelField("底格cell下标：" + _loopScrollList.DownIndex);
                var oldColor = GUI.color;
                GUI.color = Color.red;
                EditorGUILayout.LabelField(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>分割线");
                GUI.color = oldColor;
                Repaint();
            }
            EditorGUILayout.EndVertical();
            base.OnInspectorGUI();
            if (_loopScrollList.cell==null|| _loopScrollList.content==null|| _loopScrollList.viewport==null)
            {
                EditorGUILayout.HelpBox("注意参数不能为空!", MessageType.Error);
            }
        }
    }
}