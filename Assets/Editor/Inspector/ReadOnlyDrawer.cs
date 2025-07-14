
using TMPro;
using Tool.CustomAttribute;
using UnityEditor;
using UnityEngine;
namespace Editor.Attribute
{
    [CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
    public class ReadOnlyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var color = (attribute as ReadOnlyAttribute).color;
            string value;
            switch (property.propertyType)
            {
                case SerializedPropertyType.Integer:
                    value = property.intValue.ToString();
                    break;
                case SerializedPropertyType.Float:
                    value = property.floatValue.ToString();
                    break;
                case SerializedPropertyType.String:
                    value = property.stringValue;
                    break;
                case SerializedPropertyType.Boolean:
                    value = property.boolValue.ToString();
                    break;

                case SerializedPropertyType.Color:
                    value = property.colorValue.ToString();
                    break;
                case SerializedPropertyType.Vector3:
                    value = property.vector3Value.ToString();
                    break;
                case SerializedPropertyType.Rect:
                    value = property.rectValue.ToString();
                    break;
                default:
                    value = "null";
                    break;
            }

            //设置字段颜色
            var oldColor = GUI.color;
            GUI.color = color == default ? Color.white : color;
            EditorGUI.LabelField(position, property.name + "：" + value);
            GUI.color = oldColor;
        }

        public override bool CanCacheInspectorGUI(SerializedProperty property)
        {
            return base.CanCacheInspectorGUI(property);
        }
    }
}