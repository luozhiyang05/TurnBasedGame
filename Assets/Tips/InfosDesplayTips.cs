using Assets.GameSystem.BattleSystem.Scripts;
using Assets.GameSystem.CardSystem.Scripts;
using Assets.GameSystem.EffectsSystem;
using Assets.GameSystem.SkillSystem;
using Framework;
using GameSystem.MVCTemplate;
using GlobalData;
using Tool.ResourceMgr;
using Tool.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace Tips
{
    public class InfosPacking
    {
        public Sprite sprite;
        public string name;
        public string desc;
    }
    public class InfosDesplayTips : BaseTips
    {
        private QArray<InfosPacking> infosQarray;
        public Transform content;
        public Transform girdTemp;
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
            infosQarray = args[0] as QArray<InfosPacking>;

            for (int i = 0; i < infosQarray.Count; i++)
            {
                Transform temp = content.childCount <= i + 1 ? Instantiate(girdTemp, content) : content.GetChild(i + 1);
                temp.gameObject.SetActive(true);
                SetInfoGird(temp,infosQarray[i]);
                var index = i;
                temp.GetChild(0).GetComponent<Button>().onClick.AddListener(() =>
                {
                    TipsModule.InfoTips(infosQarray[index]);
                });
            }
        }

        public override void OnHide()
        {
            base.OnHide();
            for (int i = 0; i < content.childCount; i++)
            {
                var temp = content.GetChild(i);
                temp.gameObject.SetActive(false);
                temp.GetChild(0).GetComponent<Button>().onClick.RemoveAllListeners();
            }
        }

        public void SetInfoGird(Transform gird,InfosPacking infosPacking)
        {
            var txt_name = gird.GetChild(0).Find("Txt_name").GetComponent<Text>();
            var img_icon = gird.GetChild(0).Find("Img_icon").GetComponent<Image>();
            img_icon.sprite = infosPacking.sprite;
            txt_name.text = GameManager.GetText(infosPacking.name);
        }

        public override void OnRelease()
        {

        }
    }
}