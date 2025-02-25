using Assets.GameSystem.BattleSystem.Scripts;
using Framework;
using UnityEngine;

namespace Assets.GameSystem.CardSystem.Scripts.Cmd
{
    public struct AtkData
    {
        public AbsUnit self;
        public AbsUnit target;
        public int atk;
    }
    public class AtkCmd : AbsCommand<AtkData>
    {
        public override void Do(AtkData atkData)
        {
            base.Do(atkData);
            var self = atkData.self;
            var target = atkData.target;
            var atk = atkData.atk;
            var reduceHp = target.armor.Value - atk;
            if (reduceHp < 0)
            {
                var targetNowHp = target.nowHp.Value - Mathf.Abs(reduceHp);
                Debug.LogWarning($"{self.transform.parent.name}对{target.transform.parent.name}{Mathf.Abs(reduceHp)}点伤害,{target.transform.parent.name}目前血量为{targetNowHp}/{target.maxHp},护甲为{target.armor}");
                target.nowHp.Value = targetNowHp;
                target.armor.Value = 0;
            }
            else
            {
                target.armor.Value -= atk;
                Debug.LogWarning($"{self.transform.parent.name}对{target.transform.parent.name}造成{atk}点护甲伤害,目前{target.transform.parent.name}护甲为{target.armor}");
            }
        }
    }
}