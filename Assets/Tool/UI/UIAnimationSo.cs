 using UnityEngine;
namespace Tool.UI
{
   [CreateAssetMenu(fileName = "UIAnimationSo", menuName = "UI工具/新建UI动画", order = 0)]
   public class UIAnimationSo : ScriptableObject 
   {
        [Header("动画参数")]
        public float tipsDisplayTime = 0.5f;
        
        [Header("动画曲线")]
        public AnimationCurve tipsAnimCurve;
   }
}