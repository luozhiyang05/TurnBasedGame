using UIComponents;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Editor
{
    public static class CreateUIComponentsTools
    {
        public const string BtnPath = "Assets/Wights/Btn_Temp.prefab";
        public const string BtnName = "Btn_";

        [MenuItem("GameObject/自定义组件/CButton",false,-100)]
        public static void CreateCButton(MenuCommand menuCommand)
        {
            var btn = AssetDatabase.LoadAssetAtPath<GameObject>(BtnPath);
            var cBtnGo = GameObject.Instantiate(btn);
            cBtnGo.name = BtnName;
            GameObjectUtility.SetParentAndAlign(cBtnGo, menuCommand.context as GameObject);//设置父节点为当前选中物体
            Undo.RegisterCreatedObjectUndo(cBtnGo, "Create：" + "CButton");//注册到Undo系统,允许撤销
            Selection.activeObject = cBtnGo;//将新建物体设为当前选中物体
        }
    }
}