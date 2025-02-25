using Assets.GameSystem.BattleSystem.Scripts.Effect;
using Assets.GameSystem.CardSystem.Scripts;
using Framework;
using Tool.ResourceMgr;

namespace Assets.GameSystem.SkillSystem.Scripts.Cmd
{
    public class AddSelfArmorSkillCmd : AbsCommand<SkillDataPacking>
    {
        public override void Do(SkillDataPacking skillDataPacking)
        {
            base.Do(skillDataPacking);
            // 获取叠甲效果
            var addArmarEffect = ResMgr.GetInstance().SyncLoad<CardLibrarySo>("卡牌库").GetBaseEffectById(skillDataPacking.skillData.effectId) as DefenceEffect;
            // 初始化叠甲效果
            addArmarEffect.InitDefenceEffData(skillDataPacking.user, skillDataPacking.target, skillDataPacking.skillData.duration, skillDataPacking.skillData.param);

            // 挂上效果
            skillDataPacking.target.AddEffect(addArmarEffect);
            skillDataPacking.target.armor.Value += skillDataPacking.skillData.param;
        }
    }
}