using System;
using System.Collections;
using Tool.Single;
using Tool.Utilities;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Tool.ResourceMgr
{
    public class ResMgr : MonoSingleton<ResMgr>
    {
        private IEnumerator IAsyncLoadRes<T>(string resName, Action<T> callBack, bool isInstantiate = true) where T : Object
        {
            var rq = Resources.LoadAsync<T>(resName);
            while (!rq.isDone) yield return null;

            //异步加载，直到加载完毕才调用回调方法
            callBack.Invoke((rq.asset is GameObject && isInstantiate) ? GameObject.Instantiate(rq.asset) as T : rq.asset as T);
        }

        /// <summary>
        /// 同步加载Resources下的资源
        /// </summary>
        /// <param name="resName"></param>
        /// <param name="isInstantiate"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T SyncLoad<T>(string resName, bool isInstantiate = true) where T : Object
        {
            T load = Resources.Load<T>(resName);

            //判断是否属于unity的GameObject，如果属于则实例化一个GameObject，如果不属于则返回T
            return (load is GameObject && isInstantiate) ? GameObject.Instantiate(load) : load;
        }

        /// <summary>
        /// 异步加载Resources下的资源
        /// </summary>
        /// <param name="name"></param>
        /// <param name="callBack"></param>
        /// <param name="isInstantiate"></param>
        /// <typeparam name="T"></typeparam>
        public void AsyncLoad<T>(string name, Action<T> callBack, bool isInstantiate = true) where T : Object
        {
            StartCoroutine(IAsyncLoadRes<T>(name, callBack, isInstantiate));
        }

        /// <summary>
        /// 卸载Resources未占用的资源
        /// </summary>
        public void Clear()
        {
            //卸载未占用的asset资源
            Resources.UnloadUnusedAssets();
            //GC回收
            GC.Collect();
        }

        /// <summary>
        /// 卸载未使用的资源
        /// </summary>
        /// <param name="obj"></param>
        public void UnloadAsset(Object obj)
        {
            Resources.UnloadAsset(obj);
        }

        /// <summary>
        /// 加载视图
        /// </summary>
        /// <param name="path"></param>
        /// <param name="callback"></param>
        /// <exception cref="Exception"></exception>
        public void LoadView(string path, Action<GameObject> callback)
        {
#if UNITY_EDITOR
            var realPath = string.Format("{0}/{1}.prefab", "Assets/GameSystem", path);
            GameObject viewObj = AssetDatabase.LoadAssetAtPath<GameObject>(realPath);
            if (viewObj == null) throw new Exception($"加载UI失败：{realPath}");
            callback(GameObject.Instantiate(viewObj));
#else

            var viewName = PathUtils.GetViewNameFromAbPath(path);
            AssetBundleMgr.GetInstance().LoadResAsync<GameObject>(path, viewName, (viewObj) =>
            {
               callback(GameObject.Instantiate(viewObj));
            });
#endif
        }


        /// <summary>
        /// 加载资源
        /// </summary>
        /// <param name="systemName"></param>
        /// <param name="assetName"></param>
        /// <param name="callback"></param>
        public GameObject LoadAsset(string systemName, string assetName, bool fromGameSystem = true)
        {
#if UNITY_EDITOR
            string realPath = "";
            if (fromGameSystem)
                realPath = string.Format("{0}/{1}/{2}.prefab", "Assets/GameSystem", systemName, assetName);
            else
                realPath = string.Format("Assets/Wights/{0}.prefab", assetName);
            GameObject go = AssetDatabase.LoadAssetAtPath<GameObject>(realPath);
            if (go == null) throw new Exception($"加载资源失败：{realPath}");
            return go;
#else

#endif
        }

        /// <summary>
        /// 加载工具资源
        /// </summary>
        /// <param name="systemName"></param>
        /// <param name="assetName"></param>
        /// <param name="fromGameSystem"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public GameObject LoadToolAsset(string toolAssetPath)
        {
#if UNITY_EDITOR
            string realPath = string.Format("Assets/Tool/{0}", toolAssetPath);
            GameObject go = AssetDatabase.LoadAssetAtPath<GameObject>(realPath);
            if (go == null) throw new Exception($"加载资源失败：{realPath}");
            return go;
#else

#endif
        }
    }
}