using Assets.GameSystem.BattleSystem.Scripts.Effect;
using Framework;
using GlobalData;
using Tool.ResourceMgr;
using UnityEngine;
namespace Assets.GameSystem.EffectsSystem
{
    public interface IEffectsSystemModule: IModule
    {
        public int GetEffectsCnt();
        public BaseEffect GetBaseEffectById(int id);
         public Sprite GetEffectIcon(int id);
         public string GetEffectName(int id);
         public string GetEffectDesc(int id);
    }

    public class EffectsSystemModule : AbsModule, IEffectsSystemModule
    {
        private EffectsSo effectsSo;
        protected override void OnInit()
        {
            effectsSo = ResMgr.GetInstance().SyncLoad<EffectsSo>("效果库");
        }
        public int GetEffectsCnt()
        {
            return effectsSo.baseEffectDatas.Count;
        }
        public BaseEffect GetBaseEffectById(int id)
        {
            return effectsSo.GetBaseEffectById(id);
        }

        public Sprite GetEffectIcon(int id)
        {
            var iconName = GameManager.GetText(effectsSo.GetBaseEffectById(id).iconName);
            return ResMgr.GetInstance().SyncLoad<Sprite>(GameManager.EffectIconPath + iconName);
        }

        public string GetEffectName(int id)
        {
            return GameManager.GetText(effectsSo.GetBaseEffectById(id).effName);
        }

        public string GetEffectDesc(int id)
        {
            return GameManager.GetText(effectsSo.GetBaseEffectById(id).effDesc);
        }
    }
}