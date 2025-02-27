using GameSystem.MVCTemplate;
using GlobalData;
using UIComponents;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Tips
{
    public class ReConfirmTips : BaseTips
    {
        public Text txtTitle;
        public Text txtContent;
        public CButton btnComfirm;
        public CButton btnCancel;
        #region 遮罩相关
        /// <summary>
        /// 是否启用MaskPanel，启用的话只需要取消注释
        /// </summary>
        /// <returns></returns>
        public override bool MaskPanel()
        {
            return true;
        }

        /// <summary>
        /// 是否开启点击遮罩关闭View，启用的话只需要取消注释
        /// </summary>
        /// <returns></returns>
        // public override bool ClickMaskPanel()
        // {
        //     return true;
        // }
        #endregion

        protected override void Init()
        {

        }

        protected override void OnOpen(params object[] args)
        {
            SetAudio("Test", "Test");

            txtTitle.text = GameManager.GetText(args[0] as string);
            txtContent.text = GameManager.GetText(args[1] as string);
            btnComfirm.Label.text = GameManager.GetText("tips_1003");
            btnCancel.Label.text = GameManager.GetText("tips_1005");

            btnComfirm.onClick.RemoveAllListeners();
            btnCancel.onClick.RemoveAllListeners();
            btnComfirm.onClick.AddListener(() =>
            {
                (args[2] as UnityAction)?.Invoke();
                OnHide();
            });
            btnCancel.onClick.AddListener(() =>
            {
                (args[3] as UnityAction)?.Invoke();
                OnHide();
            });
        }

        public override void OnRelease()
        {

        }
    }
}