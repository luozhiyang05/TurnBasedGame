using UnityEngine;

namespace GameSystem.MVCTemplate
{
    public abstract class BasePanel : MonoBehaviour
    {
        public bool UseMaskPanel => MaskPanel();
        public bool UseClickMaskPanel => ClickMaskPanel();

        protected virtual void AutoInitUI(){}
        protected abstract void OnInit();
        public virtual void OnShow(){}
        public virtual void OnHide(){}

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