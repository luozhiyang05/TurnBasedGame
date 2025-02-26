using Assets.GameSystem.BattleSystem;
using Assets.GameSystem.BattleSystem.Scripts;
using Assets.GameSystem.SkillSystem.Scripts.Cmd;
using Framework;
using Tool.ResourceMgr;
using UnityEngine;
namespace Assets.GameSystem.SkillSystem
{
    public interface ISkillSystemModule : IModule
    {
        public bool CheckIsHadSkill(int skillId, int nowActCnt);

        public void UseSkill(int skillId, AbsUnit user);
    }

    public class SkillSystemModule : AbsModule, ISkillSystemModule
    {
        private SkillsSo skillsSo;
        protected override void OnInit()
        {
            skillsSo = ResMgr.GetInstance().SyncLoad<SkillsSo>("技能库");
        }

        public bool CheckIsHadSkill(int skillId, int nowActCnt)
        {
            var skillData = skillsSo.GetSkillDataById(skillId);
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
            switch (skillData.id)
            {
                case 1001:
                    var skillDataPacking = new SkillDataPacking(skillData, user, skillData.useType == UseType.all ? null : (skillData.useType == UseType.target ? this.GetSystem<IBattleSystemModule>().GetPlayerUnit() : user));
                    this.SendCmd<AddSelfArmorSkillCmd, SkillDataPacking>(skillDataPacking);
                    break;
            }

            Debug.Log(user.transform.parent + "使用技能" + skillData.skillName);
        }
    }
}