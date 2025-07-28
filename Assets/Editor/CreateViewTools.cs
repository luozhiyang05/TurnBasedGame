using System;
using System.IO;
using UnityEditor;
using UnityEngine;

public class CreateViewTools
{
    public enum EViewType
    {
        BaseView,
        SubPanelView,
    }
    public static EViewType viewType;
    [MenuItem("Assets/生成BaseView脚本", false, -1)]
    static void OpenWindow1()
    {
        //将path传递给窗口
        viewType = EViewType.BaseView;
        var window = EditorWindow.GetWindowWithRect<CreateViewToolsWindow>(new Rect(0, 0, 600, 400), true, "生成View");
    }

    [MenuItem("Assets/生成SubPanelView脚本", false, -1)]
    static void OpenWindow2()
    {
        //将path传递给窗口
        viewType = EViewType.SubPanelView;
        var window = EditorWindow.GetWindowWithRect<CreateViewToolsWindow>(new Rect(0, 0, 600, 400), true, "生成View");
    }
}

public class CreateViewToolsWindow : EditorWindow
{
    private const string BaseViewPrefabPath = "Assets/Wights/BaseView.prefab";
    private const string Tips = "请输入View名称";
    public string viewName;
    public string systemName;
    public string fullPath;
    public string prefabPath;
    private bool _isCreatePrefab = true;
    public CreateViewToolsWindow()
    {
        viewName = Tips;
        this.titleContent = new GUIContent("生成View");
    }

    void OnEnable()
    {
        var assetPath = AssetDatabase.GetAssetPath(Selection.activeObject);
        fullPath = Path.GetFullPath(assetPath);
        prefabPath = fullPath.Substring(0, fullPath.LastIndexOf('\\'));
        prefabPath = prefabPath.Substring(prefabPath.IndexOf("Assets"));
        systemName = prefabPath.Split('\\')[^1];
    }

    void OnGUI()
    {
        EditorGUILayout.Space();
        GUILayout.BeginVertical();
        viewName = EditorGUILayout.TextField("View名称:", viewName);
        EditorGUILayout.LabelField("脚本路径：" + fullPath);
        EditorGUILayout.LabelField("预制体路径：" + prefabPath);
        if (CreateViewTools.viewType == CreateViewTools.EViewType.BaseView)
        {
            _isCreatePrefab = GUILayout.Toggle(_isCreatePrefab, "是否生成预制体（需要手动挂载view脚本）");
        }
        if (GUILayout.Button("生成"))
        {
            CreateView(viewName);
        }
        GUILayout.EndVertical();

        EditorGUILayout.HelpBox("注意：视图名字省略View！！！（开玩笑）", MessageType.Warning);
    }

    private void CreateView(string viewName)
    {
        if (viewName.Equals(Tips) || "" == viewName)
        {
            Debug.LogWarning("请输入正确的View名称");
            return;
        }

        if (CreateViewTools.viewType == CreateViewTools.EViewType.BaseView)
        {
            const string templateViewPath = "Assets/Editor/Template/TemplateSystem/Main/TemplateSystemView.cs";
            try
            {
                //读取view的配置模板,生成view
                var viewContent = File.ReadAllText(templateViewPath);
                var newViewContent = viewContent.Replace("TemplateSystemView", viewName);
                newViewContent = newViewContent.Replace("TemplateSystem", systemName);
                var viewPath = string.Format("{0}\\{1}.cs", fullPath, viewName);
                File.WriteAllText(viewPath, newViewContent);

                //生成预制体
                if (_isCreatePrefab)
                {
                    var prefab = PrefabUtility.LoadPrefabContents(BaseViewPrefabPath);
                    prefab.name = viewName;
                    PrefabUtility.SaveAsPrefabAsset(prefab, prefabPath + '\\' + viewName + ".prefab");
                    PrefabUtility.UnloadPrefabContents(prefab);
                }

                Close();
                AssetDatabase.Refresh();
            }
            catch (Exception e)
            {
                throw new Exception("生成View失败：" + e);
            }
        }
        else
        {
            //读取view的配置模板,生成view
            const string templateViewPath = "Assets/Editor/Template/TemplateSystem/Main/TemplateSubPanelView.cs";
            try
            {
                var viewContent = File.ReadAllText(templateViewPath);
                var newViewContent = viewContent.Replace("TemplateSubPanelView", viewName);
                var viewPath = string.Format("{0}\\{1}.cs", fullPath, viewName);
                File.WriteAllText(viewPath, newViewContent);
                Close();
                AssetDatabase.Refresh();
            }
            catch (Exception e)
            {
                throw new Exception("生成View失败：" + e);
            }
        }


    }
}
