using UnityEngine;


namespace Tool.CustomAttribute
{
    public class CustomPropertyTextAttribute : PropertyAttribute
    {
        public string propertyName;
        public CustomPropertyTextAttribute(string propertyName = default)
        {
            this.propertyName = propertyName;
        }
    }
}