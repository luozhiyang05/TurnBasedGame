using System.Collections;
using Tool.AudioMgr;
using Tool.ResourceMgr;
using Tool.UI;
using UnityEngine;

namespace GameSystem.MVCTemplate
{
    public abstract class BaseTips : BaseView
    {
        protected BaseModel _baseModel;
        public UIAnimationSo UIAnimationSo => ResMgr.GetInstance().SyncLoad<UIAnimationSo>("UIAnimationSo");
        public GameObject main;

        public IEnumerator ShowAnimation()
        {
            main.transform.localScale = Vector3.zero;
            if (UseMaskPanel) UIManager.GetInstance().OpenMaskPanel(this);
            float time = 0;
            while (time < 1)
            {
                time += Time.deltaTime / UIAnimationSo.tipsDisplayTime;
                main.transform.localScale = new Vector3(UIAnimationSo.tipsAnimCurve.Evaluate(time), UIAnimationSo.tipsAnimCurve.Evaluate(time), UIAnimationSo.tipsAnimCurve.Evaluate(time));
                yield return null;
            }
        }

        protected IEnumerator HideAnimation()
        {
            float time = 0;
            while (time < 1)
            {
                time += Time.deltaTime / UIAnimationSo.tipsDisplayTime;
                main.transform.localScale = new Vector3(UIAnimationSo.tipsAnimCurve.Evaluate(1 - time), UIAnimationSo.tipsAnimCurve.Evaluate(1 - time), UIAnimationSo.tipsAnimCurve.Evaluate(1 - time));
                yield return null;
            }
            gameObject.SetActive(false);
            UIManager.GetInstance().EnterPool(this);
            if (UseMaskPanel) UIManager.GetInstance().CloseMaskPanel();
        }

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
            if (useAudio)
            {
                PlayAudio(EAudioType.Effect);
                PlayAudio(EAudioType.Bgm);
            }
            _baseModel = baseModel;
            gameObject.SetActive(true);
            StartCoroutine(ShowAnimation());

        }
        public override void OnHide()
        {
            if (useAudio)
            {
                PlayAudio(EAudioType.Effect, false);
                CloseBgm();
            }
            StartCoroutine(HideAnimation());
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