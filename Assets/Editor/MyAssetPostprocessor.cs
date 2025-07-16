using System.IO;
using Tool.Mono;
using UnityEditor;
using UnityEditor.U2D;
using UnityEngine;
using UnityEngine.U2D;

namespace Editor
{
    public class MyAssetPostprocessor : AssetPostprocessor
    {
        //Assets/GameSystem/TestSystem/Images/测试图片.png
        public const string GameDir = "Assets/GameSystem/";
        public const string Images = "Images";
        public const string NoPack = "NoPack";
        public const string SpriteAtlas = ".spriteatlas";

        //在Texture导入前设置，对纹理的设置一般放在这里
        void OnPreprocessTexture()
        {
            var importer = assetImporter as TextureImporter;
            var path = assetPath;   //相对于Assets的路径

            //在特定的文件夹下才处理
            if (!path.Contains(GameDir)) return;
            var pathStrs = path.Split('/');
            if (!Images.Equals(pathStrs[^2]) && !NoPack.Equals(pathStrs[^2])) return;

            //设置纹理为Sprite和不压缩
            importer.textureType = TextureImporterType.Sprite;
            importer.textureCompression = TextureImporterCompression.Uncompressed;


            AssetDatabase.SaveAssets();
        }

        //在纹理导入后处理
        void OnPostprocessTexture(Texture2D texture2D)
        {
            var path = assetPath;

            //在特定的文件夹下才处理
            if (!path.Contains(GameDir)) return;
            var pathStrs = path.Split('/');
            if (!Images.Equals(pathStrs[^2])) return;

            //导入图集
            path = path.Substring(0, path.LastIndexOf('/') + 1) + pathStrs[2] + SpriteAtlas;
            Debug.Log(path);
            if (File.Exists(path))
            {
               
            }
            else
            {
                //生成新的图集
                var spriteatlas = new SpriteAtlas();
                AssetDatabase.CreateAsset(spriteatlas, path);
            }
        }
    }
}