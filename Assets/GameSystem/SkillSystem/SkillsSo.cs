namespace Assets.GameSystem.SkillSystem
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using Assets.GameSystem.BattleSystem.Scripts;
    using Tool.Utilities.CSV;
    using UnityEngine;

    public enum UseType
    {
        self,
        target,
        all
    }

    [Serializable]
    public class SkillData
    {
        public int id;
        public string skillName;
        public string desc;
        public int needActCnt;
        public UseType useType;
        public int effectId;
        public int duration;
        public int param;
        public bool isOnly;
    }

    public class SkillDataPacking
    {
        public SkillData skillData;
        public AbsUnit user;
        public AbsUnit target;
        public SkillDataPacking(SkillData skillData, AbsUnit user, AbsUnit target)
        {
            this.skillData = skillData;
            this.user = user;
            this.target = target;
        }
    }

    [CreateAssetMenu(fileName = "技能库", menuName = "SkillsSo", order = 0)]
    public class SkillsSo : ScriptableObject
    {
        public TextAsset textAsset;
        public List<SkillData> skillDatas;

        private void OnValidate() {
            if (textAsset!=null)
            {
                skillDatas.Clear();
                CsvKit.Read<SkillData>(textAsset, BindingFlags.Public | BindingFlags.Instance,ValueTuple=>{
                    skillDatas.Add(ValueTuple);
                });
            }
        }

        public SkillData GetSkillDataById(int id)
        {
            return skillDatas.Find(value=>value.id==id);
        }
    }
}