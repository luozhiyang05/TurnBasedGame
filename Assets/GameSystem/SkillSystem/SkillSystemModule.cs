using Assets.GameSystem.BattleSystem;
using Assets.GameSystem.BattleSystem.Scripts;
using Assets.GameSystem.SkillSystem.Scripts.Cmd;
using Framework;
using GlobalData;
using Tool.ResourceMgr;
using Tool.Utilities;
using UnityEngine;
namespace Assets.GameSystem.SkillSystem
{
    public interface ISkillSystemModule : IModule
    {
        /// <summary>
        /// 检查是否能够释放该id的技能
        /// </summary>
        /// <param name="usedSkillIds"></param>
        /// <param name="skillId"></param>
        /// <param name="nowActCnt"></param>
        /// <returns></returns>
        public bool CheckIsHadSkill(QArray<int> usedSkillIds, int skillId, int nowActCnt);

        /// <summary>
        /// 使用技能
        /// </summary>
        /// <param name="skillId"></param>
        /// <param name="user"></param>
        public void UseSkill(int skillId, AbsUnit user);
    }

    public class SkillSystemModule : AbsModule, ISkillSystemModule
    {
        private SkillsSo skillsSo;
        protected override void OnInit()
        {
            skillsSo = ResMgr.GetInstance().SyncLoad<SkillsSo>("技能库");
        }

        public bool CheckIsHadSkill(QArray<int> usedSkillIds,int skillId, int nowActCnt)
        {
            var skillData = skillsSo.GetSkillDataById(skillId);
            if (skillData.isOnly && usedSkillIds.ContainValue(skillId)) return false;
            if (skillData == null) return false;
            if (nowActCnt < skillData.needActCnt) return false;
            return true;
        }

        public void UseSkill(int skillId, AbsUnit user)
        {
            // 根据技能Id，实例化技能命令
            // 根据技能类型，实例化一个skillDataPacking，填入user，target
            // 发送命令
            var skillData = skillsSo.GetSkillDataById(skillId);
            var skillDataPacking = new SkillDataPacking(skillData, user, skillData.useType == UseType.all ? null : (skillData.useType == UseType.target ? this.GetSystem<IBattleSystemModule>().GetPlayerUnit() : user));
            switch (skillData.id)
            {
                case 1001:
                    this.SendCmd<AddSelfArmorSkillCmd, SkillDataPacking>(skillDataPacking);
                    break;
                case 1002:
                    this.SendCmd<ResurrectionSkillCmd, SkillDataPacking>(skillDataPacking);
                    break;
            }

            Debug.Log(GameManager.GetText((user as Enemy).enemyData.enemyType.ToString()) + "使用技能" + GameManager.GetText(skillData.skillName));
        }
    }
}