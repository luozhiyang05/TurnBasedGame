using GameSystem.MVCTemplate;
using Tool.Utilities;
using Tool.Utilities.Events;
using UnityEngine;
using UnityEngine.Events;

namespace GameSystem.CardSystem.ObsCard.Main
{
    public class ObsCardViewModel : BaseModel
    {
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
        
    }
}