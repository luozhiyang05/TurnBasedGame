using Assets.GameSystem.BattleSystem.Scripts;
using Framework;
using GlobalData;
using UnityEngine;

namespace Assets.GameSystem.CardSystem.Scripts.Cmd
{
    public class GetActPointCmd : AbsCommand<CardCmdData>
    {
        public override void Do(CardCmdData cardCmdData)
        {
            base.Do(cardCmdData);
            (cardCmdData.self as Player).ModifyActPoint(cardCmdData.param1);    // param1为获取的行动点
            Debug.LogWarning($"{GameManager.GetText(cardCmdData.self.unitName)}获取了{cardCmdData.param1}点行动点");
        }
    }
}