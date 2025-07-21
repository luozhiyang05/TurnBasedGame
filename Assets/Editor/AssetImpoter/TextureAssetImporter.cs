using UnityEditor;
using UnityEngine;

public class TextureAssetImporter : AssetPostprocessor
{
    public const string GameDir = "Assets/GameSystem/";
    public const string Images = "Images";
    public const string NoPack = "NoPack";
    public const string SpriteAtlas = ".spriteatlas";

    //在Texture导入前设置，对纹理的设置一般放在这里
    void OnPreprocessTexture()
    {
        //获取对应的Importer
        var importer = assetImporter as TextureImporter;
        var path = assetPath;   

        //在特定的文件夹下才处理
        if (!path.Contains(GameDir)) return;
        var pathStrs = path.Split('/');
        if (!Images.Equals(pathStrs[^2]) && !NoPack.Equals(pathStrs[^2])) return;

        //设置纹理为Sprite和不压缩
        importer.textureType = TextureImporterType.Sprite;
        importer.textureCompression = TextureImporterCompression.Uncompressed;
    }

    void OnPostprocessTexture(Texture2D texture)
    {
        //非Images文件夹下纹理不处理
        var path = assetPath;
        var pathStrs = path.Split('/');
        if (!Images.Equals(pathStrs[^2])) return;

        //判断该纹理是否需要放入NoPack
        var importer = assetImporter as TextureImporter;
        if (importer.textureType != TextureImporterType.Sprite) return;
        if (texture.width > 1024 || texture.height > 1024)
        {
            Debug.LogWarning($"{assetPath} 图片宽或高大于1024,建议放到NoPack或者RawImage中");
        }
        else if (texture.width * texture.height > 400 * 400)
        {
            Debug.LogWarning($"{assetPath} 图片尺寸大于400*400,建议放到NoPack或者RawImage中");
        }
    }

}