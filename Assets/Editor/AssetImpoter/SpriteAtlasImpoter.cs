using System.Collections.Generic;
using Editor;
using Unity.Plastic.Antlr3.Runtime;
using UnityEditor;
using UnityEditor.U2D;
using UnityEngine;
using UnityEngine.U2D;

public class SpriteAtlasImpoter
{
    public const string SpriteAtlasFileSuffix = ".spriteatlas";
    public const string Images = "Images";
    public static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {
        for (int i = 0; i < importedAssets.Length; i++)
        {
            ProcessSpriteAtlasPath(importedAssets[i]);
        }
    }

    private static void ProcessSpriteAtlasPath(string assetPath)
    {
        if (IsValidDir(assetPath)) return;

        // Assets/GameSystem/TestSystem/Images/测试图片 1.png
        var altasName = GetAtlasName(assetPath);
        var atlasPath = GetAtlasPath(assetPath, altasName);
        ProcessSpriteAltasPacking(atlasPath, assetPath);
    }

    private static void ProcessSpriteAltasPacking(string atlasPath, string assetPath)
    {
        var atlas = AssetDatabase.LoadAssetAtPath<SpriteAtlas>(atlasPath);
        var sprite = AssetDatabase.LoadAssetAtPath<Sprite>(assetPath);
        if (atlas == null)
        {
            atlas = TryCreateSpriteAtlas(atlasPath);
            ApplyNormalSetting(atlas);
            AssetBundleTool.SetOneAssetAbName(atlasPath);
        }

        HashSet<string> spritePaths = new HashSet<string>();
        Object[] objects = atlas.GetPackables();
        for (int i = 0; i < objects.Length; i++)
        {
            string spritePath = AssetDatabase.GetAssetPath(objects[i]);
            spritePaths.Add(spritePath);
        }
        if (!spritePaths.Contains(assetPath))
        {
            atlas.Add(new Object[] { sprite });
        }
    }

    private static SpriteAtlas TryCreateSpriteAtlas(string atlasPath)
    {
        //生成新的图集
        var spriteatlas = new SpriteAtlas();
        AssetDatabase.CreateAsset(spriteatlas, atlasPath);
        return AssetDatabase.LoadAssetAtPath<SpriteAtlas>(atlasPath);
    }

    private static bool IsValidDir(string assetPath)
    {
        //非Images文件夹下纹理不处理
        var path = assetPath;
        var pathStrs = path.Split('/');
        return !Images.Equals(pathStrs[^2]) || assetPath.EndsWith(SpriteAtlasFileSuffix);
    }

    private static string GetAtlasName(string assetPath)
    {
        var altasName = assetPath.Split('/')[2];
        return altasName;
    }

    private static string GetAtlasPath(string assetPath, string altasName)
    {
        var path = assetPath.Substring(0, assetPath.LastIndexOf('/') + 1) + altasName + SpriteAtlasFileSuffix;
        return path;
    }

    private static void ApplyNormalSetting(SpriteAtlas atlas)
    {
        var packingSettings = atlas.GetPackingSettings();
        packingSettings.enableRotation = false;
        packingSettings.enableTightPacking = false;
        atlas.SetPackingSettings(packingSettings);
    }
}