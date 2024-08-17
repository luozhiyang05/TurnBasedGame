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
        

        protected BaseCtrl(BaseModel model, EuiLayer viewLayer)
        {
            var modelName = model.ToString(); //XxxSystemModel
            viewName = modelName[(modelName.LastIndexOf('.') + 1)..].Replace("Model", "View");
            this.viewLayer = viewLayer;
            View = UIManager.GetInstance().LoadViewGo(viewName, viewLayer);
            Model = model;
            View.SetModel(Model);
            InitListener();
            OnCompleteLoad();
        }

        protected abstract void InitListener();

        protected void OnOpen()=>UIManager.GetInstance().OpenView(viewName, viewLayer);
        
        protected abstract void OnCompleteLoad();

        public IMgr Ins => Global.GetInstance();
    }
}