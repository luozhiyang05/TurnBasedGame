using Assets.Wights.Scripts;
using GameSystem.MVCTemplate;

namespace Assets.GameSystem.RedPointSystem.Main
{
    public class RedPointSystemViewModel : BaseModel
    {
        public override void Init()
        {
        }

        /// <summary>
        /// 监听某些数据更改事件,可以通知view更新
        /// </summary>
        public override void BindListener()
        {
            //使用方法注册该红点路径，触发时返回1个红点
            RedPointMgr.GetInstance().RegisterWithFun(RedPointDef.ONE_TWO_TEST2, () => 1);
        }

        /// <summary>
        /// 移除事件
        /// </summary>
        public override void RemoveListener()
        {
        }
    }
}