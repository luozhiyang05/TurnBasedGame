using Tool.CustomAttribute;
using UnityEngine;

namespace Assets.GameSystem.MusicNoteSystem.Scripts
{
    [CreateAssetMenu(fileName = "GlobalMusicSettingSO", menuName = "音乐资源/全局设置")]
    public class GlobalMusicSettingSO : ScriptableObject
    {
        [CustomPropertyText("Perfect时间")]
        public float perfectTime;
        [CustomPropertyText("Great时间")]
        public float greatTime;
        [CustomPropertyText("双击音符Perfect时间")]
        public float doublePerfectTime;
        [CustomPropertyText("双击Great时间")]
        public float doubleGreatTime;
        [CustomPropertyText("音符到达打击点距离")]
        public float distance;
    }
}