using Tool.Mono;
using Tool.UI;

namespace GameSystem.MVCTemplate
{
    public abstract class BaseTips : BasePanel
    {
        public string path;
        protected BaseModel _baseModel;

        void Awake()
        {
            OnInit();
        }

        void Update()
        {
        }

        protected override void OnInit()
        {
        }

        public virtual void Init(BaseModel baseModel)
        {
            _baseModel = baseModel;
        }

        public override void OnShow()
        {
            gameObject.SetActive(true);
            if (UseMaskPanel) UIManager.GetInstance().OpenMaskPanel(this);

            ActionKit.GetInstance().RemoveTimer(GetInstanceID() + nameof(UnLoad));
        }

        public override void OnHide()
        {
            gameObject.SetActive(false);
            if (UseMaskPanel) UIManager.GetInstance().CloseMaskPanel();

            //10秒后销毁
            ActionKit.GetInstance().DelayTime(5f, GetInstanceID() + nameof(UnLoad), UnLoad);
        }

        private void UnLoad()
        {
            UIManager.GetInstance().UnloadTips(path);
        }

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