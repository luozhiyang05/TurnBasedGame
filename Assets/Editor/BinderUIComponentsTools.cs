using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UIComponents;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Editor
{
    public class BinderUIComponentsTools : EditorWindow
    {
        private static Dictionary<string, string> _uiRule = new Dictionary<string, string>()
        {
            { "Img_", "Image" },
            { "Txt_", "Text" },
            { "Btn_", "CButton" },
            { "Tog_", "Toggle" },
            { "Sld_", "Slider" },
            { "Sv_", "ScrollRect" }
        };

        private static string _componentPath;

        [MenuItem("QUFramework/生成单个预制体的UI组件/使用说明")]
        public static void Tips()
        {
            Debug.Log("必选选择一个预制体和它对应的view脚本，该脚本名字必须和预制体一直。\\n然后选择自动生成即可！");
        }

        [MenuItem("QUFramework/生成单个预制体的UI组件/自动生成")]
        public static void CreateUIComponents()
        {
            var targetGos = Selection.gameObjects;
            if (targetGos.Length is > 1 or 0) throw new Exception("必须选取一个View预制体！");
            var targetGo = targetGos[0];
            var targetGoPath = AssetDatabase.GetAssetPath(targetGo);
            var fileInfo = new FileInfo(targetGoPath);
            if (fileInfo.Extension != ".prefab") throw new Exception("必须选取一个View预制体！");
            var fileName = fileInfo.Name[..fileInfo.Name.LastIndexOf('.')] + ".cs";
            var lastIndexOf = targetGoPath.LastIndexOf("Resources", StringComparison.Ordinal);
            var searchPath = targetGoPath[..lastIndexOf];
            var searchFiles = Directory.EnumerateFiles(searchPath, fileName, SearchOption.AllDirectories);
            foreach (var filePath in searchFiles)
            {
                try
                {
                    //截取生成区域内容
                    var viewCon = File.ReadAllText(filePath);
                    var preStr = viewCon[..(viewCon.IndexOf("禁止手动更改！", StringComparison.Ordinal) + 7)] + "\n";
                    var behindStr = viewCon[(viewCon.IndexOf("自动生成UI组件区域结束！", StringComparison.Ordinal) - 11)..];

                    //获取预制体的UIBehavior，和配置对比
                    var componentsInChildren = targetGo.GetComponentsInChildren<UIBehaviour>();
                    var createdGos = new List<GameObject>();
                    var autoDefStr = new StringBuilder();
                    var autoAsgStr = new StringBuilder();
                    autoAsgStr.Append("        protected override void AutoInitUI()\n        {\n");
                    foreach (var uiBehaviour in componentsInChildren)
                    {
                        var go = uiBehaviour.gameObject;
                        //对比配置，拼接语句
                        if (go.name.Length < 4) continue;
                        var key = go.name[..4];
                        //不符合格式的，默认不需要自动生成
                        if (!_uiRule.TryGetValue(key, out var componentName)) continue;
                        if (createdGos.Contains(go)) continue;
                        createdGos.Add(go);
                        autoDefStr.Append($"\t\tpublic {componentName} {go.name};\n");
                        Debug.Log(uiBehaviour.name);
                        // var tempPath = FindComponentPath(go.name, targetGo.transform, "");
                        FindPath(targetGo.transform, go.name, "");
                        Debug.Log(_componentPath);
                        //var resultCompPath = tempPath[(tempPath.IndexOf('/') + 1)..];
                        autoAsgStr.Append(
                            $"\t\t\t{go.name} = transform.Find(\"{_componentPath}\").GetComponent<{componentName}>();\n");
                    }

                    //return;
                    autoAsgStr.Append("        }");
                    var resultContent = autoDefStr.Append(autoAsgStr.ToString());
                    //最终组合文本内容
                    var fileContent = preStr + resultContent + "\n\t\t" + behindStr;
                    //文件内容清空
                    File.WriteAllText(filePath, string.Empty);
                    File.WriteAllText(filePath, fileContent);
                }
                catch (Exception e)
                {
                    throw new Exception("生成文件信息错误：" + e);
                }
            }

            AssetDatabase.Refresh();
        }

        /// <summary>
        /// 递归寻找组件路径
        /// </summary>
        private static string FindComponentPath(string targetName, Transform parent, string path)
        {
            path += parent.gameObject.name + "/";
            Debug.Log(path);
            foreach (Transform value in parent)
            {
                if (value.gameObject.name.Equals(targetName))
                {
                    path += targetName;
                    return path;
                }
                else
                {
                    return FindComponentPath(targetName, value, path);
                }
            }

            return path;
        }

        private static void FindPath(Transform parent, string targetName, string currentPath)
        {
            // 遍历当前物体的所有子物体
            for (int i = 0; i < parent.childCount; i++)
            {
                Transform child = parent.GetChild(i);
                // 更新当前路径
                string newPath = currentPath.Length > 0 ? $"{currentPath}/{child.name}" : child.name;
                // 检查是否是目标物体
                if (child.name == targetName)
                {
                    Debug.Log($"找到目标物体路径: {newPath}");
                    _componentPath = newPath;
                    return; // 找到后退出递归
                }

                // 递归调用
                FindPath(child, targetName, newPath);
            }
        }
    }

    // [CustomEditor(typeof(CButton))]
    // public class CustomButtonEditor : UnityEditor.UI.ButtonEditor
    // {
    //     public override void OnInspectorGUI()
    //     {


    //         // 获取当前的 CustomButton 实例
    //         CButton button = (CButton)target;

    //         button.Label = (Text)EditorGUILayout.ObjectField("Text Component", button.Label, typeof(Text), true);

    //         // 调用基类的 OnInspectorGUI 以保留原有的 Button 参数
    //         base.OnInspectorGUI();
    //     }


    // }

}