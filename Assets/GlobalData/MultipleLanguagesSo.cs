namespace GlobalData
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using Tool.Utilities.CSV;
    using UnityEngine;
    public class LanguagesDataPacking{
        public string key;
        public string chinese;
        public string english;
    }

    [CreateAssetMenu(fileName = "多语言配置", menuName = "MultipleLanguagesSo", order = 0)]
    public class MultipleLanguagesSo : ScriptableObject
    {
        public TextAsset textAsset;
        public Dictionary<string, LanguagesDataPacking> languagesDic = new Dictionary<string, LanguagesDataPacking>();
        private void OnValidate()
        {
            if (textAsset != null)
            {
                languagesDic.Clear();
                CsvKit.Read<LanguagesDataPacking>(textAsset, BindingFlags.Public | BindingFlags.Instance, value =>
              {
                  languagesDic.Add(value.key, value);
              });
            }
        }

        public String GetLanguageText(string key)
        {
            if(languagesDic.TryGetValue(key, out LanguagesDataPacking value))
            {
                string text = "";
                switch (GameManager.languages)
                {
                    case Languages.中文:
                        text = value.chinese;
                        break;
                    case Languages.English:
                        text = value.english;
                        break;
                }
                return text;
            }
            throw new Exception("找不到多语言");
        }
    }
}