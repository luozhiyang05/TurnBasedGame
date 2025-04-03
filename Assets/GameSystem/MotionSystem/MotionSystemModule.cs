using System;
using Assets.GameSystem.BattleSystem.Scripts;
using Framework;
using Tool.Mono;
using UnityEngine;
namespace Assets.GameSystem.MotionSystem
{
    public interface IMotionSystemModule: IModule
    {
        void AttackAct(GameObject atker, GameObject defener, float durationTime, float stayTime, Action finishAct = null, bool isPlayerAtk = true);
    }

    public class MotionSystemModule : AbsModule, IMotionSystemModule
    {
        protected override void OnInit()
        {
        }

        public void AttackAct(GameObject atker, GameObject defener, float durationTime, float stayTime, Action finishAct = null, bool isPlayerAtk = true)
        {
            if (isPlayerAtk)
            {
                // 将攻击者和防御者按照顺序移入攻击结点
                Transform atkCode = GameObject.Find("AtkCode").transform;
                var absUnitsTrans = atker.transform.parent;
                var enemyParent = defener.transform.parent;
                defener.transform.SetParent(atkCode);
                atker.transform.SetParent(atkCode);

                // 玩家移动到敌人身上
                float percent = 0f;
                float atkPosX = atker.transform.localPosition.x;
                float atkPosY = atker.transform.localPosition.y;
                float defPosX = defener.transform.localPosition.x;
                float defPosY = defener.transform.localPosition.y;
                ActionKit.GetInstance().CreateActQue(atker,()=>{
                    percent += Time.deltaTime / durationTime;
                    atker.transform.localPosition = new Vector3(Mathf.Lerp(atkPosX, defPosX, percent), Mathf.Lerp(atkPosY, defPosY, percent), 0);
                },durationTime)
                // 进度重置，攻击逻辑
                .Append(()=>{
                    percent = 0;
                    finishAct?.Invoke();
                }, 0f)
                // 等待0.5秒
                .Append(()=>{},0.25f)
                 // 攻击者位置归位
                .Append(()=>{
                    percent += Time.deltaTime / durationTime;
                    atker.transform.localPosition = new Vector3(Mathf.Lerp(defPosX, atkPosX, percent), Mathf.Lerp(defPosY, atkPosY, percent), 0);
                },durationTime)
                // 将防御者和攻击者节点归位
                .Append(()=>{
                    atker.transform.SetParent(absUnitsTrans);
                    defener.transform.SetParent(enemyParent);
                    defener.transform.SetSiblingIndex(defener.transform.Find("body").GetComponent<AbsUnit>().id - 1);
                },0f)
                .Execute();
            }
        }
    }
}