using Assets.GameSystem.BattleSystem.Scripts.Effect;
using Assets.GameSystem.CardSystem.Scripts;
using Framework;
using Tool.ResourceMgr;

namespace Assets.GameSystem.SkillSystem.Scripts.Cmd
{
    public class ResurrectionSkillCmd : AbsCommand<SkillDataPacking>
    {
        public override void Do(SkillDataPacking skillDataPacking)
        {
            base.Do(skillDataPacking);
            // 获取复活效果
            var resurrectionEffect = ResMgr.GetInstance().SyncLoad<CardLibrarySo>("卡牌库").GetBaseEffectById(skillDataPacking.skillData.effectId) as ResurrectionEffect;
            resurrectionEffect.ResurrectionEffData(skillDataPacking.user, skillDataPacking.target, skillDataPacking.skillData.duration);
            // 挂载复活效果
            skillDataPacking.target.AddEffect(resurrectionEffect);
        }
    }
}