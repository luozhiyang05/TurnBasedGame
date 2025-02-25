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
        public int maxRoundCnt;
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
            var maxRoundCnt = defData.maxRoundCnt;
            var defenceEff = defData.defenceEffect;

            // 在叠甲命令中进一步初始化叠甲效果数据
            defenceEff.InitDefenceEffData(self, target, maxRoundCnt, armor);
            target.AddEffect(defenceEff);

            // 叠甲
            target.armor.Value += armor;
            Debug.LogWarning($"{self.gameObject.name}对{target.gameObject.name}增加{armor}点护甲");
        }
    }
}