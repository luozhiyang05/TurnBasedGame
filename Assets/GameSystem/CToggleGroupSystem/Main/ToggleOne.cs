using GameSystem.MVCTemplate;
using UnityEngine.UI;

namespace Assets.GameSystem.TemplateSystem.Main
{
    public class ToggleOne : SubPanelView
    {
        public Text txt_tip;
        protected override void Init()
        {
            SetName("ToggleOne");
        }

        protected override void Open(params object[] args)
        {
            txt_tip.text = args[1].ToString();
        }

        protected override void BindModelListener()
        {
        }

        protected override void RemoveModelListener()
        {
        }
    }
}