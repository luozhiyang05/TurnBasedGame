using Assets.GameSystem.BattleSystem.Scripts;
using Framework;
using GlobalData;
using UnityEngine;

namespace Assets.GameSystem.CardSystem.Scripts.Cmd
{
    public struct AddHpData
    {
        public AbsUnit self;
        public AbsUnit target;
        public int addHp;
    }
    public class AddHpCmd : AbsCommand<AddHpData>
    {
        public override void Do(AddHpData addHpData)
        {
            base.Do(addHpData);
            if (addHpData.target.nowHp.Value + addHpData.addHp > addHpData.target.maxHp.Value)
            {
                addHpData.target.nowHp.Value = addHpData.target.maxHp.Value;
            }
            else
            {
                addHpData.target.nowHp.Value += addHpData.addHp;
            }
            Debug.LogWarning($"{GameManager.GetText(addHpData.self.unitName)}对{GameManager.GetText(addHpData.target.unitName)}回复了{addHpData.addHp}点血");
        }
    }
}