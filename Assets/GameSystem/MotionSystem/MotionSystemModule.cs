using System;
using Assets.GameSystem.BattleSystem.Scripts;
using Assets.GameSystem.SkillSystem;
using Framework;
using GlobalData;
using Tool.Mono;
using Tool.ResourceMgr;
using UnityEngine;
using UnityEngine.UI;
namespace Assets.GameSystem.MotionSystem
{
    public interface IMotionSystemModule : IModule
    {
        void AttackAct(GameObject atker, GameObject defener, float durationTime, float stayTime, Action finishAct = null);
        void CamareShake(float durationTime, float shakeForce);
        void SkillTip(GameObject unitGo, int skillId);
    }

    public class MotionSystemModule : AbsModule, IMotionSystemModule
    {
        protected override void OnInit()
        {
        }

        public void AttackAct(GameObject atker, GameObject defener, float durationTime, float stayTime, Action finishAct = null)
        {
            // 将攻击者和防御者按照顺序移入攻击结点
            Transform atkCode = GameObject.Find("AtkCode").transform;
            var absUnitsTrans = atker.transform.parent;
            var enemyParent = defener.transform.parent;
            var defenId = defener.transform.Find("body").GetComponent<AbsUnit>().id;
            defener.transform.SetParent(atkCode);
            atker.transform.SetParent(atkCode);

            // 玩家移动到敌人身上
            float percent = 0f;
            float atkPosX = atker.transform.localPosition.x;
            float atkPosY = atker.transform.localPosition.y;
            float defPosX = defener.transform.localPosition.x;
            float defPosY = defener.transform.localPosition.y;
            ActionKit.GetInstance().CreateActQue(atker, () =>
            {
                percent += Time.deltaTime / durationTime;
                atker.transform.localPosition = new Vector3(Mathf.Lerp(atkPosX, defPosX, percent), Mathf.Lerp(atkPosY, defPosY, percent), 0);
            }, durationTime)
            // 进度重置，攻击逻辑
            .Append(() =>
            {
                percent = 0;
                finishAct?.Invoke();
                // 相机震动
                CamareShake(GameManager.atkCameraShakeDurationTime, GameManager.atkCameraShakeForce);
            }, 0f)
            // 等待
            .Append(() => { }, stayTime)
            // 攻击者位置归位
            .Append(() =>
            {
                percent += Time.deltaTime / durationTime;
                atker.transform.localPosition = new Vector3(Mathf.Lerp(defPosX, atkPosX, percent), Mathf.Lerp(defPosY, atkPosY, percent), 0);
            }, durationTime)
            // 将防御者和攻击者节点归位
            .Append(() =>
            {
                atker.transform.SetParent(absUnitsTrans);
                defener.transform.SetParent(enemyParent);
                defener.transform.SetSiblingIndex(defenId - 1);
            }, 0f)
            .Execute();
        }

        public void CamareShake(float durationTime, float shakeForce)
        {
            // 用背景震动替代相机，因为单位是UI制作，没法震动相机
            var cameraTrans = GameObject.Find("Img_bg").transform;
            var oldCameraPos = cameraTrans.position;
            ActionKit.GetInstance().CreateActQue(cameraTrans.gameObject, () =>
            {
                cameraTrans.position = oldCameraPos + (Vector3)(UnityEngine.Random.insideUnitCircle * shakeForce);
            }, durationTime)
            .Append(() =>
            {
                cameraTrans.position = oldCameraPos;
            }, 0f)
            .Execute();
        }

        public void SkillTip(GameObject unitGo,int skillId)
        {
            var imgTrans = unitGo.transform.Find("body").Find("img_skillTip");
            var img = imgTrans.GetComponent<Image>();
            var skillIconName = this.GetSystem<ISkillSystemModule>().GetSkillIconName(skillId);
            var percent = 0f;
            ActionKit.GetInstance().CreateActQue(imgTrans.gameObject,()=>{
                ResMgr.GetInstance().AsyncLoad<Sprite>(GameManager.SkillIconPath + skillIconName, (sprite) => { img.sprite = sprite; }, false);
                imgTrans.localPosition = Vector3.zero;
                imgTrans.gameObject.SetActive(true);
                img.color = new Color(1, 1, 1, 1);
            },0f)
            .Append(()=>{
                percent += Time.deltaTime / GameManager.skillTipFlyDurationTime;
                imgTrans.localPosition = new Vector3(0, Mathf.Lerp(0, 200, percent), 0);
            },GameManager.skillTipFlyDurationTime)
            .Append(()=>{
                percent = 0f;
                imgTrans.localPosition = new Vector3(0, 200, 0);
            },0f)
            .Append(()=>{

            },GameManager.skillTipStayTime)
            .Append(()=>{
                percent += Time.deltaTime / GameManager.skillTipFadeTime;
                img.color = new Color(1, 1, 1, Mathf.Lerp(1, 0, percent));
            },GameManager.skillTipFadeTime)
            .Execute();
        }
    }
}