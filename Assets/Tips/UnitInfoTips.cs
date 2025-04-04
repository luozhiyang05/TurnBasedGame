using Assets.GameSystem.BattleSystem.Scripts;
using Assets.GameSystem.CardSystem.Scripts;
using Assets.GameSystem.EffectsSystem;
using Assets.GameSystem.SkillSystem;
using Framework;
using GameSystem.MVCTemplate;
using GlobalData;
using Tool.ResourceMgr;
using UnityEngine;
using UnityEngine.UI;

namespace Tips
{
    public class UnitInfoTips : BaseTips
    {
        public Text Txt_name;
        public Text Txt_hp;
        public Text Txt_armor;
        public Text Txt_atk;
        public Text Txt_skillName;
        public Text Txt_skillDesc;
        public Text Txt_effectTip;
        public Transform effectInfos;
        private UnitInfoPacking unitInfoPacking;

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
            unitInfoPacking = args[0] as UnitInfoPacking;

            // 基础信息多语言
            Txt_name.text = GameManager.GetText("unit_info_1001") + GameManager.GetText(unitInfoPacking.unitName);
            Txt_hp.text = GameManager.GetText("unit_info_1002") + unitInfoPacking.maxHp + "/" + unitInfoPacking.nowHp;
            Txt_armor.text = GameManager.GetText("unit_info_1003") + unitInfoPacking.armor;
            Txt_atk.text = GameManager.GetText("unit_info_1004") + unitInfoPacking.atk;
            Txt_skillName.text = GameManager.GetText("unit_info_1005");
            Txt_effectTip.text = GameManager.GetText("unit_info_1006");

            // 技能描述多语言
            var skillData = ResMgr.GetInstance().SyncLoad<SkillsSo>("技能库").GetSkillDataById(unitInfoPacking.skillId);
            Txt_skillDesc.text = GameManager.GetText(skillData.desc);

            // 效果描述
            var hadEffect = unitInfoPacking.effectIds.Count > 0;
            effectInfos.gameObject.SetActive(hadEffect);
            if (hadEffect)
            {
                for (int i = 1; i < effectInfos.childCount; i++)
                {
                    var needDisplay = i <= unitInfoPacking.effectIds.Count;
                    var effIcon = effectInfos.GetChild(i);
                    effIcon.gameObject.SetActive(needDisplay);
                    if (needDisplay)
                    {
                        // 效果点击
                        var index = i - 1;
                        effIcon.GetComponent<Button>().onClick.RemoveAllListeners();
                        effIcon.GetComponent<Button>().onClick.AddListener(() =>
                        {
                            TipsModule.DescTips(this.GetSystem<IEffectsSystemModule>().GetBaseEffectById(unitInfoPacking.effectIds[index]).effDesc);
                        });

                        // 效果图片
                        var iconName = this.GetSystem<IEffectsSystemModule>().GetBaseEffectById(unitInfoPacking.effectIds[index]).iconName;
                        ResMgr.GetInstance().AsyncLoad<Sprite>(GameManager.EffectIconPath + iconName, (sprite) =>
                        {
                            effIcon.GetComponent<Image>().sprite = sprite;
                        }, false);
                    }
                }
            }
        }

        public override void OnRelease()
        {

        }
    }
}