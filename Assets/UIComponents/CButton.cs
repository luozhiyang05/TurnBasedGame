using System;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

namespace UIComponents
{
    [RequireComponent(typeof(Button))]
    public class CButton : Button
    {
        public Text Label;
        protected override void Start()
        {
            Label.text = "das";
        }
    }
}