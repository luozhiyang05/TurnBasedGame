using Assets.GameSystem.BattleSystem.Scripts;
using Framework;
using GlobalData;
using UnityEngine;

namespace Assets.GameSystem.CardSystem.Scripts.Cmd
{
    public class AddHpCmd : AbsCommand<CardCmdData>
    {
        public override void Do(CardCmdData cardCmdData)
        {
            base.Do(cardCmdData);
            if (cardCmdData.target.nowHp.Value + cardCmdData.param1 > cardCmdData.target.maxHp.Value)
            {
                cardCmdData.target.nowHp.Value = cardCmdData.target.maxHp.Value;
            }
            else
            {
                cardCmdData.target.nowHp.Value += cardCmdData.param1;
            }
            Debug.LogWarning($"{GameManager.GetText(cardCmdData.self.unitName)}对{GameManager.GetText(cardCmdData.target.unitName)}回复了{cardCmdData.param1}点血");
        }
    }
}