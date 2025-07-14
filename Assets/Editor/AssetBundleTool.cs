using System.IO;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class AssetBundleTool : EditorWindow
    {
        private int _toogleIdx = 0;
        private static Object _selectAsset;
        private static bool _isUseCustomName = false;
        private static string _assetName;
        private static string END_NAME = "_ui_prefab";
        private static string _resultPath;
        private static string _defalutName;
        private static Vector2 _windowPos = Vector2.zero;
        private static Vector2 _windowRect = new Vector2(500, 300);

        [MenuItem("Assets/设置资源AssetBundleName", false, -1)]
        public static void OpenWindow()
        {
            var assets = Selection.GetFiltered<Object>(SelectionMode.Assets);
            if (assets.Length > 0)
            {
                _selectAsset = assets[0];
                GetWindowWithRect<AssetBundleTool>(new Rect(_windowPos, _windowRect), false, "AssetBundleTool");
            }
        }

        void OnEnable()
        {
            var tempPath = AssetDatabase.GetAssetPath(_selectAsset);
            _toogleIdx = Directory.Exists(tempPath) ? 0 : 1;
            tempPath = tempPath.Substring(tempPath.IndexOf('/') + 1);
            tempPath = tempPath.Substring(0, tempPath.LastIndexOf('/') + 1);
            _resultPath = _toogleIdx == 0 ? tempPath.ToLower() : tempPath.Substring(0,tempPath.IndexOf('/') + 1).ToLower();
            _assetName = _selectAsset.name;
            _defalutName = _selectAsset.name.ToLower() + END_NAME;
        }

        private void OnGUI()
        {
            _toogleIdx = GUILayout.Toolbar(_toogleIdx, new string[] { "设置文件夹下所有预制体", "设置单个资源" });

            _isUseCustomName = EditorGUILayout.Toggle("使用自定义名称:", _isUseCustomName);

            if (_isUseCustomName)
            {
                _assetName = EditorGUILayout.TextField("AssetBundleName:", _assetName);
            }
            else
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("AssetBundleName:", GUILayout.MaxWidth(150));
                GUILayout.Label(_defalutName);
                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("AssetBundlePath：", GUILayout.MaxWidth(150));
            var style = new GUIStyle();
            style.normal.textColor = Color.red;
            GUILayout.Label(_resultPath + (_isUseCustomName ? _assetName : _defalutName).ToLower(), style);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();

            if (_toogleIdx == 0)
            {
                if (GUILayout.Button("设置所有资源AssetBundleName"))
                {
                    SetAllAssetsAssetBundleName();
                }
            }
            else
            {
                if (GUILayout.Button("设置单个资源AssetBundleName"))
                {
                    SetOneAssetAssetBundleName();
                }
            }
            if (GUILayout.Button("清除所有未被使用的AssetBundleName"))
            {
                AssetDatabase.RemoveUnusedAssetBundleNames();
                Debug.Log("清除完成");
                Close();
            }
            if (GUILayout.Button("清除所有AssetBundleName"))
            {
                ClearAllAssetsAssetBundleName();
            }
        }
        private void ClearAllAssetsAssetBundleName()
        {
            int length = AssetDatabase.GetAllAssetBundleNames().Length;
            string[] oldAssetBundleNames = new string[length];
            for (int i = 0; i < length; i++)
            {
                oldAssetBundleNames[i] = AssetDatabase.GetAllAssetBundleNames()[i];
            }

            for (int j = 0; j < oldAssetBundleNames.Length; j++)
            {
                AssetDatabase.RemoveAssetBundleName(oldAssetBundleNames[j], true);
            }
            Debug.Log("清除完成");
        }
        private void SetAllAssetsAssetBundleName()
        {
            var path = AssetDatabase.GetAssetPath(_selectAsset);
            DirectoryInfo dir = new DirectoryInfo(path);
            FileInfo[] fileInfos = dir.GetFiles();
            foreach (var fileInfo in fileInfos)
            {
                if (fileInfo.Name.EndsWith(".prefab"))
                {
                    var assetImport = AssetImporter.GetAtPath(fileInfo.FullName.Substring(fileInfo.FullName.IndexOf("Assets")));
                    assetImport.assetBundleName = _resultPath + (_isUseCustomName ? _assetName : _defalutName).ToLower();
                }
            }
            Debug.Log("设置完成");
            Close();
        }

        private void SetOneAssetAssetBundleName()
        {
            var assetImport = AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(_selectAsset));
            assetImport.assetBundleName = _resultPath + (_isUseCustomName ? _assetName : _defalutName).ToLower();
            Debug.Log("设置完成");
            Close();
        }

        private void OnInspectorUpdate()
        {
            Repaint();
        }
    }
}