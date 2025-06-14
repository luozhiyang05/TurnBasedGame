using System;
using Tool.AudioMgr;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

namespace UIComponents
{
    [RequireComponent(typeof(Button))]
    public class CButton : Button
    {
        public Text Label;
        
        public string Text
        {
            get { return Label.text;}
            set { Label.text = value;}
        }
        
        public void Init(string content,string clickAudioPath = null)
        {
            Text = content;
            onClick.AddListener(() =>
            {
                if (clickAudioPath != null)
                {
                    AudioManager.GetInstance().PlayAudio(clickAudioPath);
                }
            });
        }
    }
}