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
    }
}