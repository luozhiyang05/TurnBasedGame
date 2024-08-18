using GameSystem.BattleSystem.Scripts;

namespace GameSystem.CardSystem.Scripts
{
    public abstract class DefCardSo : BaseCardSo
    {
        public override void DefenceToSelf(AbsUnit self)
        {
            OnDefenceToSelf(self);
        }

        public abstract void OnDefenceToSelf(AbsUnit self);
    }
}