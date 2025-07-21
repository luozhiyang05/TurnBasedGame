
using Assets.Wights.Scripts;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
namespace Editor.Attribute
{
    [CustomEditor(typeof(LoopScrollList))]
    public class LoopScrollListEditor : UnityEditor.Editor
    {
        private LoopScrollList _loopScrollList;
        private static bool _showData = false;
        private static bool _inEditor = false;
        private static int _moveTargetIndex = -1;
        private static int _jumpTargetIndex = -1;
        private static int _selectIndex = -1;
        void OnEnable()
        {
            _loopScrollList = target as LoopScrollList;
        }
        public override void OnInspectorGUI()
        {
            EditorGUILayout.BeginVertical();
            if (GUILayout.Button(_showData ? "关闭调试数据" : "展示调试数据"))
            {
                _showData = !_showData;
            }
            if (_showData)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("拖拽距离：" + _loopScrollList.MoveDis.ToString());
                EditorGUILayout.LabelField("缓冲速度：" + _loopScrollList.ScrollBufferSpeed);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("是否有最小补偿：" + _loopScrollList.NeedMinPeyHeight.ToString());
                EditorGUILayout.LabelField("补偿高度：" + _loopScrollList.PayHeight);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("顶格cell下标：" + _loopScrollList.UpIndex);
                EditorGUILayout.LabelField("底格cell下标：" + _loopScrollList.DownIndex);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("数据长度：" + _loopScrollList.DataCnt);
                EditorGUILayout.LabelField("是否开启缓冲：" + _loopScrollList.useBuffer);
                EditorGUILayout.EndHorizontal();

                var oldColor = GUI.color;
                GUI.color = Color.red;
                EditorGUILayout.LabelField(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>分割线");
                GUI.color = oldColor;
                Repaint();
            }
            EditorGUILayout.EndVertical();

            base.OnInspectorGUI();
            if (_loopScrollList.isVertical && !_loopScrollList.content.GetComponent<VerticalLayoutGroup>() ||
                !_loopScrollList.isVertical && !_loopScrollList.content.GetComponent<HorizontalLayoutGroup>())
            {
                EditorGUILayout.HelpBox("Content缺少垂直或水平布局组件！", MessageType.Error);
            }
            if (_loopScrollList.cell == null || _loopScrollList.content == null || _loopScrollList.viewport == null)
            {
                EditorGUILayout.HelpBox("注意参数不能为空!", MessageType.Error);
            }

            if (GUILayout.Button(_inEditor ? "关闭调试模式" : "打开调试模式"))
            {
                _inEditor = !_inEditor;
            }
            if (_inEditor)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("移动定位Index：", GUILayout.MaxWidth(90));
                _moveTargetIndex = EditorGUILayout.IntField(_moveTargetIndex);
                if (GUILayout.Button("执行", GUILayout.MinWidth(70)))
                {
                    if (_moveTargetIndex != -1)
                    {
                        _loopScrollList.MoveToIndex(_moveTargetIndex);
                    }
                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("直接定位Index：", GUILayout.MaxWidth(90));
                _jumpTargetIndex = EditorGUILayout.IntField(_jumpTargetIndex);
                if (GUILayout.Button("执行", GUILayout.MinWidth(70)))
                {
                    if (_jumpTargetIndex != -1)
                    {
                        _loopScrollList.JumpToIndex(_jumpTargetIndex);
                    }
                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("选择cell的Index：", GUILayout.MaxWidth(100));
                _selectIndex = EditorGUILayout.IntField(_selectIndex);
                if (GUILayout.Button("执行1", GUILayout.MinWidth(50)))
                {
                    if (_selectIndex != -1)
                    {
                        _loopScrollList.SelectCell(_selectIndex);
                    }
                }
                if (GUILayout.Button("执行2", GUILayout.MinWidth(50)))
                {
                    if (_selectIndex != -1)
                    {
                        _loopScrollList.SelectCell(_selectIndex, false);
                    }
                }
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.HelpBox("执行1为触发选择事件，执行2为不触发选择事件", MessageType.Info);
            }
        }
    }
}