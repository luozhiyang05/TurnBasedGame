using Assets.GameSystem.BattleSystem.Scripts.Effect;
using Framework;
using Tool.ResourceMgr;
namespace Assets.GameSystem.EffectsSystem
{
    public interface IEffectsSystemModule: IModule
    {
        public BaseEffect GetBaseEffectById(int id);
    }

    public class EffectsSystemModule : AbsModule, IEffectsSystemModule
    {
        private EffectsSo effectsSo;
        protected override void OnInit()
        {
            effectsSo = ResMgr.GetInstance().SyncLoad<EffectsSo>("效果库");
        }
        public BaseEffect GetBaseEffectById(int id)
        {
            return effectsSo.GetBaseEffectById(id);
        }

    }
}