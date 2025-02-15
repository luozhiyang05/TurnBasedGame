using Assets.GameSystem.BattleSystem.Scripts;
using Assets.GameSystem.BattleSystem.Scripts.Effect;
using Framework;
using UnityEngine;
namespace Assets.GameSystem.CardSystem.Scripts.Cmd
{
    public struct DefData
    {
        public DefenceEffect defenceEffect;
        public AbsUnit self;
        public AbsUnit target;
        public int armor;
    }
    public class DefCmd : AbsCommand<DefData>
    {
        public override void Do(DefData defData)
        {
            base.Do(defData);
            var self = defData.self;
            var target = defData.target;
            var armor = defData.armor;
            var defenceEff = defData.defenceEffect;
            
            defenceEff.Init(self,target,armor);
            target.AddEffect(defenceEff);
            
            target.armor.Value += armor;
            Debug.LogWarning($"{self.gameObject.name}对{target.gameObject.name}增加{armor}点护甲");
        }
    }
}