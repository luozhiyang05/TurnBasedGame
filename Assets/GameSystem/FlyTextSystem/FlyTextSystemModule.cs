using Assets.GameSystem.FlyTextSystem.Main;
using Framework;
using GlobalData;
using Tool.Mono;
using Tool.UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Assets.GameSystem.FlyTextSystem
{
    public interface IFlyTextSystemModule : IModule
    {
        public void ShowView();

        /// <summary>
        /// 使用弹幕
        /// </summary>
        /// <param name="leftOrRight">0为左弹幕，1为右弹幕</param>
        /// <param name="contentKey">内容key</param>
        /// <param name="stayTime">停留时间</param>
        /// <param name="flyTime">飞行一次的时间</param>
        public void FlyText(int leftOrRight, string contentKey, float stayTime, float flyTime, UnityAction action = null);

        /// <summary>
        /// 攻击文本飞
        /// </summary>
        /// <param name="defenerGo"></param>
        /// <param name="atkTxt"></param>
        public void AtkTxtFly(GameObject defenerGo, string atkTxt);
    }

    public class FlyTextSystemModule : AbsModule, IFlyTextSystemModule
    {
        private FlyTextSystemViewCtrl _viewCtrl;
        private FlyTextSystemView _view;

        protected override void OnInit()
        {
            ShowView();
        }

        public void ShowView()
        {
            _viewCtrl ??= new FlyTextSystemViewCtrl();
            _viewCtrl.ShowView(EuiLayer.SystemUI);
        }

        public void FlyText(int leftOrRight, string contentKey, float stayTime, float flyTime, UnityAction action = null)
        {
            _view ??= _viewCtrl.GetView() as FlyTextSystemView;
            float percent = 0;
            var textGo = _view.transform.Find("Main/FlyTexts/" + (leftOrRight == 0 ? "LeftText" : "RightText")).gameObject;
            textGo.transform.Find("Txt_desc").GetComponent<Text>().text = GameManager.GetText(contentKey);
            Debug.LogWarning("<size=15><color=#004FD5>（弹幕：" + GameManager.GetText(contentKey) + "）.......... </color></size>");
            float startX = textGo.transform.localPosition.x;
            float oldX = textGo.transform.localPosition.x;
            float oldY = textGo.transform.localPosition.y;
            ActionKit.GetInstance().CreateActQue(textGo, () =>
            {
                percent += Time.deltaTime / flyTime;
                textGo.transform.localPosition = new Vector3(Mathf.Lerp(oldX, oldX + (leftOrRight == 0 ? 800 : -800), percent), oldY, 0);
            }, flyTime).Append(() =>
            {
                percent = 0;
                oldX = textGo.transform.localPosition.x;
            }, stayTime).Append(() =>
            {
                percent += Time.deltaTime / flyTime;
                textGo.transform.localPosition = new Vector3(Mathf.Lerp(oldX, oldX - (leftOrRight == 0 ? 800 : -800), percent), oldY, 0);
            }, flyTime).Append(() =>
            {
                textGo.transform.localPosition = new Vector3(startX, oldY, 0);
            }, 0).Append(() =>
            {
                action?.Invoke();
            }, 0).Execute();
        }

        public void AtkTxtFly(GameObject defenerGo, string atkTxt)
        {
            _view ??= _viewCtrl.GetView() as FlyTextSystemView;
            _view.AtkTxtFly(defenerGo,atkTxt);
        }
    }
}