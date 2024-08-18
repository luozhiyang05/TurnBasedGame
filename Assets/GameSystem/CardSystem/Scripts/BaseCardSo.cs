using GameSystem.BattleSystem.Scripts;
using UnityEngine;

namespace GameSystem.CardSystem.Scripts
{
    public class BaseCardSo : ScriptableObject
    {
        public int atk;
        
        public virtual void AttackToTarget(AbsUnit self, AbsUnit target){}
    }
}