using GameSystem.MVCTemplate;
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
        private UnityAction _comfirm;
        public CButton btnCancel;
        private UnityAction _cancel;
        private string _title;
        private string _content;
        
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
            txtTitle.text = _title;
            txtContent.text = _content;
            btnComfirm.Label.text = "确定";
            btnCancel.Label.text = "取消";
            btnComfirm.onClick.AddListener(() =>
            {
                _comfirm?.Invoke();
                OnHide();
            });
            btnCancel.onClick.AddListener(() =>
            {
                _cancel?.Invoke();
                OnHide();
            });
        }

        protected override void OnOpen(params object[] args)
        {
            SetAudio("Test","Test");
            _title = args[0] as string;
            _content = args[1] as string;
            _comfirm = args[2] as UnityAction;
            _cancel = args[3] as UnityAction;
        }

        public override void OnRelease()
        {
            
        }
    }
}