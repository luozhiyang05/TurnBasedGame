using UnityEngine;

namespace GameSystem.MVCTemplate
{
    public abstract class BasePanel : MonoBehaviour
    {
        protected virtual void AutoInitUI(){}
        protected abstract void OnInit();
        public virtual void OnShow(){}
        public virtual void OnHide(){}
    }
}