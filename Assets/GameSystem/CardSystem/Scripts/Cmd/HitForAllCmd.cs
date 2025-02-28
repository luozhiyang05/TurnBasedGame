using Assets.GameSystem.BattleSystem;
using Assets.GameSystem.BattleSystem.Scripts;
using Framework;
using GlobalData;
using UnityEngine;

namespace Assets.GameSystem.CardSystem.Scripts.Cmd
{
    public class HitForAllCmd : AbsCommand<CardCmdData>
    {
        public override void Do(CardCmdData cardCmdData)
        {
            base.Do(cardCmdData);
            // 对全体敌人造成伤害
            var enemies = this.GetSystem<IBattleSystemModule>().GetNowWavaAllEnemies();
            for (int i = 0; i < enemies.Count; i++)
            {
                this.SendCmd<AtkCmd,AtkData>(new AtkData(){
                    self = cardCmdData.self,
                    target = enemies[i],
                    atk = cardCmdData.param1
                });
            }
            // 对玩家本身造成伤害
            this.SendCmd<AtkCmd,AtkData>(new AtkData(){
                self = cardCmdData.self,
                target = cardCmdData.target,
                atk = cardCmdData.param1
            });
            Debug.LogWarning($"{GameManager.GetText(cardCmdData.self.unitName)}对{GameManager.GetText(cardCmdData.target.unitName)}回复了{cardCmdData.param1}点血");
        }
    }
}