namespace Assets.GameSystem.EffectsSystem
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using Assets.GameSystem.BattleSystem.Scripts.Effect;
    using Tool.Utilities.CSV;
    using UnityEngine;

    [Serializable]
    public class BaseEffectData
    {
        public int id;
        public string effName;
        public string effDesc;
        public string iconName;
        public bool isDieEff;
    }

    [CreateAssetMenu(fileName = "效果库", menuName = "EffectsSo", order = 0)]
    public class EffectsSo : ScriptableObject
    {
        public TextAsset effAsset;
        public List<BaseEffectData> baseEffectDatas = new List<BaseEffectData>();

        private void OnValidate()
        {
            if (effAsset != null)
            {
                baseEffectDatas.Clear();
                CsvKit.Read<BaseEffectData>(effAsset, BindingFlags.Public | BindingFlags.Instance, value =>
               {
                   baseEffectDatas.Add(value);
               });
            }
        }



        /// <summary>
        /// 根据效果id获取效果实例（包含了id，效果名，效果描述的效果实例）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public BaseEffect GetBaseEffectById(int id)
        {
            var baseEffectData = baseEffectDatas.Find(value => value.id == id) ?? throw new Exception("找不到对应的效果");

            // 返回一个具体的效果，具体参数在技能或者卡牌配置中赋值
            switch (id)
            {
                case 1:
                    var defEff = new DefenceEffect();
                    defEff.InitBaseData(id, baseEffectData.effName, baseEffectData.effDesc, baseEffectData.iconName, baseEffectData.isDieEff);
                    return defEff;
                case 3:
                    var resEff = new ResurrectionEffect();
                    resEff.InitBaseData(id, baseEffectData.effName, baseEffectData.effDesc, baseEffectData.iconName, baseEffectData.isDieEff);
                    return resEff;
                default:
                    throw new Exception("找不到对应的效果");
            }
        }
    }
}