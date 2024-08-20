using GameSystem.BattleSystem.Scripts;
using UnityEngine;

namespace GameSystem.CardSystem.Scripts
{
    public abstract class BaseCardSo : ScriptableObject
    {
        public string cardName;
        public int atk;
        public int armor;

        public abstract void UseCard(AbsUnit self,AbsUnit target);
    }
}