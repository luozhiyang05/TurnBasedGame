using UIComponents;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Editor
{
    public static class CreateUIComponentsTools
    {
        [MenuItem("GameObject/Custom UI/BaseVIew",false,1)]
        public static void CreateUIComponents()
        {
            //寻找场景中的画布
            GameObject _canvasGo = GameObject.Find("Canvas");
            if (_canvasGo == null)
            {
                _canvasGo = new GameObject("Canvas");
                RectTransform canvasRect = _canvasGo.AddComponent<RectTransform>();
                Canvas canvas = _canvasGo.AddComponent<Canvas>();
                CanvasScaler canvasScaler = _canvasGo.AddComponent<CanvasScaler>();
                _canvasGo.AddComponent<GraphicRaycaster>();
       
                //设置值
                canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
                canvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.Expand;
                canvasScaler.referenceResolution = new Vector2(1920, 1080);

                canvasRect.localPosition = Vector3.zero;
                canvasRect.localScale = Vector3.one;
                canvasRect.offsetMax = Vector2.zero;
                canvasRect.offsetMin = Vector2.zero;
            }

            _canvasGo.GetComponent<Canvas>().sortingOrder = -1;

            //生成BaseView
            GameObject baseViewGo = new GameObject("BaseView");
            baseViewGo.AddComponent<CanvasGroup>();
            RectTransform rectTransform = baseViewGo.AddComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(1920, 1080);
            //生成Main
            GameObject main = new GameObject("Main");
            main.AddComponent<RectTransform>();
            main.transform.SetParent(rectTransform);

            //设置父类
            baseViewGo.transform.SetParent(_canvasGo.transform);
            baseViewGo.transform.localPosition = Vector3.zero;

            //注册
            Undo.RegisterCreatedObjectUndo(baseViewGo, "Create" + baseViewGo.name);//注册到Undo系统,允许撤销
            Selection.activeObject = baseViewGo;//将新建物体设为当前选中物体
        }

        [MenuItem("GameObject/Custom UI/CButton",false,2)]
        public static void CreateCButton(MenuCommand menuCommand)
        {
            GameObject cBtnGo = new GameObject("Btn_");

            //按钮本身组件
            RectTransform rectTransform = cBtnGo.AddComponent<RectTransform>();
            cBtnGo.AddComponent<CanvasRenderer>();
            Image image = cBtnGo.AddComponent<Image>();
            image.raycastTarget = true;
            CButton cButton = cBtnGo.AddComponent<CButton>();
            cButton.targetGraphic = image;
            rectTransform.sizeDelta = new Vector2(200, 50);

            //text
            GameObject textGo = new GameObject("text");
            RectTransform textGoRect = textGo.AddComponent<RectTransform>();
            textGoRect.sizeDelta = new Vector2(200, 50);
            Text text = textGo.AddComponent<Text>();
            text.text = "按钮文本";
            text.fontSize = 28;
            text.color = Color.black;
            text.alignment = TextAnchor.MiddleCenter;


            textGo.transform.SetParent(rectTransform);
            textGo.transform.localPosition = Vector3.zero;

            GameObjectUtility.SetParentAndAlign(cBtnGo, menuCommand.context as GameObject);//设置父节点为当前选中物体
            Undo.RegisterCreatedObjectUndo(cBtnGo, "Create：" + "CButton");//注册到Undo系统,允许撤销
            Selection.activeObject = cBtnGo;//将新建物体设为当前选中物体
        }
    }
}