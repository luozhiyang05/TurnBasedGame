using Tool.Mono;
using Tool.UI;

namespace GameSystem.MVCTemplate
{
    public abstract class BaseTips : BaseView
    {
        protected BaseModel _baseModel;

        void OnEnable()
        {
            Init();
        }
        void Start()
        {
            OnShow();
        }
        protected abstract void Init();
        public abstract override void OnShow();
        public virtual void Open(BaseModel baseModel = null)
        {
            _baseModel = baseModel;
            gameObject.SetActive(true);
            if (UseMaskPanel) UIManager.GetInstance().OpenMaskPanel(this);
        }
        public override void OnHide()
        {
            gameObject.SetActive(false);
            UIManager.GetInstance().EnterPool(this);
            if (UseMaskPanel) UIManager.GetInstance().CloseMaskPanel();
        }

        protected override void BindModelListener(){}
        protected override void OnInit(){}

        public override void OnRelease()
        {
            _baseModel = null;   
        }

        /// <summary>
        /// 点击遮罩事件
        /// </summary>
        public override void OnClickMaskPanel()
        {
            OnHide();
        }
    }
}