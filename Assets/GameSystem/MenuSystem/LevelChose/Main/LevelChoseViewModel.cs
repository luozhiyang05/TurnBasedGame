using GameSystem.MVCTemplate;
using UnityEngine;

namespace GameSystem.MenuSystem.LevelChose.Main
{
    public class LevelChoseViewModel : BaseModel
    {
        private int _nowChooseLevelId;
        protected override void OnInit()
        {
            _nowChooseLevelId = -1;
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
        public void SetChooseLevelId(int id)
        {
            _nowChooseLevelId = id;
        }
        public int GetChooseLevelId()
        {
            return _nowChooseLevelId;
        }
    }
}