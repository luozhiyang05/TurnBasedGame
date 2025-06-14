
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
        protected override void OnEnable()
        {
            base.OnEnable();
            //target表示自定义类的实例化对象
            _cbutton = target as CButton;
        }
        public override void OnInspectorGUI()
        {
            //Text
            _cbutton.Label = (Text)EditorGUILayout.ObjectField("Text", _cbutton.Label, typeof(Text), true);
            _cbutton.Label = _cbutton.transform.Find("text").GetComponent<Text>();
            
            base.OnInspectorGUI();
        }
    }
}