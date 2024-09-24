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
        public void SetData(string title, string content,UnityAction comfirm, UnityAction cancel)
        {
            SetAudio("Test","Test");
            _title = title;
            _content = content;
            _comfirm = comfirm;
            _cancel = cancel;
        }
        protected override void Init()
        {
            txtTitle.text = _title;
            txtContent.text = _content;
            btnComfirm.Label.text = "确定";
            btnCancel.Label.text = "取消";
        }

        public override void OnShow()
        {
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
    }
}