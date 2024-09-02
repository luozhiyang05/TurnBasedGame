using Framework;
using Tool.UI;

namespace GameSystem.MVCTemplate
{
    /// <summary>
    /// 功能：
    /// 连通model和view
    /// 将view层的某些业务放入ctrl
    /// 控制view的打开和关闭
    /// </summary>
    public abstract class BaseCtrl : ICanGetSystem
    {
        protected BaseModel Model;
        protected BaseView View;
        
        protected string viewName;
        protected EuiLayer viewLayer;
        

        protected BaseCtrl()
        {
            Model = GetModel();
            View = GetView();
            View.SetModel(Model);
            View.SetClose(OnClose);
            Init();
            InitListener();
        }

        protected abstract void InitListener();
        
        protected abstract void RemoveListener();

        protected abstract void Init();

        protected void OnOpen()=>UIManager.GetInstance().OpenView(viewName, viewLayer);

        protected abstract BaseModel GetModel();

        protected abstract BaseView GetView();

        private void OnClose()
        {
            RemoveListener();
            Model.Dispose();
        }

        public IMgr Ins => Global.GetInstance();
    }
}