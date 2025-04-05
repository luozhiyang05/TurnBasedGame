using GameSystem.MVCTemplate;
using GlobalData;
using Tool.Mono;
using Tool.Utilities;
using Tool.Utilities.Events;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.GameSystem.FlyTextSystem.Main
{
    public class FlyTextSystemView : BaseView
    {
        #region 自动生成UI组件区域，内部禁止手动更改！
        protected override void AutoInitUI()
        {
        }
        #endregion 自动生成UI组件区域结束！

        #region 遮罩相关
        // /// <summary>
        // /// 是否启用MaskPanel，启用的话只需要取消注释
        // /// </summary>
        // /// <returns></returns>
        // public override bool MaskPanel()
        // {
        //     return true;
        // }

        // /// <summary>
        // /// 是否开启点击遮罩关闭View，启用的话只需要取消注释
        // /// </summary>
        // /// <returns></returns>
        // public override bool ClickMaskPanel()
        // {
        //     return true;
        // }

        // /// <summary>
        // /// 是否重写遮罩事件，重写后不执行父类点击遮罩关闭事件
        // /// </summary>
        // /// <returns></returns>
        // public override void OnClickMaskPanel()
        // {
        //     Debug.Log("点击了遮罩！");
        // }
        #endregion

        public Transform flyTxtTemp;

        /// <summary>
        /// 绑定model回调事件
        /// </summary>
        protected override void BindModelListener()
        {
        }

        /// <summary>
        /// 初始化,时机在Awake中
        /// </summary>
        protected override void OnInit()
        {

        }


        public override void OnShow()
        {
            base.OnShow();
        }

        public override void OnHide()
        {
            base.OnHide();
        }

        public override void OnRelease()
        {
            base.OnRelease();
        }

        public void AtkTxtFly(GameObject defenerGo, string atkTxt)
        {
            var txt = flyTxtTemp.GetComponent<Text>();
            flyTxtTemp.gameObject.SetActive(true);
            flyTxtTemp.position = defenerGo.transform.position;
            txt.text = atkTxt;
            txt.color = Color.red;
            var targetPosY = defenerGo.transform.position.y + GameManager.atkTxtFlyHight;
            var oldPosY = flyTxtTemp.position.y;
            var percent = 0f;
            ActionKit.GetInstance().CreateActQue(flyTxtTemp.gameObject, () =>
            {
                percent += Time.deltaTime / GameManager.atkTxtFlyDurationTime;
                flyTxtTemp.position = new Vector3(flyTxtTemp.position.x, Mathf.Lerp(oldPosY, targetPosY, percent), flyTxtTemp.position.z);
            }, GameManager.atkTxtFlyDurationTime)
            .Append(() => { }, GameManager.aykTxtStayTime)
            .Append(() => { percent = 0; }, 0)
            .Append(() =>
            {
                percent += Time.deltaTime / GameManager.aykTxtFadeTime;
                txt.color = new Color(txt.color.r, txt.color.g, txt.color.b, Mathf.Lerp(1, 0, percent));
            }, GameManager.aykTxtFadeTime)
            .Append(() => { flyTxtTemp.gameObject.SetActive(false); }, 0).Execute();
        }

    }
}