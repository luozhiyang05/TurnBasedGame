using GameSystem.MVCTemplate;
using UIComponents;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Tips
{
    public class ComfirmTips : BaseTips
    {
        public Text txtTitle;
        public Text txtContent;
        public CButton btnComfirm;
        private UnityAction _comfirm;

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
            btnComfirm.onClick.AddListener(() =>
            {
                _comfirm?.Invoke();
                OnHide();
            });
        }

        protected override void OnOpen(params object[] args)
        {
            SetAudio("Test", "Test");
            _comfirm = args[2] as UnityAction;
            txtTitle.text = args[0] as string;
            txtContent.text = args[1] as string;
            //TODO:多语言
            btnComfirm.Label.text = "确定";
        }

        public override void OnRelease()
        {

        }
    }
}