using System.IO;
using Tool.Mono;
using UnityEditor;
using UnityEditor.U2D;
using UnityEngine;
using UnityEngine.U2D;

public class MyAssetPostprocessor : AssetPostprocessor
{
    //Assets/GameSystem/TestSystem/Images/测试图片.png
    public const string GameDir = "Assets/GameSystem/";
    public const string Images = "Images";
    public const string NoPack = "NoPack";
    public const string SpriteAtlas = ".spriteatlas";

    static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {
        SpriteAtlasImpoter.OnPostprocessAllAssets(importedAssets, deletedAssets, movedAssets, movedFromAssetPaths);
    }
}
