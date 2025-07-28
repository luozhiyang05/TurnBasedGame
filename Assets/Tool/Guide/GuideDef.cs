using System.Collections.Generic;
using UnityEngine;

namespace Tool.Guide
{

    public class GuideDef
    {
        //引导路径名
        public const string TEST = "TEST";

    }

    public class GuideData
    {
        public List<GameObject> guideGos;
        public List<string> texts;
        public GuideData()
        {
            guideGos = new List<GameObject>();
            texts = new List<string>();
        }
        public void SetGuideGos(params GameObject[] gos)
        {
            guideGos.AddRange(gos);
        }
        public void SetTexts(params string[] texts)
        {
            this.texts.AddRange(texts);
        }
        public void AddGuideGo(GameObject go)
        {
            guideGos.Add(go);
        }
        public void AddText(string text)
        {
            this.texts.Add(text);
        }
    }

}