using GameSystem.BattleSystem.Scripts;
using UnityEngine;

namespace GameSystem.CardSystem.Scripts
{
    public class BaseCardSo : ScriptableObject
    {
        public string cardName;
        public int atk;
        public int armor;
        
        public virtual void AttackToTarget(AbsUnit self, AbsUnit target){}
        
        public virtual void DefenceToSelf(AbsUnit self){}
    }
}