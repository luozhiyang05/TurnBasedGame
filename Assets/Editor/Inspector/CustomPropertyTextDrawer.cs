
using Tool.CustomAttribute;
using UnityEditor;
using UnityEngine;
namespace Editor.Attribute
{
    [CustomPropertyDrawer(typeof(CustomPropertyTextAttribute))]
    public class CustomPropertyTextDrawer : PropertyDrawer
    {
        private CustomPropertyTextAttribute m_CustomPropertyAttribute;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            m_CustomPropertyAttribute = attribute as CustomPropertyTextAttribute;
            EditorGUI.PropertyField(position, property, new GUIContent(m_CustomPropertyAttribute.propertyName == default ? label : new GUIContent(m_CustomPropertyAttribute.propertyName)));
        }

        public override bool CanCacheInspectorGUI(SerializedProperty property)
        {
            return base.CanCacheInspectorGUI(property);
        }
    }
}