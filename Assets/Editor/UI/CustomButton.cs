
using UnityEngine;
using UnityEditor;
using UIComponents;
using UnityEngine.UI;
namespace Editor.UI
{
    //参数1，要自定义的类，参数2.是否对其子类有效
    //继承UnityEditor.UI.ButtonEditor,及继承ButtonEditor编辑器面板
    [CustomEditor(typeof(CButton),true)]
    public class CustomButtonEditor : UnityEditor.UI.ButtonEditor
    {
        private CButton _cbutton;
        private SerializedProperty _text;
        protected override void OnEnable()
        {
            base.OnEnable();
            //target表示自定义类的实例化对象
            _cbutton = target as CButton;
            //serializedObject属性封装了该类所有的序列化字段和一些方法
            _text = serializedObject.FindProperty("Label");
        }
        public override void OnInspectorGUI()
        {
            //让一个字段接受更多类型
            _cbutton.Label = (Text)EditorGUILayout.ObjectField("Text", _cbutton.Label, typeof(Text), true);
            base.OnInspectorGUI();
           
            //更新序列化字段
            serializedObject.Update();
            //应用修改
            serializedObject.ApplyModifiedProperties();
        }
    }
}