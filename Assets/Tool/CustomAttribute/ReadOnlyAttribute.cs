
using UnityEngine;

namespace Tool.CustomAttribute
{
    public class ReadOnlyAttribute : PropertyAttribute
    {
        public Color color = default;
        public ReadOnlyAttribute(float r, float g, float b)
        {
            this.color = new Color(r, g, b);
        }
        public ReadOnlyAttribute()
        {}
    }
}