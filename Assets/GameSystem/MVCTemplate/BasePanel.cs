using UnityEngine;

namespace GameSystem.MVCTemplate
{
    public abstract class BasePanel : MonoBehaviour
    {
        public bool UseMaskPanel => MaskPanel();
        public bool UseClickMaskPanel => ClickMaskPanel();

        protected virtual void AutoInitUI(){}
        protected abstract void OnInit();
        public abstract void OnShow();
        public abstract void OnHide();
        public virtual void OnRelease(){}

        public virtual bool MaskPanel()
        {
            return false;
        }

        public virtual bool ClickMaskPanel()
        {
            return false;
        }

        public virtual void OnClickMaskPanel() {}
    }
}