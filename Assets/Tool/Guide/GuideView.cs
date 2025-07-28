using Tool.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Tool.Guide
{
    public class GuideView : MonoBehaviour
    {
        public Transform main;
        public Transform maskPanel;
        public Text txt_guide;

        public void DisplayGuideTxt(string txt)
        {
            txt_guide.text = txt;
        }

        public void NoDisplayGuideTxt()
        {
            txt_guide.text = "";
        }
    }
}