using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.U2D;
using UnityEngine;
using UnityEngine.U2D;

public class CreateSpriteAtlasMgr
{
    public static string PATH = "";
    public static string NAME = "";
    [MenuItem("Assets/生成图集", false, -1)]
    static void OpenWindow()
    {
        PATH = AssetDatabase.GetAssetPath(Selection.activeObject);
        //将path传递给窗口
        var window = EditorWindow.GetWindowWithRect<CreateSpriteAtlas>(new Rect(0, 0, 470, 400), true, "生成图集");
    }

    [MenuItem("Assets/生成图集", true, -1)]
    static bool OpenWindowJudge()
    {
        var obj = Selection.activeObject;
        //判断选中的对象是否是文件夹
        return obj != null && obj.GetType() == typeof(DefaultAsset);
    }
}

public class CreateSpriteAtlas : EditorWindow
{
    public CreateSpriteAtlas()
    {
        this.titleContent = new GUIContent("生成图集");
        //图集名
        var path = CreateSpriteAtlasMgr.PATH.Split('/');
        CreateSpriteAtlasMgr.NAME = string.Format("{0}.spriteatlas", path[^2] ?? "");
    }

    void OnGUI()
    {
        GUILayout.Space(10);

        GUILayout.BeginHorizontal();
        //文件路径
        EditorGUILayout.TextField(CreateSpriteAtlasMgr.PATH);
        //打开文件夹按钮
        if (GUILayout.Button("文件夹", GUILayout.MaxWidth(80)))
        {
            var tempPath = EditorUtility.OpenFolderPanel("选择文件夹", CreateSpriteAtlasMgr.PATH, "");
            //将字符串按照字符Asset分割，获取相对于Assets的路径
            var temp = tempPath.Split("Assets");
            CreateSpriteAtlasMgr.PATH = string.Format("Assets{0}", temp[1]);
        }
        GUILayout.EndHorizontal();

        GUILayout.Space(10);

        EditorGUILayout.BeginHorizontal();
        //图集名字
        GUILayout.Label("图集名：", GUILayout.Width(60));
        CreateSpriteAtlasMgr.NAME = EditorGUILayout.TextField(CreateSpriteAtlasMgr.NAME, GUILayout.MaxWidth(150));
        EditorGUILayout.EndHorizontal();

        GUILayout.Space(10);

        //生成按钮
        if (GUILayout.Button("生成图集"))
        {
            // var atlas = new SpriteAtlas();
            // AssetDatabase.CreateAsset(atlas, string.Format("{0}/{1}", CreateSpriteAtlasMgr.PATH, _atlasName));
            // CreateSpriteAtlasMgr.PATH = "";

            // //将该路径钟的图片加入图集
            // List<Object> packables = new List<Object>(atlas.GetPackables());
            // Object objs = AssetDatabase.LoadAssetAtPath(CreateSpriteAtlasMgr.PATH, typeof(Texture2D));
            // if (packables.Contains(@objs) == false)
            // {
            //     atlas.Add(new Object[] { objs });
            // }
            // Close();
            OnCreateSpriteAtlas();
        }

    }

    public void OnCreateSpriteAtlas()
    {
        //获取目标路径
        var dirInfo = new DirectoryInfo(CreateSpriteAtlasMgr.PATH);
        //用完整路径判断图集是否存在，存在则删除
        if (File.Exists(dirInfo.FullName + "/" + CreateSpriteAtlasMgr.NAME))
        {
            File.Delete(dirInfo.FullName + "/" + CreateSpriteAtlasMgr.NAME);
            File.Delete(dirInfo.FullName + "/" + CreateSpriteAtlasMgr.NAME + ".meta");
            AssetDatabase.Refresh();
        }

        //生成新的图集
        var atlas = new SpriteAtlas();
        AssetDatabase.CreateAsset(atlas, CreateSpriteAtlasMgr.PATH + "/" + CreateSpriteAtlasMgr.NAME);

        //获取所有图片
        var spriteList = new List<Object>();
        var allFiles = dirInfo.GetFiles();
        foreach (var file in allFiles)
        {
            if (file.Extension == ".png" || file.Extension == ".jpg")
            {
                var sprite = AssetDatabase.LoadAssetAtPath<Sprite>(CreateSpriteAtlasMgr.PATH + "/" + file.Name);
                spriteList.Add(sprite);
            }
        }

        //导入图集
        atlas.Add(spriteList.ToArray());
        Debug.Log("图集生成成功，路径为："+CreateSpriteAtlasMgr.PATH);

        //清除缓存
        CreateSpriteAtlasMgr.NAME = "";
        CreateSpriteAtlasMgr.PATH = "";
        spriteList.Clear();
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        //关闭窗口
        Close();
    }


}
