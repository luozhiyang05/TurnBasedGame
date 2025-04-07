using GameSystem.MVCTemplate;
using GlobalData;
using UnityEngine.UI;

namespace Tips
{
    public class InfoTips : BaseTips
    {
        public Image Img_icon;
        public Text Txt_name;
        public Text Txt_desc;

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
        public override bool ClickMaskPanel()
        {
            return true;
        }
        #endregion

        protected override void Init()
        {

        }

        protected override void OnOpen(params object[] args)
        {
            var infosPacking = args[0] as InfosPacking;
            Img_icon.sprite = infosPacking.sprite;
            Txt_name.text = GameManager.GetText(infosPacking.name);
            Txt_desc.text = GameManager.GetText(infosPacking.desc);
        }


        public override void OnRelease()
        {

        }
    }
}