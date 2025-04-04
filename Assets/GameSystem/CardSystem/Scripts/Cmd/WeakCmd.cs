using Assets.GameSystem.BattleSystem.Scripts;
using Assets.GameSystem.BattleSystem.Scripts.Effect;
using Framework;
using GlobalData;
using UnityEngine;

namespace Assets.GameSystem.CardSystem.Scripts.Cmd
{
    public class WeakCmd : AbsCommand<CardCmdData>
    {
        public override void Do(CardCmdData cardCmdData)
        {
            base.Do(cardCmdData);
            var self = cardCmdData.self;
            var target = cardCmdData.target;
            var maxRoundCnt = cardCmdData.param1;
            var effect = cardCmdData.baseEffect as WeakEffect;

            effect.InitWeakEffData(self, target, maxRoundCnt);
            target.AddEffect(effect);

            cardCmdData.target.SetWeak(true);
        }
    }
}