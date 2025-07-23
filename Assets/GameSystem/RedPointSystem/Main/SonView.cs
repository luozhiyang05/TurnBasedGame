using Assets.Wights.Scripts;
using GameSystem.MVCTemplate;
using UIComponents;

namespace Assets.GameSystem.TemplateSystem.Main
{
    public class SonView : SubPanelView
    {
        public CButton btn_test1, btn_test2, btn_close;
        protected override void Init()
        {
            SetName("SonView");
            btn_test1.Text = "测试1";
            btn_test2.Text = "测试2";

            RedPointMgr.GetInstance().Register(btn_test1.gameObject, RedPointDef.ONE_TWO_TEST1);
            RedPointMgr.GetInstance().Register(btn_test2.gameObject, RedPointDef.ONE_TWO_TEST2);

            btn_close.AddListener(() =>
            {
                Close();
            });
        }

        protected override void Open(params object[] args)
        {
        }

        protected override void BindModelListener()
        {
        }

        protected override void RemoveModelListener()
        {
        }
    }
}