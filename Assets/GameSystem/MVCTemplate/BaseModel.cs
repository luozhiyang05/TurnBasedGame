using Framework;

namespace GameSystem.MVCTemplate
{
    /// <summary>
    /// 功能：
    /// 处理属于该系统临时的数据业务
    /// </summary>
    public abstract class BaseModel : ICanGetSystem
    {
        protected BaseModel() { }

        public abstract void Init();

        public abstract void BindListener();

        public abstract void RemoveListener();

        public IMgr Ins => Global.GetInstance();
    }
}