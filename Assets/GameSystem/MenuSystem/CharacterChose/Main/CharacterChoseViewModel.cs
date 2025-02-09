using GameSystem.MVCTemplate;
using UnityEngine;

namespace GameSystem.MenuSystem.CharacterChose.Main
{
    public class CharacterChoseViewModel : BaseModel
    {
        private int _nowCharacterId = -1;
        protected override void OnInit()
        {
        }

        /// <summary>
        /// 监听某些数据更改事件,可以通知view更新
        /// </summary>
        public override void BindListener()
        {
        }

        /// <summary>
        /// 移除事件
        /// </summary>
        public override void RemoveListener()
        {
        }

        public void SetChoseCharacter(int id)
        {
            _nowCharacterId = id;
        }
        public int GetChoseCharacterId()
        {
            return _nowCharacterId;
        }
    }
}